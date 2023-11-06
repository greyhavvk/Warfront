using Managers;

namespace InputSystem
{
    public interface IClickEvent
    {
        public event ClickManager.EventHandler OnUnitGetOrder ;
    
        public event ClickManager.EventHandler OnTryPlacement ;
        public event ClickManager.EventHandler OnRotateBuilding ;
    
        public event ClickManager.EventHandler OnCancel;
    
        public event ClickManager.EventHandler OnOpenProductionMenu ;
    
        public event ClickManager.EventHandler OnUnitClick;
    
        public event ClickManager.EventHandler OnPlacebleClick;
    
        public event ClickManager.EventHandler OnStartDrag ;
        public event ClickManager.EventHandler OnDrag ;
        public event ClickManager.EventHandler OnEndDrag;
    }
}