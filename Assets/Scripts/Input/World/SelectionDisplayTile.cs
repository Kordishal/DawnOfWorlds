using Model.Geo.Support;
using UnityEngine;

namespace Input.World
{
    public class SelectionDisplayTile : MonoBehaviour
    {
        public Position position;
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}