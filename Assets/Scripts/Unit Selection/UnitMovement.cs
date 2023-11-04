using System.Collections.Generic;
using DG.Tweening;
using Grid_System;
using InputSystem;
using Pathfinding;
using UnityEngine;

namespace Unit_Selection
{
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] private float moveTimePerGrid;
        [SerializeField] private LayerMask ground;

        public bool active;
        
        private void Start()
        {
            InputManager.Instance.OnMouseRightClickDown += TriggerMove;
        }

        private void TriggerMove()
        {
            if (!active)
                return;
            RaycastHit2D hit = Physics2D.Raycast(InputManager.Instance.GetMousePosToWorldPos(), Vector2.zero, Mathf.Infinity, ground);
            if (hit)
            {
                Move(AStar.FindPath(GridController.Instance.GetGridPart(transform.position),
                    GridController.Instance.GetGridPart(hit.point)));
            }
        }

        private void Move(List<GridPart> gridParts)
        {
            if (gridParts==null)
            {
                return;
            }
            DOTween.Kill(gameObject.name, true);

            var sequence = DOTween.Sequence();
            
            for (int i = 0; i < gridParts.Count; i++)
            {
                var targetGrid = gridParts[i];
                var targetPosition = gridParts[i].transform.position;
                sequence.Append(transform.DOMove(targetPosition, moveTimePerGrid).OnKill((() =>
                {
                    transform.position = targetPosition;
                    targetGrid.Empty = false;
                }))).SetEase(Ease.Linear).SetId(gameObject.name);
            }
        }
    }
}