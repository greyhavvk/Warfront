using UnityEngine;

namespace InputSystem
{
    public interface IMouse
    {
        Vector2 GetMousePosToWorldPos();
        Vector2 MousePos { get; }
    }
}