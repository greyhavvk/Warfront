using System.Linq;
using DG.Tweening;
using InputSystem;
using Managers;
using UnityEngine;

namespace Placeable.Tools
{
    public class PlaceablePlacement : MonoBehaviour
    {
        private IPlacement _unPlacedPlaceable;
        [SerializeField] private RectTransform leftPanel;

        private Camera _camera;
        
        public static PlaceablePlacement Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ClickManager.ClickEvent.OnTryPlacement += TryPlacement;
            ClickManager.ClickEvent.OnRotateBuilding += RotateBuilding;
            ClickManager.ClickEvent.OnCancel += CancelPlacement;
            _camera = Camera.main;
        }

        private void Update()
        {
            FollowMouseForPlacement();
        }

        private void FollowMouseForPlacement()
        {
            if (_unPlacedPlaceable==null)
                return;
            if (GridManager.GetPart.GetGridPart(InputManager.Mouse.GetMousePosToWorldPos())==null) return;
            var canPlace = CheckCanPlace();

            _unPlacedPlaceable.SetCanPlaceColor(canPlace);
            var position = GridManager.GetPart.GetGridPart(InputManager.Mouse.GetMousePosToWorldPos()).Transform
                .position;
            position.z = -.1f;
            _unPlacedPlaceable.Transform.position = position;
        }

        private bool CheckCanPlace()
        {
            return _unPlacedPlaceable.BuildingPieces.Select(pieces => pieces.position).Select(position => GridManager.GetPart.GetGridPart(position)).All(gridPart => gridPart is
            {
                Empty: true
            });
        }

        private void TryPlacement()
        {
            if (_unPlacedPlaceable==null)
                return;
            if (GridManager.GetPart.GetGridPart(InputManager.Mouse.GetMousePosToWorldPos())==null) return;
            var canPlace = CheckCanPlace();
            if (canPlace)
            {
                _unPlacedPlaceable.Placed();
                var position = GridManager.GetPart.GetGridPart(InputManager.Mouse.GetMousePosToWorldPos()).Transform
                    .position;
                _unPlacedPlaceable.Transform.position = position;
                foreach (var pieces in _unPlacedPlaceable.BuildingPieces)
                {
                    position = pieces.position;
                    var gridPart = GridManager.GetPart.GetGridPart(position);
                    gridPart.Empty = false;
                    gridPart.Unit = _unPlacedPlaceable.Transform;
                    gridPart.PiecePosition = _unPlacedPlaceable.GetPiecePosition;
                }

                _unPlacedPlaceable = null;
                ClickManager.Type.ClickType=ClickType.Nothing;
            }
            else
            {
                _unPlacedPlaceable.Transform.DOShakeScale(.5f, 1.2f).OnKill((() =>
                {
                    _unPlacedPlaceable.Transform.localScale = Vector3.one;
                }));
            }
        }

        private void RotateBuilding()
        {
            if (_unPlacedPlaceable==null)
                return;
            if (GridManager.GetPart.GetGridPart(InputManager.Mouse.GetMousePosToWorldPos())==null) return;
            _unPlacedPlaceable.Rotate();
        }
        
        public void CancelPlacement()
        {
            if (_unPlacedPlaceable==null)
                return;
            _unPlacedPlaceable.CancelPlacement();
            _unPlacedPlaceable = null;
            ClickManager.Type.ClickType=ClickType.Nothing;
        }

        public void SetUnplacedBuilding(IPlacement placeableEntity)
        {
            var mousePosToViewPoint = _camera.ScreenToViewportPoint(InputManager.Mouse.MousePos);
            var viewPointToWorldPosition = _camera.ViewportToWorldPoint(new Vector3(leftPanel.rect.xMax/1920f,mousePosToViewPoint.y));
            _unPlacedPlaceable = placeableEntity;
            viewPointToWorldPosition.x += .5f;
            var gridPart = GridManager.GetPart.GetGridPart(viewPointToWorldPosition);
            var position = gridPart.Transform.position;
            position.z = -.1f;
            _unPlacedPlaceable.Transform.position =position;
            var canPlace = CheckCanPlace();
            _unPlacedPlaceable.SetCanPlaceColor(canPlace);
        }
    }
}