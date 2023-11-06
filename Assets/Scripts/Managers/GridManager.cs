using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Grid_System
{
    public class GridManager: MonoBehaviour
    {
        [SerializeField] private GridPart refGridPart;
        [SerializeField] private int width = 22;
        [SerializeField] private int height = 14;
        private Grid<GridPart> _grid;

        public static GridManager Instance { get; private set; }

        public int SizeX => width;

        public int SizeY => height;

        private void Awake()
        {
            if (Instance!=null && Instance!=this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            
            CreateGrid();
        }

        private void CreateGrid()
        {
            _grid = new Grid<GridPart>(width,height, refGridPart);
        }

        public IGridPart GetGridPart(Vector3 worldPosition)
        {
            return _grid.GetGridPart(worldPosition);
        }
       
        public IGridPart GetGridPart(int x, int y)
        {
            return _grid.GetGridPart(x,y);
        }
        
    }
}