using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Grid_System
{
    public class Grid<TGridObject>  where TGridObject : Object, IGridPart
    {
        private readonly int _width;
        private readonly int _height;
        private const float GridSize = 1;
        private readonly Dictionary<Vector3,TGridObject> _gridLocalPositionDictionary;
        private readonly Transform _parent;

        public Grid(int width, int height, TGridObject refGridPart, float minX, float maxX)
        {
            int index = 0;
            _gridLocalPositionDictionary = new Dictionary<Vector3,TGridObject>();
            _height = height;
            _width = width;
            _parent = new GameObject("Grid Part Parent").transform; 
            _parent.position=new Vector2((-(width / 2f)+.5f)*GridSize, (-(height / 2f)+.5f)*GridSize);
            var mainCamera = Camera.main;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (mainCamera != null)
                    {
                        var localPosition = new Vector3(i * GridSize, j * GridSize);
                        var objectScreenPosition = mainCamera.WorldToViewportPoint(localPosition+_parent.position);
                        if ( ((objectScreenPosition.x>minX && objectScreenPosition.x<(maxX) && objectScreenPosition.y is > 0 and < 1)))
                        {
                            var createdGridObject = Object.Instantiate(refGridPart, _parent);
                            createdGridObject.name += index;
                            index++;
                            _gridLocalPositionDictionary.Add(localPosition,createdGridObject);
                        }
                    }
                }
            }

            RepositionGridParts(minX, maxX);
        }

        public void GetXY(Vector3 localPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(localPosition.x / GridSize+GridSize*.5f);
            y = Mathf.FloorToInt(localPosition.y / GridSize+GridSize*.5f);
        }

        private void RepositionGridParts(float minX, float maxX)
        {
            var mainCamera = Camera.main;
            if (mainCamera == null) return;
            foreach (var gridPart in _gridLocalPositionDictionary)
            {
                gridPart.Value.Transform.localPosition = gridPart.Key;
                gridPart.Value.Initialize((int)gridPart.Key.x, (int)gridPart.Key.y);
                var objectScreenPosition = mainCamera.WorldToViewportPoint(gridPart.Value.Transform.position);
                gridPart.Value.Empty= ((objectScreenPosition.x>minX && objectScreenPosition.x<maxX) && objectScreenPosition.y is > 0 and < 1);
            }
        }

        public IGridPart GetGridPart(Vector3 worldPosition)
        {
            var localPosition = worldPosition - _parent.position;
            GetXY(localPosition,out var x, out var y);
            return GetGridPart(x, y);
        }

        public IGridPart GetGridPart(int x, int y)
        {
            if (_gridLocalPositionDictionary.ContainsKey(new Vector2(x, y)))
            {
                return _gridLocalPositionDictionary[new Vector2(x, y)];
            }
            return null;
        }

        public bool CheckHaveAvailableGrid()
        {
            foreach (var gridPart in _gridLocalPositionDictionary.Values)
            {
                if (gridPart.Empty)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
