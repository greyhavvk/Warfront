using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HitPointSystem
{
    public class HealthPointObserver : MonoBehaviour
    {
        [SerializeField] private float initialHitPoint;
        [SerializeField] private Image progress;
        [SerializeField] private GameObject hpPanel;
        private float _currentHitPoint;
        private Action _dieAction;

        public void Initialize(Action dieAction)
        {
            _currentHitPoint = initialHitPoint;
            _dieAction = dieAction;
            hpPanel.SetActive(false);
        }

        public void ResetHp()
        {
            _currentHitPoint = initialHitPoint;
            hpPanel.SetActive(true);
        }
        
        private void UpdateProgress()
        {
            var normalizedValue = _currentHitPoint/initialHitPoint;
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

        private void Die()
        {
            _dieAction?.Invoke();
            hpPanel.SetActive(false);
        }
    }
}