using UnityEngine;

namespace HitPointSystem
{
    public interface ITakeDamage
    {
        void TakeDamage(float damage);
        GameObject GameObject { get; }
    }
}