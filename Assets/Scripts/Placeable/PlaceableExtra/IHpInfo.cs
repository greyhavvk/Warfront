using System;

namespace Placeable.PlaceableExtra
{
    public interface IHpInfo
    {
        Action OnDie { get; set; }
    }
}