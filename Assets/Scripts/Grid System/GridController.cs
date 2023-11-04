using System.Collections.Generic;
using UnityEngine;

namespace Grid_System
{
    public class GridController: MonoBehaviour
    {
        [SerializeField] private GridPart refGridPart;
        private Grid<GridPart> _grid;
        private Camera _camera;
        private Transform _parent;
        private Dictionary<Vector3,GridPart> _gridLocalPositionDictionary;

        private static GridController _instance;
        public static GridController Instance => _instance;

        public int SizeX { get; private set; }

        public int SizeY {  get; private set; }

        private void Awake()
        {
            if (_instance!=null && _instance!=this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            _camera=Camera.main;
            CreateGrid(20, 10);
        }

        public void CreateGrid(int width, int height)
        {
            SizeX = width;
            SizeY = height;
            _grid = new Grid<GridPart>(width,height, refGridPart, out _gridLocalPositionDictionary, out _parent);
            RepositionGridParts();
        }

        private void RepositionGridParts()
        {
            foreach (var gridPart in _gridLocalPositionDictionary)
            {
                gridPart.Value.transform.localPosition = gridPart.Key;
                gridPart.Value.Initialize((int)gridPart.Key.x, (int)gridPart.Key.y);
            }
        }

        public GridPart GetGridPart(Vector3 worldPosition)
        {
            var localPosition = worldPosition - _parent.position;
            _grid.GetXY(localPosition,out var x, out var y);
            return GetGridPart(x, y);
        }

        public GridPart GetGridPart(int x, int y)
        {
            if (_grid.IsWithinBounds(x, y))
            {
                return _gridLocalPositionDictionary[new Vector2(x, y)];
            }
            return null;
        }
        
        private void SetEmpty(Vector3 worldPosition, bool value)
        {
            var gridPart=GetGridPart(worldPosition);
            if (gridPart)
            {
                gridPart.Empty = value;
            }
        }

        private bool GetEmpty(Vector3 worldPosition)
        {
            var value=false;
            var gridPart=GetGridPart(worldPosition);
            if (gridPart)
            {
                value=gridPart.Empty;
            }
            return value;
        }
    }
}