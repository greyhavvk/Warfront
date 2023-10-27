using System;
using UnityEngine;
using UnityEngine.AI;

namespace Unit_Selection
{
    public class UnitMovement : MonoBehaviour
    {
        private Camera _camera;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private LayerMask ground;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray,out var hit, Mathf.Infinity,ground))
                {
                    navMeshAgent.SetDestination(hit.point);
                }
            }
        }
    }
}