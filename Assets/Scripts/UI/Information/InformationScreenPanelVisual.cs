using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Information
{
    [Serializable]
    public class InformationScreenPanelVisual
    {
        [SerializeField] private Image icon;
        [SerializeField] private GameObject gameObject;
        
        public Image Icon=>icon;
        public GameObject GameObject=>gameObject;
    }
}