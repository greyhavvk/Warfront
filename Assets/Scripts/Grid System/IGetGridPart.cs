using System.Collections.Generic;
using UnityEngine;

namespace Grid_System
{
    public interface IGetGridPart
    {
        IGridPart GetGridPart(Vector3 worldPosition);
        IGridPart GetGridPart(int x, int y);
        List<IGridPart> FindEmptyPoints(List<Vector2> possibleSpawnPoints);
    }
}