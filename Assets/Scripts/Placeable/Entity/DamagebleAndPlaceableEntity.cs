using System.Collections.Generic;
using ObjectPool;
using Placeable.PlaceableExtra;
using UnityEngine;

namespace Placeable.Entity
{
    public class DamagebleAndPlaceableEntity : PlaceableEntity
    {
        [SerializeField] protected HealthPointObserver healthPointObserver;
      
        public override void Initialize(PoolableObjectInitializeData poolableObjectInitializeData)
        {
            base.Initialize(poolableObjectInitializeData);
            healthPointObserver.Initialize(ReturnToPool);
        }

        public override void Placed()
        {
            base.Placed();
            healthPointObserver.ResetHp();
        }
    }
}