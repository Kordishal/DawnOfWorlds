using System.Collections.Generic;
using Meta;
using Model.TileFeatures;
using Model.World;
using Session;
using UnityEngine;
using UnityEngine.UI;

namespace Input.PowerControls
{
    public class ShapeClimateControl : MonoBehaviour
    {
        private const int Cost = 2;
        
        public Dropdown dropdown;
        public Button addButton;
        public Button removeButton;
        private WeatherEffect _selectedEffect;
        private List<WeatherEffect> _effects;
        private WorldMap _map;

        private SessionManager _sessionManager;

        private void AddWeatherEffect()
        {
            if (_map.selectedTile == null) return;
            if (_sessionManager.SpendPoints(Cost))
            {
                _map.selectedTile.AddWeatherEffect(_selectedEffect);
            }
        }

        private void RemoveWeatherEffect()
        {
            if (_map.selectedTile == null) return;
            if (!_sessionManager.SpendPoints(Cost)) return;
            _map.selectedTile.RemoveWeatherEffect(_selectedEffect);
        }

        private void Start()
        {
            _map = GameObject.FindWithTag(Tags.World).GetComponent<WorldMap>();
            _effects = TileFeatures.LoadWeatherEffects();
            foreach (var weatherEffect in _effects) dropdown.options.Add(new Dropdown.OptionData(weatherEffect.name));
            dropdown.onValueChanged.AddListener(OnValueChange);
            dropdown.value = 1;
            addButton.onClick.AddListener(AddWeatherEffect);
            removeButton.onClick.AddListener(RemoveWeatherEffect);
            _sessionManager = GameObject.FindWithTag(Tags.SessionManager).GetComponent<SessionManager>();
        }

        private void OnValueChange(int value)
        {
            _selectedEffect = _effects.Find(effect => effect.name == dropdown.options[value].text);
        }
    }
}