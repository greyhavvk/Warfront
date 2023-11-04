using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Grid_System
{
    public class Grid<TGridObject> where TGridObject : Object
    {
        private readonly int _width;
        private readonly int _height;
        private const float GridSize = 1;

        public Grid(int width, int height, TGridObject refGridPart, out Dictionary<Vector3, TGridObject> gridLocalPositionDictionary, out Transform parent)
        {
            gridLocalPositionDictionary = new Dictionary<Vector3,TGridObject>();
            _height = height;
            _width = width;

            parent = Object.Instantiate(new GameObject("Grid Part Parent"), new Vector2((-(width / 2f)+.5f)*GridSize, (-(height / 2f)+.5f)*GridSize),
                Quaternion.identity).transform;
        
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var createdGridObject = Object.Instantiate(refGridPart, parent);
                    gridLocalPositionDictionary.Add(new Vector2(i*GridSize, j*GridSize),createdGridObject);
                }
            }
        }

        public void GetXY(Vector3 localPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(localPosition.x / GridSize+GridSize*.5f);
            y = Mathf.FloorToInt(localPosition.y / GridSize+GridSize*.5f);
        }

        public bool IsWithinBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _width && y < _height;
        }
    }
}
