using Building;
using InputSystem;
using Placeble.Entity;
using Placeble.Tools;
using UI;
using UI.Information;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour, ISetActiveProduction
    {
        public static ISetActiveProduction SetActiveProduction { get; private set; }
        
        [SerializeField] private InformationScreenController informationScreenController;
        [SerializeField] private GameObject productionMenu;
        [SerializeField] private GameObject informationMenu;
        [SerializeField] private GameObject outSideButton;
        [SerializeField] private LayerMask clickable;

        private Vector3 _spawnPoint;
        private void Awake()
        {
            if (SetActiveProduction!=null && SetActiveProduction!=this)
            {
                Destroy(gameObject);
            }
            else
            {
                SetActiveProduction = this;
            }
            
            CloseAllPanel();
        }

        private void Start()
        {
            ClickManager.ClickEvent.OnOpenProductionMenu += OpenProductionMenu;
            ClickManager.ClickEvent.OnCancel += CloseAllPanel;
            ClickManager.ClickEvent.OnPlacebleClick += PlacebleClick;
        }

        private void OnDestroy()
        {
            ClickManager.ClickEvent.OnOpenProductionMenu -= OpenProductionMenu;
            ClickManager.ClickEvent.OnCancel -= CloseAllPanel;
            ClickManager.ClickEvent.OnPlacebleClick -= PlacebleClick;
        }

        private void PlacebleClick()
        {
            if (ClickManager.Type.ClickType != ClickType.Nothing)
                return;
            var hit = Physics2D.Raycast( InputManager.Mouse.GetMousePosToWorldPos(), Vector2.zero, Mathf.Infinity, clickable);
            if (hit)
            {
                if ( hit.collider.gameObject.CompareTag("building"))
                {
                    ClickManager.Type.ClickType = ClickType.UIPanel;
                    OpenInformationPanel(hit.collider.GetComponent<IPlacebleType>().PlacebleType);
                }

                var barracks = hit.collider.GetComponent<BarrackEntity>();
                if (barracks)
                {
                    _spawnPoint = barracks.SpawnPoint.position;
                }
            }
        }

        public void OnSpawnUnitButtonClicked(int lvl)
        {
            UnitSpawner.SpawnUnit(_spawnPoint, lvl);
            //CloseAllPanel();
        }

        public void SetActiveProductionMenu(bool active)
        {
            productionMenu.SetActive(active);
            outSideButton.SetActive(active);
        }

        private void OpenProductionMenu()
        {
            CloseAllPanel();
            SetActiveProductionMenu(true);
        }
        
        private void OpenInformationPanel(PlacebleType placebleType)
        {
            CloseAllPanel();
            informationMenu.SetActive(true);
            outSideButton.SetActive(true);
            informationScreenController.SetMenu(placebleType);
        }

        public void CloseAllPanel()
        {
            informationMenu.SetActive(false);
            SetActiveProductionMenu(false);
            outSideButton.SetActive(false);
            ClickManager.Type.ClickType = ClickType.Nothing;
        }
    }
}