using UnityEngine;

namespace Unit_Selection
{
    public class Unit : MonoBehaviour, IUnitSelecting, IUnitActive
    {
        [SerializeField] private GameObject selectPointer;
        [SerializeField] private UnitMovement unitMovement;
        
        public void UnitDisable()
        {
            UnitSelection.Instance.RemoveObjectToUnitList(gameObject);
            UnitUnselected();
        }
        
        public void UnitEnabled()
        {
            UnitSelection.Instance.AddObjectToUnitList(gameObject, this);
            UnitUnselected();
        }

        public void UnitSelected()
        {
            selectPointer.SetActive(true);
            unitMovement.enabled = true;
        }

        public void UnitUnselected()
        {
            selectPointer.SetActive(false);
            unitMovement.enabled = false;
        }
    }
}