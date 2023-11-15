using System;
using System.Collections.Generic;
using Grid_System;
using Managers;
using UnityEngine;

namespace Pathfinding
{
    public static class AStar
    {
        public static List<IGridPart> FindPath(IPathfinding startGridPart, IPathfinding targetGridPart, Transform moveObject, out float cost)
        {
            cost = 0;
            if (startGridPart==null || targetGridPart==null)
            {
                return null;
            }

            return CalculatePath(startGridPart, targetGridPart, moveObject, out var gridParts, out cost) ? gridParts : null;
        }

        private static bool CalculatePath(IPathfinding startGridPart, IPathfinding targetGridPart, Transform moveObject,
            out List<IGridPart> gridParts, out float cost)
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
                        cost = targetGridPart.FCost;
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
            cost =0;
            return false;
        }

        private static List<IPathfinding> GetNeighbors(IPathfinding gridPart, Transform moveObject)
        {
            List<IPathfinding> neighbors = new List<IPathfinding>();
            int gridSizeX = GridManager.GetGridSize.SizeX;
            int gridSizeY = GridManager.GetGridSize.SizeY;

            int[] offsetX = { 0, 0, 1, -1, 1, -1, 1, -1 };
            int[] offsetY = { 1, -1, 0, 0, 1, -1, -1, 1 };

            for (int i = 0; i < 8; i++)
            {
                int newX = gridPart.Width + offsetX[i];
                int newY = gridPart.High + offsetY[i];

                if (newX >= 0 && newX < gridSizeX && newY >= 0 && newY < gridSizeY)
                {
                    var grid = GridManager.GetPart.GetGridPart(newX, newY);
                    if (grid != null)
                    {
                        IPathfinding neighbor = grid.Pathfinding;
                        if (!(neighbor.IsObstacle || (neighbor.Unit!=null  &&  neighbor.Unit!=moveObject)))
                        {
                            neighbors.Add(neighbor);
                        }
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

    }
}