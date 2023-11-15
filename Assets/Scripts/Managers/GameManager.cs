using System;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private ClickManager clickManager;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private PlaceableManager placeableManager;
        [SerializeField] private UnitManager unitManager;

        private void Awake()
        {
           SetInstance();
           Initialize();
        }

        private void SetInstance()
        {
            clickManager.SetInstance();
            gridManager.SetInstance();
            inputManager.SetInstance();
            placeableManager.SetInstance();
            unitManager.SetInstance();
        }

        private void Initialize()
        {
            clickManager.Initialize();
            gridManager.Initialize();
            inputManager.Initialize();
            placeableManager.Initialize();
            unitManager.Initialize();
            uiManager.Initialize();
        }
    }
}