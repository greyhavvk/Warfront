using InputSystem;
using Managers;
using ObjectPool;
using Placeable.Entity;
using UnitSelectionSystem;
using UnityEngine;

namespace Placeable.Tools
{
    public class PlaceableFactory : MonoBehaviour
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
        
        public static PlaceableFactory Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            
            _barracksPool = new ObjectPool.ObjectPool(refBarracks, 10, new PlaceableInitializeData());
            _powerPlantsPool = new ObjectPool.ObjectPool(refPowerPlants, 10, new PlaceableInitializeData());
            _spikesPool = new ObjectPool.ObjectPool(refSpike, 10, new PlaceableInitializeData());
            _wallsPool = new ObjectPool.ObjectPool(refWall, 10, new PlaceableInitializeData());
            _treesPool = new ObjectPool.ObjectPool(refTree, 10, new PlaceableInitializeData());
            _unitsPool = new ObjectPool.ObjectPool(refUnit, 10, new PlaceableInitializeData());
        }

        public void PopBuilding(PlaceableType placeableType)
        {
            IPlacement placeableEntity;
            switch (placeableType)
            {
                case PlaceableType.Barracks:
                    placeableEntity=_barracksPool.GetEntity() as IPlacement;
                    break;
                case PlaceableType.PowerPlants:
                    placeableEntity=_powerPlantsPool.GetEntity() as IPlacement;
                    break;
                case PlaceableType.Spike:
                    placeableEntity=_spikesPool.GetEntity() as IPlacement;
                    break;
                case PlaceableType.Wall:
                    placeableEntity=_wallsPool.GetEntity() as IPlacement;
                    break;
                case PlaceableType.Tree:
                    placeableEntity=_treesPool.GetEntity() as IPlacement;
                    break;
                default:
                    Debug.Log("The requested building type is not available!");
                    return;
            }

            if (placeableEntity != null)
            {
                PlaceablePlacement.Instance.SetUnplacedBuilding(placeableEntity);
            }

            ClickManager.Type.ClickType=ClickType.PlaceBuilding;
            UnitSelection.Instance.DeselectAll();
        }

        public UnitEntity InstantPopUnit()
        {
            return _unitsPool.GetEntity() as UnitEntity;
        }
    }
}