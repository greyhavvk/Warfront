using System;
using Placeable;
using UnityEngine;

namespace UI.Information
{
    [Serializable]
    public class InformationScreenSprite
    {
       public PlaceableType informationScreen;
        public Sprite sprite;
    }
    [Serializable]
    public class InformationScreenModel
    {
        public InformationScreenSprite[] informationScreenSprites;

        public Sprite GetSprite(PlaceableType informationScreen)
        {
            var sprite = informationScreenSprites[0].sprite;

            for (int i = 1; i < informationScreenSprites.Length; i++)
            {
                if (informationScreen==informationScreenSprites[i].informationScreen)
                {
                    sprite = informationScreenSprites[i].sprite;
                }
            }

            return sprite;
        }
    }
}