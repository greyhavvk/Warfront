using System;
using InputSystem;
using Placeable;
using Placeable.Entity;
using Placeable.PlaceableExtra;
using Placeable.Tools;
using UI;
using UI.Information;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        
        [SerializeField] private InformationScreenController informationScreenController;
        [SerializeField] private LayerMask clickable;

        private Vector2[] _spawnPoints;
        private IHpInfo _listenedHpInfo;

        private void Start()
        {
            ClickManager.ClickEvent.OnPlaceableClick += PlaceableClick;
        }

        private void OnDestroy()
        {
            ClickManager.ClickEvent.OnPlaceableClick -= PlaceableClick;
        }

        private void PlaceableClick()
        {
            if (ClickManager.Type.ClickType != ClickType.Nothing)
                return;
            var hit = Physics2D.Raycast( InputManager.Mouse.GetMousePosToWorldPos(), Vector2.zero, Mathf.Infinity, clickable);
            if (!hit) return;
            if (!hit.collider.gameObject.CompareTag("building")) return;
            SetInformationPanel(hit.collider.GetComponent<IPlaceableType>().PlaceableType, hit.collider.GetComponent<IHpInfo>());
                    
            var barracks = hit.collider.GetComponent<BarrackEntity>();
            if (barracks)
            {
                _spawnPoints = barracks.SpawnPoints;
            }
        }

        private void SetListenedHpInfo(IHpInfo hpInfo)
        {
            if (_listenedHpInfo != null)
            {
                _listenedHpInfo.OnDie -= ListenedHpInfoDied;
                _listenedHpInfo = null;
            }
                
            if (hpInfo!=null)
            {
                _listenedHpInfo = hpInfo;
                _listenedHpInfo.OnDie += ListenedHpInfoDied;
            }
        }

        private void ListenedHpInfoDied()
        {
            informationScreenController.DisablePanel();
        }

        private void SetInformationPanel(PlaceableType placeableType, IHpInfo hpInfo)
        {
            informationScreenController.SetMenu(placeableType);
            SetListenedHpInfo(hpInfo);
        }


        public void OnSpawnUnitButtonClicked(int lvl)
        {
            UnitSpawner.SpawnUnit(_spawnPoints, lvl);
        }
    }
}