using System.Collections.Generic;
using Managers;
using ObjectPool;
using UnityEngine;

namespace Placeable
{
    public class PlaceableEntity : PoolableObject,IPlaceableType,IPlacement,IGetPiecePosition
    {
        [SerializeField] private PlaceableType placeableType;
        [SerializeField] private Transform[] buildingPieces;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] private Color canPlaceColor;
        [SerializeField] private Color cantPlaceColor;
        [SerializeField] private List<Transform> pieces;
        public  Transform[] BuildingPieces => buildingPieces;
        public PlaceableType PlaceableType => placeableType;
        public Transform Transform => transform;
        public List<Transform> Pieces => pieces;
        public IGetPiecePosition GetPiecePosition => this;
        
        protected override void ReturnToPool()
        {
            foreach (var piece in BuildingPieces)
            {
                var grid=GridManager.GetPart.GetGridPart(piece.position);
                grid.Empty = true;
                grid.PiecePosition = null;
            }
            base.ReturnToPool();
        }

        public void SetCanPlaceColor(bool canPlace)
        {
            spriteRenderer.color = canPlace?canPlaceColor: cantPlaceColor;
        }

        public virtual void Placed()
        {
            spriteRenderer.color=Color.white;
        }

        public void CancelPlacement()
        {
            spriteRenderer.color = canPlaceColor;
            ReturnToPool();
        }

        public virtual void Rotate()
        {
        }
    }
}