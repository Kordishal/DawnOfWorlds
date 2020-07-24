using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Input
{
    public class Zoom : MonoBehaviour
    {
        public new Camera camera;
        
        public void OnScroll(InputAction.CallbackContext context)
        {
            if (!(context.ReadValueAsObject() is Vector2 value)) return;
            if (value.y < 0)
            {
                camera.orthographicSize -= 0.5f;
            }
            else
            {
                camera.orthographicSize += 0.5f;
            }
        }
    
        
        // Start is called before the first frame update
        void Start()
        {
            camera = GetComponent<Camera>();
            camera.orthographicSize = 10;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
