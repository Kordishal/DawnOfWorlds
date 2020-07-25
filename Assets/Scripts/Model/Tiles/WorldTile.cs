using System;
using System.Collections.Generic;
using Model.TileFeatures;
using Model.World;
using UnityEngine;

namespace Model.Tiles
{
    [Serializable]
    public class WorldTile : MonoBehaviour
    {
        public Position Position { get; private set; }
        public List<WeatherEffect> weatherEffects;
        public Biome biome;
        
        private const int PositionFactor = 7;
        
        private void Start()
        {
            name = "Tile (" + Position.x + ", " + Position.y + ")";

        }

        public bool IsInBounds(Vector3 pos)
        {
            var x = (int) pos.x;
            var y = (int) pos.y;
            if (x < Position.x || x > Position.x + PositionFactor) return false;
            return y > Position.y && y < Position.y + PositionFactor;
        }
        public void ChangePosition(Position newPosition)
        {
            Position = newPosition;
            transform.position = new Vector3(Position.x * PositionFactor, Position.y * PositionFactor, 0);
        }


        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
