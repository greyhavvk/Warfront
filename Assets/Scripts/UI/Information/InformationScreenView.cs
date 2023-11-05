using UnityEngine;
using UnityEngine.UI;

namespace UI.Information
{
    public class InformationScreenView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private GameObject buttons;

        public void SetSprite(Sprite sprite)
        {
            image.sprite = sprite;
        }

        public void SetActiveButtons(bool active)
        {
            buttons.SetActive(active);
        }
    }
}