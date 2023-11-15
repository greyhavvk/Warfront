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

        private void Awake()
        {
            GetGridSize = this;
            GetPart = this;
               
        }

        private void Start()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            var minX = leftPanel.rect.xMax/1920f;
            var maxX = (1920f + rightPanel.rect.xMin) / 1920f;
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

        public bool CheckHaveAvailableGrid()
        {
            return _grid.CheckHaveAvailableGrid();
        }
        
         public List<IGridPart> FindEmptyPoints(List<Vector2> possibleSpawnPoints)
        {
            var spawnPoints = ClearObstacleFromPossibleSpawnPoints(possibleSpawnPoints);
            if (spawnPoints.Count <= 0)
            {
                spawnPoints = SearchOuterPoints(possibleSpawnPoints);
                possibleSpawnPoints = new List<Vector2>();
                if (spawnPoints!=null)
                {
                    foreach (var spawnPoint in spawnPoints)
                    {
                        possibleSpawnPoints.Add(spawnPoint.Transform.position);
                    }
                }
            }
            return spawnPoints;
        }

         private List<IGridPart> ClearObstacleFromPossibleSpawnPoints(List<Vector2> possibleSpawnPoints)
        {
            List<IGridPart> spawnPoints = new List<IGridPart>();
            foreach (var possibleSpawnPoint in possibleSpawnPoints)
            {
                var grid = GetGridPart(possibleSpawnPoint);
                if (grid != null)
                {
                    if (!grid.IsObstacle)
                    {
                        spawnPoints.Add(grid);
                    }
                }
            }

            return spawnPoints;
        }

        public List<IGridPart> SearchOuterPoints(List<Vector2> possibleSpawnPoints)
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

                        if (newX < 0 || newX >= gridSizeX || newY < 0 || newY >= gridSizeY) continue;
                        var gridPart = GetGridPart(newX, newY);
                        if (gridPart == null) continue;
                        if (possibleSpawnPoints.Contains(gridPart.Transform.position)) continue;
                        if (!gridPart.IsObstacle)
                        {
                            newPossibleSpawnPoints.Add(gridPart.Transform.position);
                        }
                        searchedPoints.Add(gridPart.Transform.position);
                    }
                }
            }

            return newPossibleSpawnPoints.Count > 0 ? FindEmptyPoints(newPossibleSpawnPoints) : SearchOuterPoints(searchedPoints);
        }
    }
}