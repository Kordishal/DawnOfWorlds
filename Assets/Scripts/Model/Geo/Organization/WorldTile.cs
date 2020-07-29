using System;
using System.Collections.Generic;
using Meta.EventArgs;
using Model.Geo.Features.Climate;
using Model.Geo.Features.Terrain;
using Model.Geo.Support;
using UnityEngine;

namespace Model.Geo.Organization
{
    public sealed class WorldTile : MonoBehaviour
    {
        public Position position;
        public WorldMap worldMap;
        public WorldArea worldArea;
        public List<WeatherEffect> weatherEffects;
        public Biome biome;

        private void Start()
        {
            name = "Tile (" + position.x + ", " + position.y + ")";
            weatherEffects = new List<WeatherEffect>();
            biome = new Biome("Barren", "There is no life here.");
        }

        public void RemoveWeatherEffect(WeatherEffect effect)
        {
            if (weatherEffects.Remove(effect))
            {
                OnWorldTileChanged(this);
            }
        }

        public void AddWeatherEffect(WeatherEffect effect)
        {
            if (weatherEffects.Contains(effect)) return;
            weatherEffects.Add(effect);
            OnWorldTileChanged(this);
        }

        public event EventHandler<UpdatedWorldTile> WorldTileChanged;

        private void OnWorldTileChanged(WorldTile e)
        {
            var args = new UpdatedWorldTile(e);
            WorldTileChanged?.Invoke(this, args);
        }

        public override string ToString()
        {
            return name;
        }
    }
}