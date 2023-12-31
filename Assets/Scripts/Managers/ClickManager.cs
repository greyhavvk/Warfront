using InputSystem;
using UnityEngine;

namespace Managers
{
    public class ClickManager : MonoBehaviour,IClickEvent,IClickType
    {
        public static IClickEvent ClickEvent { get; private set; }
        public static IClickType Type { get; private set; }
    
        public delegate void EventHandler();

        public event EventHandler OnUnitGetOrder ;
    
        public event EventHandler OnTryPlacement ;
        public event EventHandler OnRotateBuilding ;
    
        public event EventHandler OnCancel;
        
        public event EventHandler OnUnitClick;
    
        public event EventHandler OnPlaceableClick;
    
        public event EventHandler OnStartDrag ;
        public event EventHandler OnDrag ;
        public event EventHandler OnEndDrag;

        public ClickType ClickType { get; set; } = ClickType.Nothing;

        
        public void SetInstance()
        {
            ClickEvent = this;
            Type = this;
        }

        public void Initialize()
        {
            ListenEvents();
        }

        private void ListenEvents()
        {
            InputManager.InputEvent.OnMouseLeftClickDown += MouseLeftClickDown;
            InputManager.InputEvent.OnMouseLeftClick += MouseLeftClick;
            InputManager.InputEvent.OnMouseLeftClickUp += MouseLeftClickUp;
            InputManager.InputEvent.OnMouseRightClickDown += MouseRightClickDown;
            InputManager.InputEvent.OnRotateButtonDown += RotateButtonDown;
            InputManager.InputEvent.OnCancelButtonDown += CancelButtonDown;
        }

        private void OnDestroy()
        {
            MuteEvents();
        }

        private void MuteEvents()
        {
            InputManager.InputEvent.OnMouseLeftClickDown -= MouseLeftClickDown;
            InputManager.InputEvent.OnMouseLeftClick -= MouseLeftClick;
            InputManager.InputEvent.OnMouseLeftClickUp -= MouseLeftClickUp;
            InputManager.InputEvent.OnMouseRightClickDown -= MouseRightClickDown;
            InputManager.InputEvent.OnRotateButtonDown -= RotateButtonDown;
            InputManager.InputEvent.OnCancelButtonDown -= CancelButtonDown;
        }


        private void CancelButtonDown()
        {
            if (ClickType != ClickType.Nothing)
            {
                OnCancel?.Invoke();
                ClickType = ClickType.Nothing;
            }
            else
            {
                Application.Quit();
            }
        }

        private void RotateButtonDown()
        {
            if (ClickType==ClickType.PlaceBuilding)
            {
                OnRotateBuilding?.Invoke();
            }
        }

        private void MouseRightClickDown()
        {
            if (ClickType==ClickType.Nothing)
            {
                OnUnitGetOrder?.Invoke();
            }
        }

        private void MouseLeftClickUp()
        {
            OnEndDrag?.Invoke();
        }

        private void MouseLeftClick()
        {
            if (ClickType==ClickType.Nothing)
                OnDrag?.Invoke();
        }

        private void MouseLeftClickDown()
        {
            if (ClickType==ClickType.PlaceBuilding)
            {
                OnTryPlacement?.Invoke();
            }
            else if (ClickType==ClickType.Nothing)
            {
                OnStartDrag?.Invoke();
                OnUnitClick?.Invoke();
                OnPlaceableClick?.Invoke();
            }
        }
    }
}