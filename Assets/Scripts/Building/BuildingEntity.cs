using HitPointSystem;
using ObjectPool;
using UnityEngine;
using UnityEngine.UIElements;

namespace Building
{
    public class BuildingEntity : PoolableObject
    {
        [SerializeField] private HealthPointObserver healthPointObserver;
        [SerializeField] private Transform[] buildingPieces;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color canPlaceColor;
        [SerializeField] private Color cantPlaceColor;
        public  Transform[] BuildingPieces => buildingPieces;

        public override void Initialize(PoolableObjectInitializeData poolableObjectInitializeData)
        {
            base.Initialize(poolableObjectInitializeData);
            healthPointObserver.Initialize(ReturnToPool);
        }

        public void SetCanPlaceColor(bool canPlace)
        {
            spriteRenderer.color = canPlace?canPlaceColor: cantPlaceColor;
        }

        public void Placed()
        {
            spriteRenderer.color=Color.white;
            healthPointObserver.ResetHp();
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