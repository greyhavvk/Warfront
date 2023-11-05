using System.Collections;
using Grid_System;
using InputSystem;
using Pathfinding;
using UnityEngine;

namespace Unit_Selection
{
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] private float moveTimePerGrid;
        [SerializeField] private LayerMask canTakeDamage;

        private Transform _lastTarget;
        private GridPart _lastMoveGrid;
        private bool _onMoving;

        private void Start()
        {
            _lastMoveGrid = GridManager.Instance.GetGridPart(transform.position);
            transform.position = _lastMoveGrid.transform.position;
            _lastMoveGrid.Empty = false;
        }

        public void TriggerMove(RaycastHit2D hit)
        {
            StopCoroutine(Move());
            var gridPart = GridManager.Instance.GetGridPart(hit.point);
            if (gridPart == null)
                return;
            var hitUnit = Physics2D.Raycast(InputManager.Mouse.GetMousePosToWorldPos(), Vector2.zero, Mathf.Infinity, canTakeDamage);
            if (hitUnit)
            {
                _lastTarget = hitUnit.collider.gameObject.CompareTag("unit") ? hitUnit.collider.transform : gridPart.transform;
            }
            else
            {
                _lastTarget = gridPart.transform;
            }
            StartCoroutine(Move());
        }

        private IEnumerator Move()
        {
            var elapsedTime = 0f;
            var moveDuration = moveTimePerGrid;

            if (!_lastTarget.gameObject.activeSelf)
            {
                yield break;
            }

            var gridParts = AStar.FindPath(GridManager.Instance.GetGridPart(transform.position),
                GridManager.Instance.GetGridPart(_lastTarget.position), transform);
            if (gridParts == null || gridParts.Count == 0)
                yield break;


            Vector3 startPos = transform.position;
            var gridPart = _onMoving ? _lastMoveGrid : gridParts[0];
            _onMoving = true;
            var startGridPart = _lastMoveGrid;
            _lastMoveGrid = gridPart;
            gridPart.unit = transform;
            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(startPos, gridPart.transform.position, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            gridPart.Empty = false;
            startGridPart.Empty = true;
            gridPart.unit = null;
            _onMoving = false;
            if (_lastTarget.gameObject.activeSelf)
                if (Vector3.Distance(transform.position, _lastTarget.position) > 1.41f)
                {
                    StartCoroutine(Move());
                }
        }

        public void StopMove()
        {
            StopCoroutine(Move());
        }
    }
}