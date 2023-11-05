using System;
using System.Collections.Generic;
using Grid_System;
using UnityEngine;

namespace Pathfinding
{
    public static class AStar
    {
        public static List<GridPart> FindPath(GridPart startGridPart, GridPart targetGridPart, Transform moveObject)
        {
            if (targetGridPart.IsObstacle || (targetGridPart.unit!=null  &&  targetGridPart.unit!=moveObject))
            {
                var newTarget = FindClosestWalkableGridPart(targetGridPart, startGridPart, moveObject);
                targetGridPart = newTarget;
            }
            
            return CalculatePath(startGridPart, targetGridPart, moveObject, out var gridParts) ? gridParts : null;
        }

        private static bool CalculatePath(GridPart startGridPart, GridPart targetGridPart, Transform moveObject, out List<GridPart> gridParts)
        {
            List<GridPart> openSet = new List<GridPart>();
            HashSet<GridPart> closedSet = new HashSet<GridPart>();
            openSet.Add(startGridPart);

            while (openSet.Count > 0)
            {
                GridPart currentGridPart = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentGridPart.FCost || (openSet[i].FCost == currentGridPart.FCost &&
                                                                     openSet[i].hCost < currentGridPart.hCost))
                    {
                        currentGridPart = openSet[i];
                    }
                }

                openSet.Remove(currentGridPart);
                closedSet.Add(currentGridPart);

                if (currentGridPart == targetGridPart && !(currentGridPart.IsObstacle || (currentGridPart.unit!=null  &&  currentGridPart.unit!=moveObject)))
                {
                    {
                        gridParts = RetracePath(startGridPart, targetGridPart);
                        return true;
                    }
                }

                foreach (var neighbor in GetNeighbors(currentGridPart, moveObject))
                {
                    if (closedSet.Contains(neighbor) || (neighbor.IsObstacle || (neighbor.unit!=null  &&  neighbor.unit!=moveObject)))
                    {
                        continue;
                    }

                    int newCostToNeighbor = currentGridPart.gCost + GetDistance(currentGridPart, neighbor);
                    if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCostToNeighbor;
                        neighbor.hCost = GetDistance(neighbor, targetGridPart);
                        neighbor.cameFrom = currentGridPart;

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

        private static List<GridPart> GetNeighbors(GridPart gridPart, Transform moveObject)
        {
            List<GridPart> neighbors = new List<GridPart>();
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
                    GridPart neighbor = GridManager.Instance.GetGridPart(newX, newY);
                    if (!(neighbor.IsObstacle || (neighbor.unit!=null  &&  neighbor.unit!=moveObject)))
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }

            return neighbors;
        }


        private static List<GridPart> RetracePath(GridPart startGridPart, GridPart endGridPart)
        {
            List<GridPart> path = new List<GridPart>();
            GridPart currentGridPart = endGridPart;

            while (currentGridPart != startGridPart)
            {
                path.Add(currentGridPart);
                currentGridPart = currentGridPart.cameFrom;
            }

            path.Reverse();
            return path;
        }

        private static int GetDistance(GridPart gridPartA, GridPart gridPartB)
        {
            int distX = Math.Abs(gridPartA.Width - gridPartB.Width);
            int distY = Math.Abs(gridPartA.High - gridPartB.High);

            return distX + distY;
        }

        private static GridPart FindClosestWalkableGridPart(GridPart targetGridPart, GridPart startGridPart, Transform moveObject)
        {
            int gridSizeX = GridManager.Instance.SizeX;
            int gridSizeY = GridManager.Instance.SizeY;
            int maxDistance = Math.Max(gridSizeX, gridSizeY);
            int closestDistance = int.MaxValue;

            for (int distance = 1; distance < maxDistance; distance++)
            {
                var closestList = new List<GridPart>();
                for (int i = -distance; i <= distance; i++)
                {
                    for (int j = -distance; j <= distance; j++)
                    {
                        int newX = targetGridPart.Width + i;
                        int newY = targetGridPart.High + j;

                        if (newX >= 0 && newX < gridSizeX && newY >= 0 && newY < gridSizeY)
                        {
                            GridPart gridPart = GridManager.Instance.GetGridPart(newX, newY);
                            if (!(gridPart.IsObstacle || (gridPart.unit!=null  &&  gridPart.unit!=moveObject)))
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