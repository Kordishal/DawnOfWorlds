using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace Input
{
    public class SelectTile : MonoBehaviour
    {

        public Tilemap map;

        public void OnSelect(InputAction.CallbackContext context)
        {
        
        }
    
        // Start is called before the first frame update
        void Start()
        {
            map = GetComponent<Tilemap>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
