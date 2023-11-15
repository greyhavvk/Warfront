using UnityEngine;

namespace Placeable
{
    public interface IPlacement
    {
        void SetCanPlaceColor(bool canPlace);
        void Placed();
        void CancelPlacement();
        void Rotate();
        Transform[] BuildingPieces { get;}
        Transform Transform { get; }
        
        IGetPiecePosition GetPiecePosition { get; }
    }
}