using System;
using InputSystem;
using ObjectPool;
using Unit_Selection;
using UnityEngine;

namespace Building
{
    public class BuildingFactory : MonoBehaviour
    {
        [SerializeField] private BuildingEntity refBarracks;
        [SerializeField] private BuildingEntity refPowerPlants;

        private ObjectPool.ObjectPool _barracksPool;
        private ObjectPool.ObjectPool _powerPlantsPool;
        
        public static BuildingFactory Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance!=null && Instance!=this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            _barracksPool = new ObjectPool.ObjectPool(refBarracks, 10, new BuildingInitializeData());
            _powerPlantsPool = new ObjectPool.ObjectPool(refPowerPlants, 10, new BuildingInitializeData());
        }

        
        public void BuildBarrack()
        {
            PopBuilding(BuildingType.Barracks);
        }
        
        public void BuildPowerPlant()
        {
            PopBuilding(BuildingType.PowerPlants);
        }

        public void PopBuilding(BuildingType buildingType)
        {
            BuildingEntity buildingEntity;
            switch (buildingType)
            {
                case BuildingType.Barracks:
                    buildingEntity=_barracksPool.GetEntity() as BuildingEntity;
                    break;
                case BuildingType.PowerPlants:
                    buildingEntity=_powerPlantsPool.GetEntity() as BuildingEntity;
                    break;
                default:
                    Debug.Log("The requested building type is not available!");
                    return;
            }

            BuildingPlacement.Instance.SetUnplacedBuilding(buildingEntity);
            InputManager.Instance.inputType=InputType.PlaceBuilding;
            UnitSelection.Instance.DeselectAll();

        }
    }
}