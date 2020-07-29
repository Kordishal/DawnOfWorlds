using Meta.EventArgs;
using Model.Geo.Organization;
using UnityEngine;
using UnityEngine.UI;

namespace Input.World
{
    public class RegionDetailsControl : MonoBehaviour
    {
        public Text regionName;
        public Text tiles;

        private WorldRegion _selectedRegion;

        public void OnRegionSelectionChange(object sender, SelectedWorldRegion e)
        {
            if (e.Region == null) return;
            _selectedRegion = e.Region;
            UpdateTexts(_selectedRegion);
        }

        private void UpdateTexts(WorldRegion region)
        {
            regionName.text = region.name;
            tiles.text = string.Join(", ", region.areas);
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}