using Grid_System;
using UnityEngine;

namespace Pathfinding
{
    public interface IPathfinding
    {
        public int GCost { get; set; }
        public int HCost{ get; set; }
        public int FCost { get; }
        
        public int Width { get; }
        public int High { get; }
        bool IsObstacle { get; }
        Transform Unit { get;}
        IPathfinding CameFrom { get; set; }
        IGridPart gridPart { get; }
    }
}