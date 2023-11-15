using ObjectPool;
using UnityEngine;
using UnityEngine.Serialization;

namespace Placeable.Entity
{
    public class BarrackEntity : DamagebleAndPlaceableEntity
    {
        [SerializeField] private Transform[] spawnPointTransforms;
        
        public Vector2[] SpawnPoints => SetSpawnPoints();

        public override void Initialize(PoolableObjectInitializeData poolableObjectInitializeData)
        {
            base.Initialize(poolableObjectInitializeData);
            SetSpawnPoints();
        }

        private Vector2[] SetSpawnPoints()
        {
            var spawnPoints = new Vector2[spawnPointTransforms.Length];
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                spawnPoints[i] = spawnPointTransforms[i].position;
            }

            return spawnPoints;
        }

        public override void Rotate()
        {
            transform.localEulerAngles = transform.localEulerAngles.z == 0 ? Vector3.forward * 90 : Vector3.zero;
        }
    }
}