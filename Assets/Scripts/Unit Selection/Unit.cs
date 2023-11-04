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

        private void Start()
        {
            UnitEnabled();
        }

        public void UnitEnabled()
        {
            UnitSelection.Instance.AddObjectToUnitList(gameObject, this);
            UnitUnselected();
        }

        public void UnitSelected()
        {
            selectPointer.SetActive(true);
            unitMovement.active = true;
        }

        public void UnitUnselected()
        {
            selectPointer.SetActive(false);
            unitMovement.active = false;
        }
    }
}