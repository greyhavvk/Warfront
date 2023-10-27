using UnityEngine;

namespace Unit_Selection
{
    public class UnitClick : MonoBehaviour
    {
        private Camera _camera;

        [SerializeField] private GameObject groundMarker;
        [SerializeField] private LayerMask clickable;
        [SerializeField] private LayerMask ground;
        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray,out var hit, Mathf.Infinity,clickable))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        UnitSelection.Instance.ShiftClickSelect(hit.collider.gameObject);
                    }
                    else
                    {
                        UnitSelection.Instance.ClickSelect(hit.collider.gameObject);
                    }
                }
                else
                {
                    if (!Input.GetKey(KeyCode.LeftShift))
                    {
                        UnitSelection.Instance.DeselectAll();
                    }
                   
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray,out var hit, Mathf.Infinity,ground))
                {
                    groundMarker.transform.position = hit.point;
                    groundMarker.SetActive(false);
                    groundMarker.SetActive(true);
                }
            }
        }
    }
}