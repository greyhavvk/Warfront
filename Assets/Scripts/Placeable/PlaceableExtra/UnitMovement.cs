using System.Collections.Generic;
using System.Linq;
using Grid_System;
using Managers;
using Pathfinding;
using UnityEngine;

namespace Placeable.PlaceableExtra
{
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] private float moveTimePerGrid;

        public IGetPiecePosition GetPiecePosition
        { private get; set; }
        
        private float _time;
        private bool _followingTarget;
        private bool _hasRequest;
        private bool _onMoving;
        private bool _triggerMoving;

        private List<Transform> _requestedPossiblePoints;
        private List<Transform> _possiblePoints;

        public IGridPart LastMoveGrid { get; set; }
        private IGridPart _destinationGrid;
        private IGridPart _movingGrid;

        private void Update()
        {
            if (_hasRequest && !_onMoving)
            {
                TriggerMovement();
            }
            else if (_triggerMoving)
            {
                Movement();
            }
        }

        private void Movement()
        {
            if (!_onMoving)
            {
                if (_destinationGrid == null || _destinationGrid.Unit!=transform  || _followingTarget)
                    _destinationGrid = SelectDestinationPoint();
                if (_destinationGrid == null)
                {
                    ResetMove();
                    return;
                }
                var path = FindPath();
                if (path == null)
                {
                    ResetMove();
                    return;
                }

                _movingGrid = path[0];
                LastMoveGrid.Unit = null;
                LastMoveGrid.PiecePosition = null;
                LastMoveGrid.Empty = true;

                _movingGrid.Empty = false;
                _movingGrid.Unit = transform;
                _movingGrid.PiecePosition = GetPiecePosition;
                
                _onMoving = true;
            }

            Move();
            CheckMoveIsComplete();
        }

        public void MoveRequest(IGridPart gridPart)
        {
            _requestedPossiblePoints = new List<Transform>();
            if (gridPart.Unit!=null)
            {
                if (gridPart.Unit.CompareTag("unit"))
                {
                    if (Vector2.Distance(gridPart.Unit.position, transform.position)<=Mathf.Sqrt(2f))
                    {
                        return;
                    }
                    _requestedPossiblePoints.Add(gridPart.Unit);
                }
                else
                {
                    var piecePosition = gridPart.Unit.GetComponent<IGetPiecePosition>();
                    _requestedPossiblePoints=LookPossibleNearPoints(piecePosition.Pieces);
                }
               
                _followingTarget = gridPart.Unit.CompareTag("unit");
            }
            else
            {
                _followingTarget = false;
                _requestedPossiblePoints.Add(gridPart.Transform);
            }

            _hasRequest = true;
        }

        private List<Transform> LookPossibleNearPoints(List<Transform> piecePositionPieces)
        {
            var searchedPoints = new List<Transform>();
            
            foreach (var targetGridPart in piecePositionPieces.Select(possibleSpawnPoint => GridManager.GetPart.GetGridPart(possibleSpawnPoint.position)).Where(targetGridPart => targetGridPart != null))
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i==0 && j==0)
                            continue;
                        int newX = targetGridPart.Width + i;
                        int newY = targetGridPart.High + j;
                        
                        var gridPart = GridManager.GetPart.GetGridPart(newX, newY);
                        if (gridPart == null) continue;
                        if (searchedPoints.Contains(gridPart.Transform)) continue;
                        searchedPoints.Add(gridPart.Transform);
                    }
                }
            }

            return searchedPoints;
        }

        public void StopMove()
        {
            ResetMove();
            _requestedPossiblePoints = null;
            _hasRequest = false;
        }

        private void ResetMove()
        {
            _time = 0;
            _possiblePoints = null;
            _triggerMoving = false;
            _onMoving = false;
            _destinationGrid = null;
            _movingGrid = null;
        }

        private void TriggerMovement()
        {
            _time = 0;
            _hasRequest = false;
            _triggerMoving = true;
            _possiblePoints = _requestedPossiblePoints;
            _requestedPossiblePoints = null;
        }

        private void CheckMoveIsComplete()
        {
            if (_time / moveTimePerGrid>=1)
            {
                _onMoving = false;
                _time = 0;
                LastMoveGrid = _movingGrid;
                _movingGrid = null;
                if (_followingTarget)
                {
                    if (Vector2.Distance(_possiblePoints[0].position, transform.position)<=Mathf.Sqrt(2f) || !_possiblePoints[0].gameObject.activeSelf)
                    {
                        ResetMove();
                    }
                }
                else if (_destinationGrid==LastMoveGrid)
                {
                    ResetMove();
                }
                else if (!_destinationGrid.Empty && IsNear(_possiblePoints))
                {
                    ResetMove();
                }
            }
        }

        private void Move()
        {
            _time += Time.deltaTime;
            transform.position = Vector3.Lerp(LastMoveGrid.Transform.position, _movingGrid.Transform.position, _time / moveTimePerGrid);
        }

        private List<IGridPart> FindPath()
        {
            var path = AStar.FindPath(LastMoveGrid, _destinationGrid, transform, out _);
            return path;
        }

        private IGridPart SelectDestinationPoint()
        {
            IGridPart gridPart;
            if (_possiblePoints==null || _possiblePoints.Count==0)
            {
                return null;
            }
            
            if (_possiblePoints.Count>1)
            {
                gridPart = FindNearestNeighbor(_possiblePoints);
            }
            else
            {
                gridPart = GridManager.GetPart.GetGridPart(_possiblePoints[0].position);
                if (gridPart!=null && !gridPart.Empty)
                {
                    gridPart = FindNearestNeighbor(_possiblePoints);
                }
            }

            return gridPart;
        }

        private IGridPart FindNearestNeighbor(List<Transform> searchPoints)
        {
            
            if (searchPoints==null  || searchPoints.Count==0)
            {
                return null;
            }
            if (IsNear(searchPoints))
            {
                return null;
            }
            
            var searchPositions = new List<Vector2>();
            foreach (var searchPoint in searchPoints)
            {
                searchPositions.Add(searchPoint.position);
            }

            var outerPoints = GridManager.GetPart.FindEmptyPoints(searchPositions);
            if (outerPoints==null || outerPoints.Count==0)
            {
                return null;
            }

            var nearest = outerPoints[0];
            foreach (var outerPoint in outerPoints)
            {
                var pathOne = AStar.FindPath(LastMoveGrid, nearest, transform,
                    out float costOne);
                var pathTwo = AStar.FindPath(LastMoveGrid, outerPoint, transform,
                    out float costTwo);

                if (pathTwo!=null)
                {
                    if (pathOne!=null)
                    {
                        nearest = costTwo < costOne ? outerPoint : nearest;
                    }
                    else
                    {
                        nearest = outerPoint;
                    }
                }
            }

            return AStar.FindPath(LastMoveGrid, nearest, transform,
                out _)==null ? null : nearest;
        }

        private bool IsNear(List<Transform> searchPoints)
        {
            return searchPoints.Any(searchPoint => (Vector2)searchPoint.position == (Vector2)transform.position);
        }
    }
}