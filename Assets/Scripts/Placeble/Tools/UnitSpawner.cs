using System;
using Grid_System;
using Managers;
using UnityEngine;

namespace Placeble.Tools
{
    public class UnitSpawner
    {
        public static void SpawnUnit(Vector3 spawnPoint , int lvl)
        {
            var grid = GridManager.GetPart.GetGridPart(spawnPoint);
            if (grid.IsObstacle)
            {
                grid = FindClosestWalkableGridPart(grid);
            }

            if (grid==null)
            {
                return;
            }

            var unit = PlacebleFactory.Instance.InstantPopUnit();
            unit.Transform.position = grid.Transform.position;
            unit.SetLevel(lvl);
            unit.Placed();
            foreach (var pieces in unit.BuildingPieces)
            {
                var position = pieces.position;
                var gridPart = GridManager.GetPart.GetGridPart(position);
                gridPart.Empty = false;
                gridPart.Unit = unit.Transform;
            }
        }

        private static IGridPart FindClosestWalkableGridPart(IGridPart targetGridPart)
        {
            int gridSizeX = GridManager.GetGridSize.SizeX;
            int gridSizeY = GridManager.GetGridSize.SizeY;
            int maxDistance = Math.Max(gridSizeX, gridSizeY);
            int closestDistance = int.MaxValue;

            IGridPart closest = null;
            for (int distance = 1; distance < maxDistance; distance++)
            {
                for (int i = -distance; i <= distance; i++)
                {
                    for (int j = -distance; j <= distance; j++)
                    {
                        int newX = targetGridPart.Width + i;
                        int newY = targetGridPart.High + j;

                        if (newX >= 0 && newX < gridSizeX && newY >= 0 && newY < gridSizeY)
                        {
                            IGridPart gridPart = GridManager.GetPart.GetGridPart(newX, newY);
                            if (!(gridPart.IsObstacle) && gridPart.Unit == null)
                            {
                                int currentDistance = Math.Abs(newX - targetGridPart.Width) +
                                                      Math.Abs(newY - targetGridPart.High);
                                if (currentDistance < closestDistance)
                                {
                                    closestDistance = currentDistance;
                                    closest= gridPart;
                                }
                            }
                        }
                    }
                }
            }

            return closest;
        }
    }
}