using System;
using TMPro;
using UnityEngine;

namespace Placeable.PlaceableExtra
{
    public class HealthPointObserver : MonoBehaviour,ITakeDamage,IHpInfo
    {
        [SerializeField] private float maxHp;
        [SerializeField] private GameObject hpPanel;
        [SerializeField] private TMP_Text healthText;
        private float _currentHitPoint;
        public float MaxHp => maxHp;
        public float CurrentHp => _currentHitPoint;
        public Action OnDie { get; set; }

        public void Initialize(Action dieAction)
        {
            _currentHitPoint = maxHp;
            UpdateProgress();
            OnDie = dieAction;
            hpPanel.SetActive(false);
        }

        public void ResetHp()
        {
            _currentHitPoint = maxHp;
            UpdateProgress();
           hpPanel.SetActive(true);
        }
        
        private void UpdateProgress()
        {
            healthText.text=_currentHitPoint + "/" + maxHp;
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
            OnDie?.Invoke();
            hpPanel.SetActive(false);
        }

        public void SetHp(float dataHp)
        {
            maxHp=dataHp;
        }
    }
}