using Placeable;
using UnityEngine;

namespace Grid_System
{
    public interface IGridPart
    {
        Transform Transform { get; }
        void Initialize(int width, int high);
        bool Empty { get; set; }
        Transform Unit { get; set; }
        IGetPiecePosition PiecePosition { get; set; }
        int High { get; }
        int Width { get; }
        public int GCost { get; set; }
        public int HCost{ get; set; }
        public int FCost { get; }
        IGridPart CameFrom { get; set; }
    }
}