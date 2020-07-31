using System;
using Input.Dropdowns;
using Input.World;
using Meta;
using Meta.EventArgs;
using Model.Geo.Features.Climate;
using Session;
using UnityEngine;
using UnityEngine.UI;

namespace Input.PowerControls
{
    public class ShapeClimateControl : MonoBehaviour
    {
        private const int Cost = 2;
        private const int ChangeClimateCost = 5;
        private const float ChangeClimateRegionDiscount = 0.5f;

        private WeatherEffectDropdown _weatherEffectDropdown;
        public Button addButton;
        public Button removeButton;


        private ClimateDropdown _climateDropdown;
        public Button submitClimateChange;


        private WeatherEffect _selectedEffect;

        private SelectionModeControl _selectionModeControl;
        private SelectionDisplayControl _selectionDisplayControl;

        private SessionManager _sessionManager;

        private void Start()
        {
            _selectionDisplayControl = GameObject.FindWithTag(Tags.MainCamera).GetComponent<SelectionDisplayControl>();
            _selectionModeControl = GameObject.FindWithTag(Tags.PowerButtonPanel).GetComponent<SelectionModeControl>();
            _sessionManager = GameObject.FindWithTag(Tags.SessionManager).GetComponent<SessionManager>();

            _weatherEffectDropdown = GetComponentInChildren<WeatherEffectDropdown>();
            _weatherEffectDropdown.OnWeatherEffectChanged += delegate(object sender, WeatherEffectEventArgs args)
            {
                _selectedEffect = args.WeatherEffect;
            };

            addButton.onClick.AddListener(AddWeatherEffect);
            removeButton.onClick.AddListener(RemoveWeatherEffect);


            _selectionModeControl.OnChangedSelectionMode += delegate(object sender, ChangeSelectionModeEventArgs args)
            {
                switch (args.NewMode)
                {
                    case SelectionMode.Area:
                    case SelectionMode.Region:
                        submitClimateChange.interactable = false;
                        break;
                    case SelectionMode.Tile:
                    case SelectionMode.AreaCreation:
                    case SelectionMode.RegionCreation:
                        submitClimateChange.interactable = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };

            _climateDropdown = GetComponentInChildren<ClimateDropdown>();
            _climateDropdown.SetValue(Climate.Temperate);
            submitClimateChange.onClick.AddListener(SubmitClimateChange);
            submitClimateChange.interactable = false;

            _climateDropdown.OnClimateChanged += OnClimateChanged;
        }

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

        private void SubmitClimateChange()
        {
            switch (_selectionModeControl.CurrentMode)
            {
                case SelectionMode.Area:
                    Debug.Assert(_selectionDisplayControl.SelectedArea != null, "Area should be selected!");
                    if (_sessionManager.SpendPoints(ChangeClimateCost))
                    {
                        _selectionDisplayControl.SelectedArea.climate = _climateDropdown.CurrentValue();
                        submitClimateChange.interactable = false;
                    }

                    break;
                case SelectionMode.Region:
                    Debug.Assert(_selectionDisplayControl.SelectedArea != null, "Area should be selected!");
                    // TODO: Should this be possible if not all areas are of the same distance to the new climate?
                    if (_selectionDisplayControl.SelectedRegion != null)
                    {
                        var areas = _selectionDisplayControl.SelectedRegion.areas;
                        if (_sessionManager.SpendPoints(
                            (int) Math.Floor((areas.Count * ChangeClimateCost) * ChangeClimateRegionDiscount)))
                        {
                            foreach (var worldArea in areas)
                            {
                                worldArea.climate = _climateDropdown.CurrentValue();
                            }
                        }

                        submitClimateChange.interactable = false;
                    }

                    break;
                case SelectionMode.Tile:
                case SelectionMode.AreaCreation:
                case SelectionMode.RegionCreation:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnClimateChanged(object sender, ClimateEventArgs args)
        {
            var area = _selectionDisplayControl.SelectedArea;
            if (area == null) return;
            var currentClimate = (int) area.climate;
            var selectedClimate = (int) args.Climate;
            if (currentClimate + 1 == selectedClimate || currentClimate - 1 == selectedClimate)
                submitClimateChange.interactable = true;
            else
                submitClimateChange.interactable = false;
        }
    }
}