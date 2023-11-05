using Building;
using UnityEngine;

namespace UI.Information
{
    public class InformationScreenController : MonoBehaviour
    {
        [SerializeField] private InformationScreenView informationScreenView;
        [SerializeField] private InformationScreenModel informationScreenModel;

        public void SetMenu(PlacebleType placebleType)
        {
            informationScreenView.SetSprite(informationScreenModel.GetSprite(placebleType));
            informationScreenView.SetActiveButtons(placebleType==PlacebleType.Barracks);
        }
    }
}