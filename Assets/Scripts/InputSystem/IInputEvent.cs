using Managers;

namespace InputSystem
{
    public interface IInputEvent
    {
        public event InputManager.EventHandler OnMouseLeftClickDown ;
        public event InputManager.EventHandler OnMouseLeftClick;
        public event InputManager.EventHandler OnMouseLeftClickUp;
        public event InputManager.EventHandler OnMouseRightClickDown ;
        public event InputManager.EventHandler OnRotateButtonDown;
        public event  InputManager.EventHandler OnCancelButtonDown;
    }
}