using System;
using Building;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Information
{
    [Serializable]
    public class InformationScreenSprite
    {
       public PlacebleType informationScreen;
        public Sprite sprite;
    }
    [Serializable]
    public class InformationScreenModel
    {
        public InformationScreenSprite[] informationScreenSprites;

        public Sprite GetSprite(PlacebleType informationScreen)
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