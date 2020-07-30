using System;
using Meta.EventArgs;
using Model.Geo.Features.Climate;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Menu
{
    public class ClimateDropdown : MonoBehaviour
    {
        private Dropdown _dropdown;

        private void Start()
        {
            if (_dropdown == null)
                _dropdown = GetComponent<Dropdown>();
            foreach (var climate in Enum.GetNames(typeof(Climate)))
            {
                _dropdown.options.Add(new Dropdown.OptionData(climate));
            }

            _dropdown.value = (int) Climate.Temperate;
            _dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        public void SetValue(Climate climate)
        {
            if (_dropdown == null)
                _dropdown = GetComponent<Dropdown>();
            _dropdown.value = (int) climate;
        }

        private void OnValueChanged(int value)
        {
            ClimateChanged((Climate) value);
        }

        public event EventHandler<ClimateEventArgs> OnClimateChanged;

        private void ClimateChanged(Climate climate)
        {
            OnClimateChanged?.Invoke(this, new ClimateEventArgs(climate));
        }
    }
}