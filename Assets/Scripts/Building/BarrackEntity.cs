using UnityEngine;

namespace Building
{
    public class BarrackEntity : BuildingEntity
    {
        public override void Rotate()
        {
            transform.localEulerAngles = transform.localEulerAngles.z == 0 ? Vector3.forward * 90 : Vector3.zero;
        }
    }
}