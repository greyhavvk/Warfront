using Building;
using InputSystem;
using UI;

using UnityEngine;

namespace Unit_Selection
{
    public class UnitClick : MonoBehaviour
    {
        [SerializeField] private LayerMask clickable;

        private void Start()
        {
            ClickManager.Instance.OnUnitClick+=Click;
        }

        private void OnDestroy()
        {
            ClickManager.Instance.OnUnitClick-=Click;
        }

        private void Click()
        {
            if (ClickManager.ClickType != ClickType.Nothing)
                return;
            var hit = Physics2D.Raycast( InputManager.Mouse.GetMousePosToWorldPos(), Vector2.zero, Mathf.Infinity, clickable);
            if (hit)
            {
                if ( hit.collider.gameObject.CompareTag("unit"))
                {
                    if ( InputManager.Key.GetKey(KeyCode.LeftShift))
                    {
                        UnitSelection.Instance.ShiftClickSelect(hit.collider.gameObject);
                    }
                    else
                    {
                        UnitSelection.Instance.ClickSelect(hit.collider.gameObject);
                    } 
                }
            }
            else
            {
                if (! InputManager.Key.GetKey(KeyCode.LeftShift))
                {
                    UnitSelection.Instance.DeselectAll();
                }
            }
        }
    }
}