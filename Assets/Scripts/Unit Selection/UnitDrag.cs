using System.Linq;
using InputSystem;
using UnityEngine;

namespace Unit_Selection
{
    public class UnitDrag : MonoBehaviour
    {
        private Camera _camera;
        [SerializeField] private RectTransform boxVisual;

        private Rect _selectionBox;
        private Vector2 _startPosition=Vector2.zero;
        private Vector2 _endPosition=Vector2.zero;

        private void Start()
        {
            _camera=Camera.main;
            DrawVisual();
            InputManager.Instance.OnMouseLeftClickDown+=StartDrag;
            InputManager.Instance.OnMouseLeftClick+=Drag;
            InputManager.Instance.OnMouseLeftClickUp+=EndDrag;
        }

        private void OnDestroy()
        {
            InputManager.Instance.OnMouseLeftClickDown-=StartDrag;
            InputManager.Instance.OnMouseLeftClick-=Drag;
            InputManager.Instance.OnMouseLeftClickUp-=EndDrag;
        }

        private void EndDrag()
        {
            if (InputManager.Instance.inputType == InputType.Nothing)
                SelectUnits();
            _startPosition = Vector2.zero;
            _endPosition = Vector2.zero;
            DrawVisual();
        }

        private void Drag()
        {
            if (InputManager.Instance.inputType != InputType.Nothing)
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
            if (InputManager.Instance.inputType != InputType.Nothing)
                return;
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
            if (InputManager.Instance.MousePos.x < _startPosition.x)
            {
                _selectionBox.xMin = InputManager.Instance.MousePos.x;
                _selectionBox.xMax = _startPosition.x;
            }
            else
            {
                _selectionBox.xMin = _startPosition.x;
                _selectionBox.xMax = InputManager.Instance.MousePos.x;
            }
            
            if (InputManager.Instance.MousePos.y < _startPosition.y)
            {
                _selectionBox.yMin = InputManager.Instance.MousePos.y;
                _selectionBox.yMax = _startPosition.y;
            }
            else
            {
                _selectionBox.yMin = _startPosition.y;
                _selectionBox.yMax = InputManager.Instance.MousePos.y;
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