using InputSystem;
using UnityEngine;

namespace Unit_Selection
{
    public class UnitClick : MonoBehaviour
    {
        [SerializeField] private LayerMask clickable;

        private void Start()
        {
            InputManager.Instance.OnMouseLeftClickDown+=Click;
        }

        private void OnDestroy()
        {
            InputManager.Instance.OnMouseLeftClickDown-=Click;
        }

        private void Click()
        {
            if (InputManager.Instance.inputType != InputType.Nothing)
                return;
            var hit = Physics2D.Raycast( InputManager.Instance.GetMousePosToWorldPos(), Vector2.zero, Mathf.Infinity, clickable);
            if (hit)
            {
                if ( InputManager.Instance.GetKey(KeyCode.LeftShift))
                {
                    UnitSelection.Instance.ShiftClickSelect(hit.collider.gameObject);
                }
                else
                {
                    UnitSelection.Instance.ClickSelect(hit.collider.gameObject);
                }
            }
            else
            {
                if (! InputManager.Instance.GetKey(KeyCode.LeftShift))
                {
                    UnitSelection.Instance.DeselectAll();
                }
            }
        }
    }
}