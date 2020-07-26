using System.Collections.Generic;
using Model.TileFeatures;
using Model.World;
using UnityEngine;
using UnityEngine.UI;

namespace Input
{
    public class ShapeClimateControl : MonoBehaviour
    {
        public Dropdown dropdown;
        public Button addButton;
        public Button removeButton;
        private WeatherEffect _selectedEffect;
        private List<WeatherEffect> _effects;
        private WorldMap _map;

        private void AddWeatherEffect()
        {
            if (_map.selectedTile != null) _map.selectedTile.AddWeatherEffect(_selectedEffect);
        }

        private void RemoveWeatherEffect()
        {
            if (_map.selectedTile != null) _map.selectedTile.RemoveWeatherEffect(_selectedEffect);
        }

        private void Start()
        {
            _map = GameObject.Find("World").GetComponent<WorldMap>();
            _effects = TileFeatures.LoadWeatherEffects();
            foreach (var weatherEffect in _effects) dropdown.options.Add(new Dropdown.OptionData(weatherEffect.name));
            dropdown.onValueChanged.AddListener(OnValueChange);
            dropdown.value = 1;
            addButton.onClick.AddListener(AddWeatherEffect);
            removeButton.onClick.AddListener(RemoveWeatherEffect);
        }

        private void OnValueChange(int value)
        {
            _selectedEffect = _effects.Find(effect => effect.name == dropdown.options[value].text);
        }
    }
}