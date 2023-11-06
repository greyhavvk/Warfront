using Managers;
using Placeble.Tools;
using UnityEngine;

namespace UI.ProductionButton
{
    public class ProductionMenuButtonController:MonoBehaviour
    {
        [SerializeField] private ProductionMenuModel productionMenuModel;
        [SerializeField] private ProductionMenuButtonView productionMenuButtonView;
        
        public void OnButtonClick()
        {
            PlacebleFactory.Instance.PopBuilding(productionMenuModel.placebleType, productionMenuModel.lvl);
            UIManager.SetActiveProduction.SetActiveProductionMenu(false);
        }
    }
}