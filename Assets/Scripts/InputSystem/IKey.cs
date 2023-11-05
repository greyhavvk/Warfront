using UnityEngine;

namespace InputSystem
{
    public interface IKey
    {
        bool GetKey(KeyCode keyCode);
    }
}