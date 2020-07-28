using System.Linq;
using Meta.EventArgs;
using Model.Geo.Organization;
using UnityEngine;
using UnityEngine.UI;

namespace Input
{
    public class TileDetailsComponentControl : MonoBehaviour
    {
        public Text tileName;
        public Text tileBiome;
        public Text tileWeatherEffects;

        private WorldTile _selectedTile;

        public void OnWorldTileSelected(object sender, SelectedWorldTileEventArg e)
        {
            _selectedTile = e.SelectedTile;
            UpdateTexts();
        }

        public void OnWorldTileChanged(object sender, ChangedWorldTileEventArg e)
        {
            if (_selectedTile.Equals(e.ChangedWorldTile))
            {
                UpdateTexts();
            }
        }

        private void UpdateTexts()
        {
            tileName.text = _selectedTile.name;
            tileBiome.text = _selectedTile.BiomeToString();
            tileWeatherEffects.text = _selectedTile.WeatherEffectToString();
        }
    }
}