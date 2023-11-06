using System;
using Grid_System;
using UnityEngine;

namespace Managers
{
    public class GridManager: MonoBehaviour,IGetGridPart,IGetGridSize
    {
        [SerializeField] private GridPart refGridPart;
        [SerializeField] private int width = 22;
        [SerializeField] private int height = 14;
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