using System;
using Placeable;
using UnityEngine;

namespace UI.ProductionButton
{
    [Serializable]
    public class ProductionMenuModel
    {
        [SerializeField] private PlaceableType placeableType;

        public PlaceableType PlaceableType => placeableType;
    }
}