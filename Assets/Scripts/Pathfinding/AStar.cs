using System;
using System.Collections.Generic;
using Grid_System;
using Managers;
using UnityEngine;

namespace Pathfinding
{
    public static class AStar
    {
        public static List<IGridPart> FindPath(IGridPart startGridPart, IGridPart targetGridPart, Transform moveObject, out float cost)
        {
            cost = 0;
            if (startGridPart==null || targetGridPart==null  || !moveObject)
            {
                return null;
            }

            return CalculatePath(startGridPart, targetGridPart, moveObject, out var gridParts, out cost) ? gridParts : null;
        }

        private static bool CalculatePath(IGridPart startGridPart, IGridPart targetGridPart, Transform moveObject,
            out List<IGridPart> gridParts, out float cost)
        {
            List<IGridPart> openSet = new List<IGridPart>();
            HashSet<IGridPart> closedSet = new HashSet<IGridPart>();
            openSet.Add(startGridPart);

            while (openSet.Count > 0)
            {
                IGridPart currentGridPart = openSet[0];
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

                if (currentGridPart == targetGridPart && !(!currentGridPart.Empty || (currentGridPart.Unit!=null  &&  currentGridPart.Unit!=moveObject)))
                {
                    {
                        gridParts = RetracePath(startGridPart, targetGridPart);
                        cost = targetGridPart.FCost;
                        return true;
                    }
                }

                foreach (var neighbor in GetNeighbors(currentGridPart, moveObject))
                {
                    if (closedSet.Contains(neighbor) || (!neighbor.Empty || (neighbor.Unit!=null  &&  neighbor.Unit!=moveObject)))
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

        private static List<IGridPart> GetNeighbors(IGridPart gridPart, Transform moveObject)
        {
            List<IGridPart> neighbors = new List<IGridPart>();
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
                        IGridPart neighbor = grid;
                        if (!(!neighbor.Empty || (neighbor.Unit!=null  &&  neighbor.Unit!=moveObject)))
                        {
                            neighbors.Add(neighbor);
                        }
                    }
                   
                }
            }

            return neighbors;
        }


        private static List<IGridPart> RetracePath(IGridPart startGridPart, IGridPart endGridPart)
        {
            List<IGridPart> path = new List<IGridPart>();
            IGridPart currentGridPart = endGridPart;

            while (currentGridPart != startGridPart)
            {
                path.Add(currentGridPart);
                currentGridPart = currentGridPart.CameFrom;
            }

            path.Reverse();
            return path;
        }

        private static int GetDistance(IGridPart gridPartA, IGridPart gridPartB)
        {
            int distX = Math.Abs(gridPartA.Width - gridPartB.Width);
            int distY = Math.Abs(gridPartA.High - gridPartB.High);

            return distX + distY;
        }

    }
}