using System.Linq;
using Building;
using DG.Tweening;
using InputSystem;
using Managers;
using UnityEngine;

namespace Placeble.Tools
{
    public class PlaceblePlacement : MonoBehaviour
    {
        private IPlacement _unPlacedPlaceble;

        private Camera _camera;
        
        public static PlaceblePlacement Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ClickManager.ClickEvent.OnTryPlacement += TryPlacement;
            ClickManager.ClickEvent.OnRotateBuilding += RotateBuilding;
            ClickManager.ClickEvent.OnCancel += CancelPlacement;
        }

        private void Update()
        {
            FollowMouseForPlacement();
        }

        private void FollowMouseForPlacement()
        {
            if (_unPlacedPlaceble==null)
                return;
            if (GridManager.GetPart.GetGridPart(InputManager.Mouse.GetMousePosToWorldPos())==null) return;
            var canPlace = CheckCanPlace();

            _unPlacedPlaceble.SetCanPlaceColor(canPlace);
            var position = GridManager.GetPart.GetGridPart(InputManager.Mouse.GetMousePosToWorldPos()).Transform
                .position;
            _unPlacedPlaceble.Transform.position = position;
        }

        private bool CheckCanPlace()
        {
            return _unPlacedPlaceble.BuildingPieces.Select(pieces => pieces.position).Select(position => GridManager.GetPart.GetGridPart(position)).All(gridPart => gridPart != null && gridPart.Empty);
        }

        private void TryPlacement()
        {
            if (_unPlacedPlaceble==null)
                return;
            if (GridManager.GetPart.GetGridPart(InputManager.Mouse.GetMousePosToWorldPos())==null) return;
            var canPlace = CheckCanPlace();
            if (canPlace)
            {
                _unPlacedPlaceble.Placed();
                foreach (var pieces in _unPlacedPlaceble.BuildingPieces)
                {
                    var position = pieces.position;
                    var gridPart = GridManager.GetPart.GetGridPart(position);
                    gridPart.Empty = false;
                    gridPart.Unit = _unPlacedPlaceble.Transform;
                }

                _unPlacedPlaceble = null;
                ClickManager.Type.ClickType=ClickType.Nothing;
            }
            else
            {
                _unPlacedPlaceble.Transform.DOShakeScale(.5f, 1.2f).OnKill((() =>
                {
                    _unPlacedPlaceble.Transform.localScale = Vector3.one;
                }));
            }
        }

        private void RotateBuilding()
        {
            if (_unPlacedPlaceble==null)
                return;
            if (GridManager.GetPart.GetGridPart(InputManager.Mouse.GetMousePosToWorldPos())==null) return;
            _unPlacedPlaceble.Rotate();
        }
        
        private void CancelPlacement()
        {
            if (_unPlacedPlaceble==null)
                return;
            _unPlacedPlaceble.CancelPlacement();
            _unPlacedPlaceble = null;
            ClickManager.Type.ClickType=ClickType.Nothing;
        }

        public void SetUnplacedBuilding(IPlacement placebleEntity)
        {
            _unPlacedPlaceble = placebleEntity;
        }
    }
}