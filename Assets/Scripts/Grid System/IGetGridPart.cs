using UnityEngine;

namespace Grid_System
{
    public interface IGetGridPart
    {
        IGridPart GetGridPart(Vector3 worldPosition);
        IGridPart GetGridPart(int x, int y);
    }
}