using System;
using InputSystem;
using UI;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public static ClickManager Instance { get; private set; }
    
    public delegate void EventHandler();

    public event EventHandler OnUnitGetOrder ;
    
    public event EventHandler OnTryPlacement ;
    public event EventHandler OnRotateBuilding ;
    
    public event EventHandler OnCancel;
    
    public event EventHandler OnOpenProductionMenu ;
    
    public event EventHandler OnUnitClick;
    
    public event EventHandler OnPlacebleClick;
    
    public event EventHandler OnStartDrag ;
    public event EventHandler OnDrag ;
    public event EventHandler OnEndDrag;
    
    public static ClickType ClickType;

   private void Awake()
    {
        if (Instance!=null && Instance!=this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InputManager.InputEvent.OnMouseLeftClickDown+=MouseLeftClickDown;
        InputManager.InputEvent.OnMouseLeftClick+=MouseLeftClick;
        InputManager.InputEvent.OnMouseLeftClickUp+=MouseLeftClickUp;
        InputManager.InputEvent.OnMouseRightClickDown+=MouseRightClickDown;
        InputManager.InputEvent.OnRotateButtonDown+=RotateButtonDown;
        InputManager.InputEvent.OnCancelButtonDown+=CancelButtonDown;
        InputManager.InputEvent.OnProductionButtonDown+=ProductionButtonDown;
    }
    
    private void OnDestroy()
    {
        InputManager.InputEvent.OnMouseLeftClickDown-=MouseLeftClickDown;
        InputManager.InputEvent.OnMouseLeftClick-=MouseLeftClick;
        InputManager.InputEvent.OnMouseLeftClickUp-=MouseLeftClickUp;
        InputManager.InputEvent.OnMouseRightClickDown-=MouseRightClickDown;
        InputManager.InputEvent.OnRotateButtonDown-=RotateButtonDown;
        InputManager.InputEvent.OnCancelButtonDown-=CancelButtonDown;
        InputManager.InputEvent.OnProductionButtonDown-=ProductionButtonDown;
    }

    private void ProductionButtonDown()
    {
        if (ClickType == ClickType.PlaceBuilding)
            return;
        OnOpenProductionMenu?.Invoke();
        ClickType = ClickType.UIPanel;
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
            OnPlacebleClick?.Invoke();
        }
    }
}