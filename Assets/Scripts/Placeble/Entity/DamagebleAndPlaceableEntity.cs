using Grid_System;
using HitPointSystem;
using ObjectPool;
using Placeble;
using Placeble.Entity;
using UnityEngine;

namespace Building
{
    public class DamagebleAndPlaceableEntity : PlacebleEntity
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