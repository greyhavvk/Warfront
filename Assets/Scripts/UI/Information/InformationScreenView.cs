using UnityEngine;

namespace UI.Information
{
    public class InformationScreenView : MonoBehaviour
    {
        [SerializeField] private InformationScreenPanelVisual[] visuals;
        private InformationScreenPanelVisual _currentVisual;

        public void SetSprite(Sprite sprite)
        {
            _currentVisual.Icon.sprite = sprite;
        }

        public void EnablePanel(bool isBarracks)
        {
            DisablePanel();
            _currentVisual = isBarracks ? visuals[1] : visuals[0];
            _currentVisual.GameObject.SetActive(true);
            
        }

        public void DisablePanel()
        {
            foreach (var visual in visuals)
                visual.GameObject.SetActive(false);
        }
    }
}