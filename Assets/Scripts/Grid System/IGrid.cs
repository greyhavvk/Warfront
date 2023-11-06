using Grid_System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public interface IGrid<TGridObject> where TGridObject : IGridPart
{
    void GetXY(Vector3 localPosition, out int x, out int y);
    bool IsWithinBounds(int x, int y);
}