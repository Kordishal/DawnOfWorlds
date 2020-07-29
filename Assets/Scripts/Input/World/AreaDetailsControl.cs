using Meta.EventArgs;
using Model.Geo.Organization;
using UnityEngine;
using UnityEngine.UI;

namespace Input.World
{
    public class AreaDetailsControl : MonoBehaviour
    {
        public Text areaName;
        public Text tiles;

        private WorldArea _selectedArea;

        public void OnAreaSelectionChange(object sender, SelectedWorldArea e)
        {
            if (e.Area == null) return;
            _selectedArea = e.Area;
            UpdateTexts(e.Area);
        }

        private void UpdateTexts(WorldArea area)
        {
            areaName.text = area.name;
            tiles.text = string.Join(", ", area.tiles);
        }
        
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}