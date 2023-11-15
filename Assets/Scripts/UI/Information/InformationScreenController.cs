using Placeable;
using Placeable.PlaceableExtra;
using UnityEngine;

namespace UI.Information
{
    public class InformationScreenController : MonoBehaviour
    {
        [SerializeField] private InformationScreenView informationScreenView;
        [SerializeField] private InformationScreenModel informationScreenModel;

        public void SetMenu(PlaceableType placeableType)
        {
            informationScreenView.EnablePanel(placeableType==PlaceableType.Barracks);
            informationScreenView.SetSprite(informationScreenModel.GetSprite(placeableType));
        }

        public void DisablePanel()
        {
            informationScreenView.DisablePanel();
        }
    }
}