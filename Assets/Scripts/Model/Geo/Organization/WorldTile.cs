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
        public TerrainType terrain;
        public List<TerrainFeature> terrainFeatures;
        
        public Biome biome;
        
        public Climate Climate => worldArea != null ? worldArea.climate : ProfileSettings.DefaultClimate;
        public List<WeatherEffect> weatherEffects;

        public new SpriteRenderer renderer;
        public WorldMap worldMap;
        
        private void Start()
        {
            name = "Tile (" + position.x + ", " + position.y + ")";
            weatherEffects = new List<WeatherEffect>();
            terrainFeatures = new List<TerrainFeature>();
            type = TileType.Continental;
            terrain = TerrainType.Flat;
        }

        public void ChangeType(bool updateSprite)
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
            };
            WorldTileChanged();
            if (updateSprite)
                worldMap.UpdateSprite(this);
        }

        public void ChangeTerrain(TerrainType terrainType)
        {
            terrain = terrainType;
            WorldTileChanged();
        }

        public void UpdateSprite()
        {
            worldMap.UpdateSprite(this);
        }

        public void AddTerrainFeature(TerrainFeature terrainFeature)
        {
            terrainFeatures.Add(terrainFeature);
            WorldTileChanged();
        }

        public void RemoveWeatherEffect(WeatherEffect effect)
        {
            if (weatherEffects.Remove(effect))
            {
                WorldTileChanged();
            }
        }

        public void AddWeatherEffect(WeatherEffect effect)
        {
            if (weatherEffects.Contains(effect)) return;
            weatherEffects.Add(effect);
            WorldTileChanged();
        }

        public event EventHandler<UpdatedWorldTile> OnWorldTileChanged;

        private void WorldTileChanged()
        {
            OnWorldTileChanged?.Invoke(this, new UpdatedWorldTile(this));
        }

        public override string ToString()
        {
            return name;
        }
    }
}