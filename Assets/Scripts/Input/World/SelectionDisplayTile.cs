using System;
using Model.Geo.Support;
using UnityEngine;

namespace Input.World
{
    public class SelectionDisplayTile : MonoBehaviour
    {
        public Position position;
        public new SpriteRenderer renderer;
        public bool IsActive => renderer.enabled;
        public void SetActive(bool value)
        {
            renderer.enabled = value;
        }
        
    }
}