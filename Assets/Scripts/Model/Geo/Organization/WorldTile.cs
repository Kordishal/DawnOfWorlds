using System;
using System.Collections.Generic;
using Meta.EventArgs;
using Model.Geo.Features.Climate;
using Model.Geo.Features.Terrain;
using Model.Geo.Support;
using Player;
using UnityEngine;

namespace Model.Geo.Organization
{
    [Serializable]
    public sealed class WorldTile : MonoBehaviour
    {
        public Position position;
        
        public WorldArea worldArea;

        public TileType type;
        
        public Biome biome;
        public List<WeatherEffect> weatherEffects;

        public new SpriteRenderer renderer;
        public WorldMap worldMap;
        
        public Climate Climate => worldArea != null ? worldArea.climate : ProfileSettings.DefaultClimate;

        private void Start()
        {
            name = "Tile (" + position.x + ", " + position.y + ")";
            weatherEffects = new List<WeatherEffect>();
            biome = new Biome("Barren", "There is no life here.");
            type = TileType.Continental;
        }

        public void ChangeType()
        {
            switch (type)
            {
                case TileType.Continental:
                    type = TileType.Oceanic;
                    break;
                case TileType.Oceanic:
                    type = TileType.Continental;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            worldMap.UpdateSprite(this);
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