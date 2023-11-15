using System.Collections.Generic;
using System.Linq;
using Grid_System;
using UnityEngine;

namespace Managers
{
    public class GridManager: MonoBehaviour,IGetGridPart,IGetGridSize
    {
        [SerializeField] private GridPart refGridPart;
        [SerializeField] private int width = 22;
        [SerializeField] private int height = 14;
        [SerializeField] private RectTransform leftPanel;
        [SerializeField] private RectTransform rightPanel;
        private Grid<GridPart> _grid;

        public static IGetGridPart GetPart { get; private set; }
        public static IGetGridSize GetGridSize { get; private set; }

        public int SizeX => width;

        public int SizeY => height;

        private const float PanelWidthNormalizationFactor = 1920f;
        
        public void SetInstance()
        {
            GetGridSize = this;
            GetPart = this;
        }
       
        public void Initialize()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            var minX = leftPanel.rect.xMax/PanelWidthNormalizationFactor;
            var maxX = (PanelWidthNormalizationFactor + rightPanel.rect.xMin) / PanelWidthNormalizationFactor;
            _grid = new Grid<GridPart>(width,height, refGridPart, minX, maxX);
        }

        public IGridPart GetGridPart(Vector3 worldPosition)
        {
            return _grid.GetGridPart(worldPosition);
        }
       
        public IGridPart GetGridPart(int x, int y)
        {
            return _grid.GetGridPart(x,y);
        }

        private bool CheckHaveAvailableGrid()
        {
            return _grid.CheckHaveAvailableGrid();
        }
        
        public List<IGridPart> FindEmptyPoints(List<Vector2> possibleSpawnPoints)
        {
            var spawnPoints = ClearObstacleFromPossibleSpawnPoints(possibleSpawnPoints);

            if (spawnPoints.Count <= 0)
            {
                spawnPoints = SearchOuterPoints(possibleSpawnPoints);
            }

            UpdatePossibleSpawnPoints(possibleSpawnPoints, spawnPoints);

            return spawnPoints;
        }
        
        private void UpdatePossibleSpawnPoints(List<Vector2> possibleSpawnPoints, List<IGridPart> spawnPoints)
        {
            if (spawnPoints != null)
            {
                foreach (var spawnPoint in spawnPoints)
                {
                    possibleSpawnPoints.Add(spawnPoint.Transform.position);
                }
            }
        }

        private List<IGridPart> ClearObstacleFromPossibleSpawnPoints(List<Vector2> possibleSpawnPoints)
        {
            return possibleSpawnPoints
                .Select(possibleSpawnPoint => GetGridPart(possibleSpawnPoint))
                .Where(grid => grid is { Empty: true })
                .ToList();
        }

        private List<IGridPart> SearchOuterPoints(List<Vector2> possibleSpawnPoints)
        {
            if (!CheckHaveAvailableGrid())
            {
                return null;
            }

            var newPossibleSpawnPoints = new List<Vector2>();
            var searchedPoints = new List<Vector2>();
            var gridSizeX = SizeX;
            var gridSizeY = SizeY;

            foreach (var targetGridPart in possibleSpawnPoints.Select(possibleSpawnPoint => GetGridPart(possibleSpawnPoint)).Where(targetGridPart => targetGridPart != null))
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int newX = targetGridPart.Width + i;
                        int newY = targetGridPart.High + j;

                        if (IsWithinGridBounds(newX, newY, gridSizeX, gridSizeY))
                        {
                            var gridPart = GetGridPart(newX, newY);
                            if (gridPart != null && !possibleSpawnPoints.Contains(gridPart.Transform.position) && gridPart.Empty)
                            {
                                newPossibleSpawnPoints.Add(gridPart.Transform.position);
                            }

                            if (gridPart != null) searchedPoints.Add(gridPart.Transform.position);
                        }
                    }
                }
            }

            return newPossibleSpawnPoints.Count > 0 ? FindEmptyPoints(newPossibleSpawnPoints) : SearchOuterPoints(searchedPoints);
        }
        
        private bool IsWithinGridBounds(int x, int y, int gridSizeX, int gridSizeY)
        {
            return x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY;
        }
    }
}