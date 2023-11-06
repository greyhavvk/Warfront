using System.Linq;
using InputSystem;
using Managers;
using UnityEngine;

namespace UnitSelectionSystem
{
    public class UnitDrag : MonoBehaviour
    {
        private Camera _camera;
        [SerializeField] private RectTransform boxVisual;
        private Rect _selectionBox;
        private Vector2 _startPosition=Vector2.zero;
        private Vector2 _endPosition=Vector2.zero;

        private bool _dragging;
        
        private void Start()
        {
            _camera=Camera.main;
            DrawVisual();
            ClickManager.ClickEvent.OnStartDrag+=StartDrag;
            ClickManager.ClickEvent.OnDrag+=Drag;
            ClickManager.ClickEvent.OnEndDrag+=EndDrag;
        }

        private void OnDestroy()
        {
            ClickManager.ClickEvent.OnStartDrag-=StartDrag;
            ClickManager.ClickEvent.OnDrag-=Drag;
            ClickManager.ClickEvent.OnEndDrag-=EndDrag;
        }

        private void EndDrag()
        {
            SelectUnits();
            _startPosition = Vector2.zero;
            _endPosition = Vector2.zero;
            DrawVisual();
            _dragging = false;
        }

        private void Drag()
        {
            if (_dragging == false)
                return;
            if (ClickManager.Type.ClickType != ClickType.Nothing)
            {
                _startPosition = Vector2.zero;
                _endPosition = Vector2.zero;
                DrawVisual();
                return;
            }
            _endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        private void StartDrag()
        {
            if (ClickManager.Type.ClickType != ClickType.Nothing)
                return;
            _dragging = true;
            _startPosition = Input.mousePosition;
            _selectionBox = new Rect();
        }
        

        private void DrawVisual()
        {
            var boxStart = _startPosition;
            var boxEnd = _endPosition;

            var boxCenter = (boxStart + boxEnd) / 2;
            boxVisual.position = boxCenter;

            var boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

            boxVisual.sizeDelta = boxSize;
        }

        private void DrawSelection()
        {
            if (InputManager.Mouse.MousePos.x < _startPosition.x)
            {
                _selectionBox.xMin = InputManager.Mouse.MousePos.x;
                _selectionBox.xMax = _startPosition.x;
            }
            else
            {
                _selectionBox.xMin = _startPosition.x;
                _selectionBox.xMax = InputManager.Mouse.MousePos.x;
            }
            
            if (InputManager.Mouse.MousePos.y < _startPosition.y)
            {
                _selectionBox.yMin = InputManager.Mouse.MousePos.y;
                _selectionBox.yMax = _startPosition.y;
            }
            else
            {
                _selectionBox.yMin = _startPosition.y;
                _selectionBox.yMax = InputManager.Mouse.MousePos.y;
            }
        }

        private void SelectUnits()
        {
            foreach (var unit in UnitSelection.Instance.UnitTransformList.Where(unit =>
                         _selectionBox.Contains(_camera.WorldToScreenPoint(unit.position))))
            {
                UnitSelection.Instance.DragSelect(unit.gameObject);
            }
        }
    }
}