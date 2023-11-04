using UnityEngine;
using UnityEngine.Serialization;

namespace Grid_System
{
    public class GridPart : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private bool _empty = true;
        private int _width;
        private int _high;

        public int gCost;
        public int hCost;
        public int FCost=>gCost+ hCost;
        
        public int Width=>_width;
        public int High=>_high;

        public GridPart _cameFrom;
        
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
                UpdateVisual();
            }
        }

        public bool IsObstacle => !_empty;

        private void UpdateVisual()
        {
            var color = spriteRenderer.color;
            color.a = _empty ? .3f : 1;
            spriteRenderer.color = color;
        }
    }
}