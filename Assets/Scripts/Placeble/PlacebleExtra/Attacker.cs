using System.Collections;
using HitPointSystem;
using InputSystem;
using UnityEngine;

namespace Placeble.PlacebleExtra
{
    public class Attacker : MonoBehaviour
    {
        [SerializeField] private float timeBetweenAttacking;
        private float _damage;
        [SerializeField] private LayerMask canTakeDamage;
        private ITakeDamage _target;
        private bool _isAttacking;

        private void Update()
        {
            CheckTargetIsActive();
        }

        private bool CheckTargetIsActive()
        {
            if (_target == null) return false;
            if (!_target.GameObject.activeSelf)
            {
                StopAttack();
                return false;
            }

            return true;
        }

        public void SetAttackTarget()
        {
            var hit = Physics2D.Raycast(InputManager.Mouse.GetMousePosToWorldPos(), Vector2.zero, Mathf.Infinity, canTakeDamage);
            if (hit)
                if (hit.transform!=transform.parent)
                    _target = hit.transform.GetComponent<ITakeDamage>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (_isAttacking) return;
            if (_target == null) return;
            if (_target.GameObject != other.gameObject) return;
            _isAttacking = true;
            StartCoroutine(Attack());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_target == null) return;
            if (_target.GameObject != other.gameObject) return;
            _isAttacking = false;
            StopCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            while (CheckTargetIsActive())
            {
                yield return new WaitForSeconds(timeBetweenAttacking);
                _target.TakeDamage(_damage);
            }
        }

        public void StopAttack()
        {
            StopCoroutine(Attack());
            _target = null;
        }

        public void SetDamage(float dataDamage)
        {
            _damage = dataDamage;
        }
    }
}