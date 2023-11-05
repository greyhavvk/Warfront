using Building;
using InputSystem;
using Placeble;
using Placeble.Tools;
using UI.Information;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        
        [SerializeField] private InformationScreenController informationScreenController;
        [SerializeField] private GameObject productionMenu;
        [SerializeField] private GameObject informationMenu;
        [SerializeField] private GameObject outSideButton;
        [SerializeField] private LayerMask clickable;

        private Vector3 _spawnPoint;
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
            
            CloseAllPanel();
        }

        private void Start()
        {
            ClickManager.Instance.OnOpenProductionMenu += OpenProductionMenu;
            ClickManager.Instance.OnCancel += CloseAllPanel;
            ClickManager.Instance.OnPlacebleClick += PlacebleClick;
        }

        private void OnDestroy()
        {
            ClickManager.Instance.OnOpenProductionMenu -= OpenProductionMenu;
            ClickManager.Instance.OnCancel -= CloseAllPanel;
            ClickManager.Instance.OnPlacebleClick -= PlacebleClick;
        }

        private void PlacebleClick()
        {
            if (ClickManager.ClickType != ClickType.Nothing)
                return;
            var hit = Physics2D.Raycast( InputManager.Mouse.GetMousePosToWorldPos(), Vector2.zero, Mathf.Infinity, clickable);
            if (hit)
            {
                if ( hit.collider.gameObject.CompareTag("building"))
                {
                    ClickManager.ClickType = ClickType.UIPanel;
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
            CloseAllPanel();
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
        
        public void OpenInformationPanel(PlacebleType placebleType)
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
            ClickManager.ClickType = ClickType.Nothing;
        }
    }
}