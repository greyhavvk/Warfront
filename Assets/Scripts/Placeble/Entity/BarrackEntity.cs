using Building;
using UnityEngine;

namespace Placeble.Entity
{
    public class BarrackEntity : DamagebleAndPlaceableEntity
    {
        [SerializeField] private Transform spawnPoint;

        public Transform SpawnPoint => spawnPoint;
        public override void Rotate()
        {
            transform.localEulerAngles = transform.localEulerAngles.z == 0 ? Vector3.forward * 90 : Vector3.zero;
        }
    }
}