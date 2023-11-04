using System;
using System.Collections.Generic;
using DG.Tweening;
using Grid_System;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

namespace Unit_Selection
{
    public class UnitMovement : MonoBehaviour
    {
        private Camera _camera;
        [SerializeField] private float moveTimePerGrid;
        [SerializeField] private LayerMask ground;
        
        
        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, ground);
                if (hit)
                {
                    Move(AStar.FindPath(GridController.Instance.GetGridPart(transform.position), GridController.Instance.GetGridPart(hit.point)));
                }
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