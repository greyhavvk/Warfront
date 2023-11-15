using UnityEngine;

namespace Placeable.PlaceableExtra
{
    public interface ITakeDamage
    {
        void TakeDamage(float damage);
        GameObject GameObject { get; }
    }
}