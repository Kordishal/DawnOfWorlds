using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Meta.EventArgs;
using Model.Geo.Features;
using Model.Geo.Organization;
using PowerSystems.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Dropdowns
{
    public class TerrainFeatureDropdown : MonoBehaviour
    {
        private Dropdown _dropdown;

        private Dictionary<int, TerrainFeatureAction> _actionMap;
        private List<TerrainFeatureAction> _terrainFeatures;

        private void Start()
        {
            _dropdown = GetComponent<Dropdown>();
            _actionMap = new Dictionary<int, TerrainFeatureAction>();
            var manager = FeatureManager.GetInstance();
            _terrainFeatures = manager.LoadTerrainFeatures().ToList();
            _dropdown.interactable = false;
            _dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        public bool IsActive() => _dropdown.interactable;

        public void UpdateElements([CanBeNull] WorldTile tile)
        {     
            if (_dropdown == null) return;
            _dropdown.options.Clear();
            _actionMap.Clear();
            if (tile == null)
            {
                _dropdown.interactable = false;
                return;
            }
            var tempList = _terrainFeatures
                .Where(t => t.allowedTileType == tile.type)
                .Where(t => t.allowedTerrainTypes.Contains(tile.terrain)).ToList();
            if (!tempList.Any())
            {
                _dropdown.interactable = false;
                return;
            }
            _dropdown.interactable = true;
            var count = 1;
            foreach (var action in tempList)
            {
                _dropdown.options.Add(new Dropdown.OptionData(action.creationActionName));
                _actionMap[count] = action;
                count += 1;
            }
        }


        public TerrainFeatureAction CurrentValue() => _actionMap[_dropdown.value];

        private void OnValueChanged(int value)
        {
            TerrainFeatureChanged(_terrainFeatures[value]);
        }

        public event EventHandler<TerrainFeaturesEventArgs> OnTerrainFeatureChanged;

        private void TerrainFeatureChanged(TerrainFeatureAction terrainFeatureAction)
        {
            OnTerrainFeatureChanged?.Invoke(this, new TerrainFeaturesEventArgs(terrainFeatureAction));
        }
    }
}