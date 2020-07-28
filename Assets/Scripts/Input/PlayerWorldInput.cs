using System;
using Model.Geo.Organization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Input
{
    public class PlayerWorldInput : MonoBehaviour
    {
        public new Camera camera;
        public WorldMap map;

        public void OnSelect(InputAction.CallbackContext context)
        {
            if (EventSystem.current.IsPointerOverGameObject(-1)) return;
            if (!context.performed) return;
            var pos = Mouse.current.position;
            var x = pos.x.ReadValue();
            var y = pos.y.ReadValue();
            var ray = camera.ScreenPointToRay(new Vector3(x, y, 0));
            var hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider == null) return;
            map.UpdateSelection(hit.collider.gameObject.GetComponent<WorldTile>());
        }

        public void OnScroll(InputAction.CallbackContext context)
        {
            if (!(context.ReadValueAsObject() is Vector2 value)) return;
            if (value.y < 0)
            {
                if (camera.orthographicSize >= 20)
                {
                    camera.orthographicSize -= 4;
                }
            }
            else
            {
                if (camera.orthographicSize <= 120)
                {
                    camera.orthographicSize += 4;
                }
            }
        }

        public void OnPanWorldMap(InputAction.CallbackContext context)
        {
            switch (context.control.name)
            {
                case "w":
                    _moveUp = context.performed;
                    break;
                case "s":
                    _moveDown = context.performed;
                    break;
                case "d":
                    _moveRight = context.performed;
                    break;
                case "a":
                    _moveLeft = context.performed;
                    break;
                case "rightButton":
                    _panIsActive = context.performed;
                    break;
                default:
                    Debug.Log(context.control.name);
                    break;
            }
        }

        private bool _moveUp;
        private bool _moveDown;
        private bool _moveRight;
        private bool _moveLeft;
        private bool _panIsActive;

        private void TranslateCamera()
        {
            var (x, y) = DetermineDelta();
            camera.transform.Translate(new Vector3(x, y, 0));
        }

        private Tuple<int, int> DetermineDelta()
        {
            if (_moveUp) return new Tuple<int, int>(0, 4);
            if (_moveDown) return new Tuple<int, int>(0, -4);
            if (_moveRight) return new Tuple<int, int>(4, 0);
            if (_moveLeft) return new Tuple<int, int>(-4, 0);
            if (!_panIsActive) return new Tuple<int, int>(0, 0);
            var delta = Mouse.current.delta;
            var x = delta.x.ReadValue();
            var y = delta.y.ReadValue();
            return new Tuple<int, int>((int) x, (int) y);
        }

        private void Update()
        {
            TranslateCamera();
        }
    }
}