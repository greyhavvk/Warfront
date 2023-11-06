using Building;
using Grid_System;
using InputSystem;
using Managers;
using ObjectPool;
using Placeble.PlacebleExtra;
using UnitSelectionSystem;
using UnityEngine;

namespace Placeble.Entity
{
    public class UnitEntity : DamagebleAndPlaceableEntity, IUnitSelecting
    {
        [SerializeField] private GameObject selectPointer;
        [SerializeField] private UnitMovement unitMovement;
        [SerializeField] private Attacker attacker;
        [SerializeField] private LayerMask ground;
        [HideInInspector]public bool active;
        [SerializeField] private UnitData[] datas;
        public override void Initialize(PoolableObjectInitializeData poolableObjectInitializeData)
        {
            base.Initialize(poolableObjectInitializeData);
            ClickManager.ClickEvent.OnUnitGetOrder += UnitGetOrder;
        }

        public void OnDestroy()
        {
            ClickManager.ClickEvent.OnUnitGetOrder -= UnitGetOrder;
        }

        public override void SetLevel(int lvl)
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
                unitMovement.TriggerMove(hit);
                attacker.SetAttackTarget();
            }
        }

        public override void Placed()
        {
            base.Placed();
            UnitEnabled();
            unitMovement.LastMoveGrid = GridManager.GetPart.GetGridPart(transform.position);
        }

        protected override void ReturnToPool()
        {
            unitMovement.StopMove();
            attacker.StopAttack();
            UnitDisable();
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
    }
}