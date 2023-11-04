using System;
using UnityEngine;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        private Camera _camera;
        private InputType _inputType = InputType.Nothing;
        public InputType InputType => _inputType;

        private InputManager _instance;
        public static InputManager Instance;

        public bool GetKey(KeyCode keyCode) => UnityEngine.Input.GetKey(keyCode);

        private void Awake()
        {
            if (_instance!=null && _instance!=this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            _camera=Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                
            }

            if (Input.GetMouseButtonDown(1))
            {
                
            }
            
        }
        
        public Vector2 GetMousePosToWorldPos(){
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            return new Vector2(mousePos.x, mousePos.y);
        }
    }
}