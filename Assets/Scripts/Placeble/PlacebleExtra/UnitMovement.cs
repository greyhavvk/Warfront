using System.Collections;
using Grid_System;
using InputSystem;
using Pathfinding;
using UnityEngine;

namespace Placeble.PlacebleExtra
{
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] private float moveTimePerGrid;
        [SerializeField] private LayerMask canTakeDamage;

        private Transform _lastTarget;
        private IGridPart _lastMoveGrid;
        private bool _onMoving;

        public IGridPart LastMoveGrid
        {
            set => _lastMoveGrid = value;
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
                _lastTarget = hitUnit.collider.gameObject.CompareTag("unit") ? hitUnit.collider.transform : gridPart.Transform;
            }
            else
            {
                _lastTarget = gridPart.Transform;
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
            var gridParts = AStar.FindPath(GridManager.Instance.GetGridPart(transform.position).Pathfinding,
                GridManager.Instance.GetGridPart(_lastTarget.position).Pathfinding, transform);
            if (gridParts == null || gridParts.Count == 0)
                yield break;

            Vector3 startPos = transform.position;
            var gridPart = _onMoving ? _lastMoveGrid : gridParts[0];
            _onMoving = true;
            var startGridPart = _lastMoveGrid;
            _lastMoveGrid = gridPart;
            gridPart.Unit = transform;
            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(startPos, gridPart.Transform.position, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            gridPart.Empty = false;
            startGridPart.Empty = true;
            gridPart.Unit = null;
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