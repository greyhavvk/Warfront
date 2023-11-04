using System;
using System.Linq;
using DG.Tweening;
using Grid_System;
using InputSystem;
using UnityEngine;

namespace Building
{
    public class BuildingPlacement : MonoBehaviour
    {
        private BuildingEntity _unPlacedBuilding;

        private Camera _camera;
        
        public static BuildingPlacement Instance { get; private set; }
        
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
            InputManager.Instance.OnMouseLeftClickDown += TryPlacement;
            InputManager.Instance.OnRotateButtonDown += RotateBuilding;
            InputManager.Instance.OnCancelButtonDown += CancelPlacement;
        }

        private void Update()
        {
            FollowMouseForPlacement();
        }

        private void FollowMouseForPlacement()
        {
            if (!_unPlacedBuilding)
                return;
            if (!GridController.Instance.GetGridPart(InputManager.Instance.GetMousePosToWorldPos())) return;
            var canPlace = CheckCanPlace();

            _unPlacedBuilding.SetCanPlaceColor(canPlace);
            var position = GridController.Instance.GetGridPart(InputManager.Instance.GetMousePosToWorldPos()).transform
                .position;
            _unPlacedBuilding.transform.position = position;
        }

        private bool CheckCanPlace()
        {
            return _unPlacedBuilding.BuildingPieces.Select(pieces => pieces.position).Select(position => GridController.Instance.GetGridPart(position)).All(gridPart => gridPart != null && gridPart.Empty);
        }

        private void TryPlacement()
        {
            if (!_unPlacedBuilding)
                return;
            if (!GridController.Instance.GetGridPart(InputManager.Instance.GetMousePosToWorldPos())) return;
            var canPlace = CheckCanPlace();
            if (canPlace)
            {
                _unPlacedBuilding.Placed();
                foreach (var pieces in _unPlacedBuilding.BuildingPieces)
                {
                    var position = pieces.position;
                    var gridPart = GridController.Instance.GetGridPart(position);
                    gridPart.Empty = false;
                }

                _unPlacedBuilding = null;
                InputManager.Instance.inputType=InputType.Nothing;
            }
            else
            {
                _unPlacedBuilding.transform.DOShakeScale(.5f, 1.2f).OnKill((() =>
                {
                    _unPlacedBuilding.transform.localScale = Vector3.one;
                }));
            }
        }

        private void RotateBuilding()
        {
            if (!_unPlacedBuilding)
                return;
            if (!GridController.Instance.GetGridPart(InputManager.Instance.GetMousePosToWorldPos())) return;
            _unPlacedBuilding.Rotate();
        }
        
        private void CancelPlacement()
        {
            if (!_unPlacedBuilding)
                return;
            _unPlacedBuilding.CancelPlacement();
            _unPlacedBuilding = null;
            InputManager.Instance.inputType=InputType.Nothing;
        }

        public void SetUnplacedBuilding(BuildingEntity buildingEntity)
        {
            _unPlacedBuilding = buildingEntity;
        }
    }
}