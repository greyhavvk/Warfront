using System.Collections;
using UnityEngine;

namespace Building
{
    public interface IPlacement
    {
        void SetCanPlaceColor(bool canPlace);
        void Placed();
        void CancelPlacement();
        void Rotate();
        Transform[] BuildingPieces { get;}
        Transform Transform { get; }
        void SetLevel(int lvl);
    }
}