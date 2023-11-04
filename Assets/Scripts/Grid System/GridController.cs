using System.Collections.Generic;
using UnityEngine;

namespace Grid_System
{
    public class GridController: MonoBehaviour
    {
        [SerializeField] private GridPart refGridPart;
        private Grid<GridPart> _grid;
        private Transform _parent;
        private Dictionary<Vector3,GridPart> _gridLocalPositionDictionary;

        public static GridController Instance { get; private set; }

        public int SizeX { get; private set; }

        public int SizeY {  get; private set; }

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
        }

        private void Start()
        {
            CreateGrid(20, 10);
        }

        private void CreateGrid(int width, int height)
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
                gridPart.Value.Initialize((int)gridPart.Key.x, (int)gridPart.Key.y,GridEmptiesChange);
            }
        }

        private void GridEmptiesChange()
        {
            //TODO unit'ler bunu dinleyecek.
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
    }
}