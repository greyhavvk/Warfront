using System;
using System.Collections.Generic;
using Grid_System;
using UnityEngine;

namespace Pathfinding
{
    public static class AStar
    {
        public static List<IGridPart> FindPath(IPathfinding startGridPart, IPathfinding targetGridPart, Transform moveObject)
        {
            if (targetGridPart.IsObstacle || (targetGridPart.Unit!=null  &&  targetGridPart.Unit!=moveObject))
            {
                var newTarget = FindClosestWalkableGridPart(targetGridPart, startGridPart, moveObject);
                targetGridPart = newTarget;
            }
            
            return CalculatePath(startGridPart, targetGridPart, moveObject, out var gridParts) ? gridParts : null;
        }

        private static bool CalculatePath(IPathfinding startGridPart, IPathfinding targetGridPart, Transform moveObject, out List<IGridPart> gridParts)
        {
            List<IPathfinding> openSet = new List<IPathfinding>();
            HashSet<IPathfinding> closedSet = new HashSet<IPathfinding>();
            openSet.Add(startGridPart);

            while (openSet.Count > 0)
            {
                IPathfinding currentGridPart = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentGridPart.FCost || (openSet[i].FCost == currentGridPart.FCost &&
                                                                     openSet[i].HCost < currentGridPart.HCost))
                    {
                        currentGridPart = openSet[i];
                    }
                }

                openSet.Remove(currentGridPart);
                closedSet.Add(currentGridPart);

                if (currentGridPart == targetGridPart && !(currentGridPart.IsObstacle || (currentGridPart.Unit!=null  &&  currentGridPart.Unit!=moveObject)))
                {
                    {
                        gridParts = RetracePath(startGridPart, targetGridPart);
                        return true;
                    }
                }

                foreach (var neighbor in GetNeighbors(currentGridPart, moveObject))
                {
                    if (closedSet.Contains(neighbor) || (neighbor.IsObstacle || (neighbor.Unit!=null  &&  neighbor.Unit!=moveObject)))
                    {
                        continue;
                    }

                    int newCostToNeighbor = currentGridPart.GCost + GetDistance(currentGridPart, neighbor);
                    if (newCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                    {
                        neighbor.GCost = newCostToNeighbor;
                        neighbor.HCost = GetDistance(neighbor, targetGridPart);
                        neighbor.CameFrom = currentGridPart;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            gridParts = null;
            return false;
        }

        private static List<IPathfinding> GetNeighbors(IPathfinding gridPart, Transform moveObject)
        {
            List<IPathfinding> neighbors = new List<IPathfinding>();
            int gridSizeX = GridManager.Instance.SizeX;
            int gridSizeY = GridManager.Instance.SizeY;

            int[] offsetX = { 0, 0, 1, -1, 1, -1, 1, -1 };
            int[] offsetY = { 1, -1, 0, 0, 1, -1, -1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newX = gridPart.Width + offsetX[i];
                int newY = gridPart.High + offsetY[i];

                if (newX >= 0 && newX < gridSizeX && newY >= 0 && newY < gridSizeY)
                {
                    IPathfinding neighbor = GridManager.Instance.GetGridPart(newX, newY).Pathfinding;
                    if (!(neighbor.IsObstacle || (neighbor.Unit!=null  &&  neighbor.Unit!=moveObject)))
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }


        private static List<IGridPart> RetracePath(IPathfinding startGridPart, IPathfinding endGridPart)
        {
            List<IGridPart> path = new List<IGridPart>();
            IPathfinding currentGridPart = endGridPart;

            while (currentGridPart != startGridPart)
            {
                path.Add(currentGridPart.gridPart);
                currentGridPart = currentGridPart.CameFrom;
            }

            path.Reverse();
            return path;
        }

        private static int GetDistance(IPathfinding gridPartA, IPathfinding gridPartB)
        {
            int distX = Math.Abs(gridPartA.Width - gridPartB.Width);
            int distY = Math.Abs(gridPartA.High - gridPartB.High);

            return distX + distY;
        }

        private static IPathfinding FindClosestWalkableGridPart(IPathfinding targetGridPart, IPathfinding startGridPart, Transform moveObject)
        {
            int gridSizeX = GridManager.Instance.SizeX;
            int gridSizeY = GridManager.Instance.SizeY;
            int maxDistance = Math.Max(gridSizeX, gridSizeY);
            int closestDistance = int.MaxValue;

            for (int distance = 1; distance < maxDistance; distance++)
            {
                var closestList = new List<IPathfinding>();
                for (int i = -distance; i <= distance; i++)
                {
                    for (int j = -distance; j <= distance; j++)
                    {
                        int newX = targetGridPart.Width + i;
                        int newY = targetGridPart.High + j;

                        if (newX >= 0 && newX < gridSizeX && newY >= 0 && newY < gridSizeY)
                        {
                            IPathfinding gridPart = GridManager.Instance.GetGridPart(newX, newY).Pathfinding;
                            if (!(gridPart.IsObstacle || (gridPart.Unit!=null  &&  gridPart.Unit!=moveObject)))
                            {
                                int currentDistance = Math.Abs(newX - targetGridPart.Width) + Math.Abs(newY - targetGridPart.High);
                                if (currentDistance < closestDistance)
                                {
                                    closestDistance = currentDistance;
                                    closestList.Add(gridPart);
                                }
                            }
                        }
                    }
                }

                if (closestList.Count>0)
                {
                    var closest = closestList[0];
                    for (int i = 1; i < closestList.Count; i++)
                    {
                        if (closestList[i].FCost < closest.FCost)
                        {
                            closest = closestList[i];
                        }
                    }

                    if (CalculatePath(startGridPart, closest,moveObject, out _))
                    {
                        return closest;
                    }
                }
          
            }

            return null;
        }

    }
}