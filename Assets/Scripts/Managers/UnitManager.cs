using UnitSelectionSystem;
using UnityEngine;

namespace Managers
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private UnitSelection unitSelection;
        [SerializeField] private UnitDrag unitDrag;
        [SerializeField] private UnitClick unitClick;
        public void SetInstance()
        {
            unitSelection.SetInstance();
        }

        public void Initialize()
        {
            unitSelection.Initialize();
            unitDrag.Initialize();
            unitClick.Initialize();
        }
    }
}