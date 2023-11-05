using Building;
using InputSystem;
using ObjectPool;
using Placeble.Entity;
using UnitSelectionSystem;
using UnityEngine;

namespace Placeble.Tools
{
    public class PlacebleFactory : MonoBehaviour
    {
        [SerializeField] private PoolableObject refBarracks;
        [SerializeField] private PoolableObject refPowerPlants;
        [SerializeField] private PoolableObject refSpike;
        [SerializeField] private PoolableObject refTree;
        [SerializeField] private PoolableObject refWall;
        [SerializeField] private PoolableObject refUnit;

        private ObjectPool.ObjectPool _barracksPool;
        private ObjectPool.ObjectPool _powerPlantsPool;
        private ObjectPool.ObjectPool _spikesPool;
        private ObjectPool.ObjectPool _wallsPool;
        private ObjectPool.ObjectPool _treesPool;
        private ObjectPool.ObjectPool _unitsPool;
        
        public static PlacebleFactory Instance { get; private set; }
        
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

            _barracksPool = new ObjectPool.ObjectPool(refBarracks, 10, new PlacebleInitializeData());
            _powerPlantsPool = new ObjectPool.ObjectPool(refPowerPlants, 10, new PlacebleInitializeData());
            _spikesPool = new ObjectPool.ObjectPool(refSpike, 10, new PlacebleInitializeData());
            _wallsPool = new ObjectPool.ObjectPool(refWall, 10, new PlacebleInitializeData());
            _treesPool = new ObjectPool.ObjectPool(refTree, 10, new PlacebleInitializeData());
            _unitsPool = new ObjectPool.ObjectPool(refUnit, 10, new PlacebleInitializeData());
        }

        public void PopBuilding(PlacebleType placebleType, int lvl)
        {
            IPlacement placebleEntity;
            switch (placebleType)
            {
                case PlacebleType.Barracks:
                    placebleEntity=_barracksPool.GetEntity() as IPlacement;
                    break;
                case PlacebleType.PowerPlants:
                    placebleEntity=_powerPlantsPool.GetEntity() as IPlacement;
                    break;
                case PlacebleType.Spike:
                    placebleEntity=_spikesPool.GetEntity() as IPlacement;
                    break;
                case PlacebleType.Wall:
                    placebleEntity=_wallsPool.GetEntity() as IPlacement;
                    break;
                case PlacebleType.Unit:
                    placebleEntity=_unitsPool.GetEntity() as IPlacement;
                    break;
                case PlacebleType.Tree:
                    placebleEntity=_treesPool.GetEntity() as IPlacement;
                    break;
                default:
                    Debug.Log("The requested building type is not available!");
                    return;
            }

            if (placebleEntity != null)
            {
                placebleEntity.SetLevel(lvl);
                PlaceblePlacement.Instance.SetUnplacedBuilding(placebleEntity);
            }

            ClickManager.ClickType=ClickType.PlaceBuilding;
            UnitSelection.Instance.DeselectAll();
        }

        public UnitEntity InstantPopUnit()
        {
            return _unitsPool.GetEntity() as UnitEntity;
        }
    }
}