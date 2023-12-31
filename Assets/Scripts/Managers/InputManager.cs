using InputSystem;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour, IMouse,IInputEvent,IKey
    {
        private Camera _camera;
        
        public static IInputEvent InputEvent { get; private set; }
        public static IMouse Mouse { get; private set; }
        public static IKey Key { get; private set; }

        public delegate void EventHandler();

        public event EventHandler OnMouseLeftClickDown ;
        public event EventHandler OnMouseLeftClick;
        public event EventHandler OnMouseLeftClickUp;
        public event EventHandler OnMouseRightClickDown ;
        public event EventHandler OnRotateButtonDown;
        public event  EventHandler OnCancelButtonDown;

        public bool GetKey(KeyCode keyCode) => Input.GetKey(keyCode);

        public void SetInstance()
        {
            InputEvent = this;
            Mouse = this;
            Key = this;
        }

        public void Initialize()
        {
            _camera=Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseLeftClickDown?.Invoke();
            }
            
            if (Input.GetMouseButton(0))
            {
                OnMouseLeftClick?.Invoke();
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                OnMouseLeftClickUp?.Invoke();
            }

            if (Input.GetMouseButtonDown(1))
            {
                OnMouseRightClickDown?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                OnRotateButtonDown?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnCancelButtonDown?.Invoke();
            }
        }
        
        public Vector2 GetMousePosToWorldPos(){
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            return new Vector2(mousePos.x, mousePos.y);
        }

        public Vector2 MousePos => Input.mousePosition;
    }
}