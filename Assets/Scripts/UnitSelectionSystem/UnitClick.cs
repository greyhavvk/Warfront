using InputSystem;
using Managers;
using UnityEngine;

namespace UnitSelectionSystem
{
    public class UnitClick : MonoBehaviour
    {
        [SerializeField] private LayerMask clickable;

        private void Start()
        {
            ClickManager.ClickEvent.OnUnitClick+=Click;
        }

        private void OnDestroy()
        {
            ClickManager.ClickEvent.OnUnitClick-=Click;
        }

        private void Click()
        {
            if (ClickManager.Type.ClickType != ClickType.Nothing)
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