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
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, clickable);
                if (hit)
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
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, Mathf.Infinity, ground);
                if (hit)
                {
                    groundMarker.transform.position = hit.point;
                    groundMarker.SetActive(false);
                    groundMarker.SetActive(true);
                }
            }
        }
    }
}