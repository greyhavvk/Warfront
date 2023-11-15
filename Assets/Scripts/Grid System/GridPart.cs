using Pathfinding;
using Placeable;
using UnityEngine;

namespace Grid_System
{
    public class GridPart : MonoBehaviour, IGridPart, IPathfinding
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        public int GCost { get=>_gCost; set=>_gCost=value; }
        public int HCost  { get=>_hCost; set=>_hCost=value; }
        public int FCost=>_gCost+ _hCost;
        public bool IsObstacle => !_empty;
        public Transform Unit  { get=>_unit; set=>_unit=value; }
        public IGetPiecePosition PiecePosition { get; set; }
        public IPathfinding CameFrom { get; set; }

        public IGridPart gridPart => this;
        public IPathfinding Pathfinding => this;
        public int Width=>_width;
        public int High=>_high;
        
        private bool _empty = true;
        private int _width;
        private int _high;

        private int _gCost;
        private int _hCost;
        private Transform _unit;

        public Transform Transform => transform;

        public void Initialize(int width, int high)
        {
            _width = width;
            _high = high;
        }
        
        public bool Empty
        {
            get => _empty;
            set
            {
                _empty = value;
                if (_empty)
                {
                    _unit = null;
                }
                UpdateVisual();
            }
        }

        private void UpdateVisual()
        {
            var color = spriteRenderer.color;
            color.a = _empty ? .3f : 1;
            spriteRenderer.color = color;
        }
    }
}