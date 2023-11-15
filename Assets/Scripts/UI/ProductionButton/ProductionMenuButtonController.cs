using Managers;
using Placeable.Tools;
using UnityEngine;

namespace UI.ProductionButton
{
    public class ProductionMenuButtonController:MonoBehaviour
    {
        [SerializeField] private ProductionMenuModel productionMenuModel;
        [SerializeField] private ProductionMenuButtonView productionMenuButtonView;
        
        public void OnButtonClick()
        {
            PlaceablePlacement.Instance.CancelPlacement();
            PlaceableFactory.Instance.PopBuilding(productionMenuModel.PlaceableType);
        }
    }
}