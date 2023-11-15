using Placeable.Tools;
using UnityEngine;

namespace Managers
{
    public class PlaceableManager : MonoBehaviour
    {
        [SerializeField] private PlaceablePlacement placeablePlacement;
        [SerializeField] private PlaceableFactory placeableFactory;
        
        public void SetInstance()
        {
            placeablePlacement.SetInstance();
            placeableFactory.SetInstance();
        }

        public void Initialize()
        {
            placeablePlacement.Initialize();
            placeableFactory.Initialize();
        }
    }
}