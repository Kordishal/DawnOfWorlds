using System;
using System.Collections.Generic;
using Meta.EventArgs;
using Model.Geo.Features;
using Model.Geo.Features.Climate;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Dropdowns
{
    public class WeatherEffectDropdown : MonoBehaviour
    {
        private Dropdown _dropdown;

        private List<WeatherEffect> _effects;

        private void Start()
        {
            if (_dropdown == null)
                _dropdown = GetComponent<Dropdown>();

            _effects = FeatureManager.LoadWeatherEffects();
            foreach (var weatherEffect in _effects) 
                _dropdown.options.Add(new Dropdown.OptionData(weatherEffect.name));
            _dropdown.onValueChanged.AddListener(OnValueChanged);
            _dropdown.value = 1;
        }

        public void SetValue(WeatherEffect weatherEffect)
        {
            if (_dropdown == null)
                _dropdown = GetComponent<Dropdown>();
            _dropdown.value = _effects.FindIndex(w => w.name == weatherEffect.name);
        }

        public WeatherEffect CurrentValue()
        {
            if (_dropdown == null)
                _dropdown = GetComponent<Dropdown>();
            return _effects[_dropdown.value];
        }

        private void OnValueChanged(int value)
        {
            WeatherEffectChanged(_effects[value]);
        }

        public event EventHandler<WeatherEffectEventArgs> OnWeatherEffectChanged;

        private void WeatherEffectChanged(WeatherEffect weatherEffect)
        {
            OnWeatherEffectChanged?.Invoke(this, new WeatherEffectEventArgs(weatherEffect));
        }
    }
}