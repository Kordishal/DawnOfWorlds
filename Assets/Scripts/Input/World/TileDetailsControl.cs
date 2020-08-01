using JetBrains.Annotations;
using Meta.EventArgs;
using Model.Geo.Organization;
using UnityEngine;
using UnityEngine.UI;

namespace Input.World
{
    public class TileDetailsControl : MonoBehaviour
    {
        public Text tileName;
        public Text tileType;
        public Text tileTerrainType;
        public Text tileBiome;
        public Text tileTerrainFeature;
        public Text tileWeatherEffects;

        private WorldTile _currentDisplayedTile;

        public void OnTileSelectionChange(object sender, SelectedWorldTile e)
        {
            if (e.Tile == null) return;
            if (e.Tile == _currentDisplayedTile) return;
            _currentDisplayedTile = e.Tile;
            UpdateTexts(_currentDisplayedTile);
        }

        public void OnTileDataUpdate(object sender, UpdatedWorldTile e)
        {
            if (e.Tile == null) return;
            if (e.Tile != _currentDisplayedTile) return;
            UpdateTexts(_currentDisplayedTile);
        }

        private void UpdateTexts(WorldTile worldTile)
        {
            tileName.text = worldTile.name;
            tileType.text = worldTile.type.ToString();
            tileTerrainType.text = worldTile.terrain.ToString();
            tileBiome.text = worldTile.biome.ToString();
            tileTerrainFeature.text = string.Join(", ", worldTile.terrainFeatures);
            tileWeatherEffects.text = string.Join(", ", worldTile.weatherEffects);
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}