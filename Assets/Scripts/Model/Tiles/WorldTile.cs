using System;
using System.Collections.Generic;
using Meta.EventArgs;
using Model.TileFeatures;
using Model.World;
using UnityEngine;

namespace Model.Tiles
{
    [Serializable]
    public class WorldTile : MonoBehaviour
    {
        public Position Position { get; private set; }
        private List<WeatherEffect> _weatherEffects;

        public string WeatherEffectToString()
        {
            return string.Join(", ", _weatherEffects);
        }
        private Biome _biome;

        public string BiomeToString()
        {
            return _biome.ToString();
        }

        private const int PositionFactor = 7;

        private void Start()
        {
            name = "Tile (" + Position.x + ", " + Position.y + ")";
            _weatherEffects = new List<WeatherEffect>();
            _biome = new Biome("Barren", "There is no life here.");
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


        public void RemoveWeatherEffect(WeatherEffect effect)
        {
            if (_weatherEffects.Remove(effect))
            {
                OnWorldTileChanged(this);
            }
        }

        public void AddWeatherEffect(WeatherEffect effect)
        {
            if (_weatherEffects.Contains(effect)) return;
            _weatherEffects.Add(effect);
            OnWorldTileChanged(this);
        }
        
        public event EventHandler<ChangedWorldTileEventArg> WorldTileChanged;

        protected virtual void OnWorldTileChanged(WorldTile e)
        {
            var args = new ChangedWorldTileEventArg(e);
            WorldTileChanged?.Invoke(this, args);
        }
        
    }
}