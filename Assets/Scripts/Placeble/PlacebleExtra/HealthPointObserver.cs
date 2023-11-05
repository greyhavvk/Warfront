using System;
using UnityEngine;
using UnityEngine.UI;

namespace HitPointSystem
{
    public class HealthPointObserver : MonoBehaviour,ITakeDamage
    {
        [SerializeField] private float _maxHp;
        [SerializeField] private Image progress;
        [SerializeField] private GameObject hpPanel;
        private float _currentHitPoint;
        private Action _dieAction;

        public void Initialize(Action dieAction)
        {
            _currentHitPoint = _maxHp;
            UpdateProgress();
            _dieAction = dieAction;
            hpPanel.SetActive(false);
        }

        public void ResetHp()
        {
            _currentHitPoint = _maxHp;
            UpdateProgress();
            hpPanel.SetActive(true);
        }
        
        private void UpdateProgress()
        {
            var normalizedValue = _currentHitPoint/_maxHp;
            progress.fillAmount = normalizedValue;
        }

        public void TakeDamage(float damage)
        {
            _currentHitPoint -= damage;
            UpdateProgress();
            if (_currentHitPoint<=0)
            {
                Die();
            }
        }

        public GameObject GameObject => gameObject;

        private void Die()
        {
            _dieAction?.Invoke();
            hpPanel.SetActive(false);
        }

        public void SetHp(float dataHp)
        {
            _maxHp=dataHp;
        }
    }
}