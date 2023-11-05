using Building;
using Grid_System;
using ObjectPool;
using UnityEngine;

namespace Placeble.Entity
{
    public class PlacebleEntity : PoolableObject,IPlacebleType,IPlacement
    {
        [SerializeField] private PlacebleType placebleType;
        [SerializeField] private Transform[] buildingPieces;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] private Color canPlaceColor;
        [SerializeField] private Color cantPlaceColor;
        public  Transform[] BuildingPieces => buildingPieces;
        public PlacebleType PlacebleType => placebleType;
        public Transform Transform => transform;
        
        public virtual void SetLevel(int lvl)
        {
            
        }

        protected override void ReturnToPool()
        {
            foreach (var piece in BuildingPieces)
            {
                GridManager.Instance.GetGridPart(piece.position).Empty = true;
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