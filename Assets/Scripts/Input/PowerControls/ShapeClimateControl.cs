using System.Collections.Generic;
using Input.World;
using Meta;
using Model.Geo.Features;
using Model.Geo.Features.Climate;
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
        private SelectionDisplayControl _selectionDisplayControl;

        private SessionManager _sessionManager;

        private void AddWeatherEffect()
        {
            if (_selectionDisplayControl.selectedTile == null) return;
            if (_sessionManager.SpendPoints(Cost))
            {
                _selectionDisplayControl.selectedTile.AddWeatherEffect(_selectedEffect);
            }
        }

        private void RemoveWeatherEffect()
        {
            if (_selectionDisplayControl.selectedTile == null) return;
            if (!_sessionManager.SpendPoints(Cost)) return;
            _selectionDisplayControl.selectedTile.RemoveWeatherEffect(_selectedEffect);
        }

        private void Start()
        {
            _selectionDisplayControl = GameObject.FindWithTag(Tags.MainCamera).GetComponent<SelectionDisplayControl>();
            _effects = FeatureManager.LoadWeatherEffects();
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