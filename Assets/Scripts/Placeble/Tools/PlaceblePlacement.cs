using System;
using System.Linq;
using DG.Tweening;
using Grid_System;
using InputSystem;
using Placeble;
using UnityEngine;

namespace Building
{
    public class PlaceblePlacement : MonoBehaviour
    {
        private IPlacement _unPlacedPlaceble;

        private Camera _camera;
        
        public static PlaceblePlacement Instance { get; private set; }
        
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
            ClickManager.Instance.OnTryPlacement += TryPlacement;
            ClickManager.Instance.OnRotateBuilding += RotateBuilding;
            ClickManager.Instance.OnCancel += CancelPlacement;
        }

        private void Update()
        {
            FollowMouseForPlacement();
        }

        private void FollowMouseForPlacement()
        {
            if (_unPlacedPlaceble==null)
                return;
            if (!GridManager.Instance.GetGridPart(InputManager.Mouse.GetMousePosToWorldPos())) return;
            var canPlace = CheckCanPlace();

            _unPlacedPlaceble.SetCanPlaceColor(canPlace);
            var position = GridManager.Instance.GetGridPart(InputManager.Mouse.GetMousePosToWorldPos()).transform
                .position;
            _unPlacedPlaceble.Transform.position = position;
        }

        private bool CheckCanPlace()
        {
            return _unPlacedPlaceble.BuildingPieces.Select(pieces => pieces.position).Select(position => GridManager.Instance.GetGridPart(position)).All(gridPart => gridPart != null && gridPart.Empty);
        }

        private void TryPlacement()
        {
            if (_unPlacedPlaceble==null)
                return;
            if (!GridManager.Instance.GetGridPart(InputManager.Mouse.GetMousePosToWorldPos())) return;
            var canPlace = CheckCanPlace();
            if (canPlace)
            {
                _unPlacedPlaceble.Placed();
                foreach (var pieces in _unPlacedPlaceble.BuildingPieces)
                {
                    var position = pieces.position;
                    var gridPart = GridManager.Instance.GetGridPart(position);
                    gridPart.Empty = false;
                    gridPart.unit = _unPlacedPlaceble.Transform;
                }

                _unPlacedPlaceble = null;
                ClickManager.ClickType=ClickType.Nothing;
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
            if (!GridManager.Instance.GetGridPart(InputManager.Mouse.GetMousePosToWorldPos())) return;
            _unPlacedPlaceble.Rotate();
        }
        
        private void CancelPlacement()
        {
            if (_unPlacedPlaceble==null)
                return;
            _unPlacedPlaceble.CancelPlacement();
            _unPlacedPlaceble = null;
            ClickManager.ClickType=ClickType.Nothing;
        }

        public void SetUnplacedBuilding(IPlacement placebleEntity)
        {
            _unPlacedPlaceble = placebleEntity;
        }
    }
}