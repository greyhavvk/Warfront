using Pathfinding;
using UnityEngine;

namespace Grid_System
{
    public interface IGridPart
    {
        Transform Transform { get; }
        void Initialize(int width, int high);
        bool Empty { get; set; }
        IPathfinding Pathfinding { get; }
        
        Transform Unit { get; set; }
        int High { get; }
        int Width { get; }
        bool IsObstacle { get; }
    }
}