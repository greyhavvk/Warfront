using System.Collections.Generic;
using InputSystem;
using Managers;
using ObjectPool;
using Placeable.PlaceableExtra;
using UnitSelectionSystem;
using UnityEngine;

namespace Placeable.Entity
{
    public class UnitEntity : PoolableObject, IUnitSelecting,IGetPiecePosition
    {
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] private HealthPointObserver healthPointObserver;
        [SerializeField] private GameObject selectPointer;
        [SerializeField] private UnitMovement unitMovement;
        [SerializeField] private Attacker attacker;
        [SerializeField] private LayerMask ground;
        [HideInInspector]public bool active;
        [SerializeField] private UnitData[] datas;

        private List<Transform> _pieces;
        
        public override void Initialize(PoolableObjectInitializeData poolableObjectInitializeData)
        {
            base.Initialize(poolableObjectInitializeData);
            ClickManager.ClickEvent.OnUnitGetOrder += UnitGetOrder;
            _pieces = new List<Transform>();
            _pieces.Add(transform);
            healthPointObserver.Initialize(ReturnToPool);
            unitMovement.GetPiecePosition = this;
        }

        public void OnDestroy()
        {
            ClickManager.ClickEvent.OnUnitGetOrder -= UnitGetOrder;
        }

        public void SetLevel(int lvl)
        {
            var data = datas[lvl];
            spriteRenderer.sprite = data.sprite;
            healthPointObserver.SetHp(data.hp);
            attacker.SetDamage(data.damage);
        }

        private void UnitGetOrder()
        {
            if (!active)
                return;
            RaycastHit2D hit = Physics2D.Raycast(InputManager.Mouse.GetMousePosToWorldPos(), Vector2.zero, Mathf.Infinity, ground);
            if (hit)
            {
                var gridPart = GridManager.GetPart.GetGridPart(hit.point);
                if (gridPart == null)
                    return;
                if ((Vector2)gridPart.Transform.position!=(Vector2)unitMovement.LastMoveGrid.Transform.position)
                {
                    unitMovement.MoveRequest(gridPart);
                    attacker.SetAttackTarget();
                }
            }
        }

        public void Placed()
        {
            healthPointObserver.ResetHp();
            UnitEnabled();
            unitMovement.LastMoveGrid = GridManager.GetPart.GetGridPart(transform.position);
        }

        protected override void ReturnToPool()
        {
            unitMovement.StopMove();
            attacker.StopAttack();
            UnitDisable();
            var grid = GridManager.GetPart.GetGridPart(transform.position);
            grid.Empty = true;
            grid.PiecePosition = null;
            base.ReturnToPool();
        }

        private void UnitDisable()
        {
            UnitSelection.Instance.RemoveObjectToUnitList(gameObject);
            UnitUnselected();
            UnitSelection.Instance.Deselect(gameObject);
        }

        private void UnitEnabled()
        {
            UnitSelection.Instance.AddObjectToUnitList(gameObject, this);
            UnitUnselected();
        }

        public void UnitSelected()
        {
            selectPointer.SetActive(true);
            active = true;
        }

        public void UnitUnselected()
        {
            selectPointer.SetActive(false);
            active = false;
        }

        public List<Transform> Pieces => _pieces;
    }
}