using System.Collections.Generic;
using Model.TileFeatures;
using UnityEngine;
using UnityEngine.UI;

namespace Input
{
    public class WeatherEffectDropdown : MonoBehaviour
    {

        private Dropdown _dropdown;
        private List<WeatherEffect> _effects;

        // Start is called before the first frame update
        void Start()
        {
            _dropdown = gameObject.GetComponent<Dropdown>();

            _effects = TileFeatures.LoadWeatherEffects();

            foreach (var weatherEffect in _effects)
            {
                _dropdown.options.Add(new Dropdown.OptionData(weatherEffect.name));
            }
            _dropdown.onValueChanged.AddListener(OnValueChange);
        }
        private void OnValueChange(int value)
        {
            var currentEffect = _effects.Find(effect => effect.name == _dropdown.options[value].text);
            Debug.Log(currentEffect.name);
        }
    }
}
