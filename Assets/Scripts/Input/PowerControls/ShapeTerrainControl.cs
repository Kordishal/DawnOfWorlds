using System;
using System.Linq;
using Input.Dropdowns;
using Input.World;
using Meta;
using Meta.EventArgs;
using Model.Geo.Features.Terrain;
using Session;
using UnityEngine;
using UnityEngine.UI;

namespace Input.PowerControls
{
    public class ShapeTerrainControl : MonoBehaviour
    {
        private const int ChangeTypeCost = 5;
        private const float AreaChangeTypeDiscount = 0.8f;
        private const float RegionChangeTypeDiscount = 0.5f;

        private const int RaiseTerrainCost = 3;
        private const int RaiseDoubleCost = 5;
        private const float AreaRaiseTerrainDiscount = 0.9f;
        private const float RegionRaiseTerrainDiscount = 0.8f;

        public Button raiseSelectionButton;
        public Button submergeSelectionButton;

        public Button raiseHillsButton;
        public Button raiseMountainsButton;

        private TerrainFeatureDropdown _terrainFeatureDropdown;
        public Button createTerrainFeature;
        public InputField terrainFeatureName;
        public InputField terrainFeatureDescription;

        private SelectionModeControl _selectionModeControl;
        private SelectionDisplayControl _selectionDisplayControl;
        private SessionManager _sessionManager;
        private bool _nameValid;
        private bool _descriptionValid;

        private bool NameValid
        {
            get => _nameValid;
            set
            {
                if (value)
                {
                    if (DescriptionValid && SelectedActionValid)
                        createTerrainFeature.interactable = true;
                }
                else
                {
                    createTerrainFeature.interactable = false;
                }

                _nameValid = value;
            }
        }

        private bool DescriptionValid
        {
            get => _descriptionValid;
            set
            {
                if (value)
                {
                    if (NameValid && SelectedActionValid)
                        createTerrainFeature.interactable = true;
                }
                else
                {
                    createTerrainFeature.interactable = false;
                }

                _descriptionValid = value;
            }
        }

        private bool SelectedActionValid => _terrainFeatureDropdown.IsActive();

        private void Start()
        {
            _selectionDisplayControl = GameObject.FindWithTag(Tags.MainCamera).GetComponent<SelectionDisplayControl>();
            _selectionModeControl = GameObject.FindWithTag(Tags.PowerButtonPanel).GetComponent<SelectionModeControl>();
            _sessionManager = GameObject.FindWithTag(Tags.SessionManager).GetComponent<SessionManager>();
            
            _selectionDisplayControl.TileSelectionChange += OnTileSelectionChange;
            _selectionDisplayControl.AreaSelectionChange += OnAreaSelectionChange;
            _selectionDisplayControl.RegionSelectionChange += OnRegionSelectionChange;

            submergeSelectionButton.onClick.AddListener(SubmitTypeChange);
            raiseSelectionButton.onClick.AddListener(SubmitTypeChange);

            raiseHillsButton.onClick.AddListener(RaiseHills);
            raiseMountainsButton.onClick.AddListener(RaiseMountains);

            
            _terrainFeatureDropdown = GetComponentInChildren<TerrainFeatureDropdown>();
            if (_selectionDisplayControl.selectedTile != null)
                _terrainFeatureDropdown.UpdateElements(_selectionDisplayControl.selectedTile);
            createTerrainFeature.onClick.AddListener(CreateTerrainFeature);

            terrainFeatureName.onEndEdit.AddListener(delegate(string text) { NameValid = text != ""; });
            terrainFeatureDescription.onEndEdit.AddListener(delegate(string text) { DescriptionValid = text != ""; });
            createTerrainFeature.interactable = false;
        }

        private void CreateTerrainFeature()
        {
            var tile = _selectionDisplayControl.selectedTile;
            if (tile == null) return;
            var feature = _terrainFeatureDropdown.CurrentValue();
            if (!_sessionManager.SpendPoints(feature.cost)) return;
            var newFeature = new TerrainFeature
            {
                name = terrainFeatureName.text, 
                featureType = feature.objectName,
                description = terrainFeatureDescription.text
            };
            tile.AddTerrainFeature(newFeature);
        }

        private void RaiseHills()
        {
            switch (_selectionModeControl.CurrentMode)
            {
                case SelectionMode.Tile:
                    var tile = _selectionDisplayControl.selectedTile;
                    if (tile != null)
                    {
                        switch (tile.terrain)
                        {
                            case TerrainType.Flat:
                                if (_sessionManager.SpendPoints(RaiseTerrainCost))
                                {
                                    tile.ChangeTerrain(TerrainType.Hilly);
                                    raiseHillsButton.interactable = false;
                                }

                                break;
                            case TerrainType.Hilly:
                            case TerrainType.Mountainous:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    break;
                case SelectionMode.Area:
                    var area = _selectionDisplayControl.SelectedArea;
                    if (area != null)
                    {
                        var flatTiles = area.tiles.Where(t => t.terrain == TerrainType.Flat).ToList();
                        var cost = flatTiles.Count * RaiseTerrainCost * AreaRaiseTerrainDiscount;
                        if (_sessionManager.SpendPoints(cost))
                        {
                            flatTiles.ForEach(t => t.ChangeTerrain(TerrainType.Hilly));
                            raiseHillsButton.interactable = false;
                        }
                    }

                    break;
                case SelectionMode.Region:
                    var region = _selectionDisplayControl.SelectedRegion;
                    if (region != null)
                    {
                        var flatTiles = region.areas.SelectMany(a => a.tiles).Where(t => t.terrain == TerrainType.Flat)
                            .ToList();
                        var cost = flatTiles.Count * RaiseTerrainCost * RegionRaiseTerrainDiscount;
                        if (_sessionManager.SpendPoints(cost))
                        {
                            flatTiles.ForEach(t => t.ChangeTerrain(TerrainType.Hilly));
                            raiseHillsButton.interactable = false;
                        }
                    }

                    break;
                case SelectionMode.AreaCreation:
                    break;
                case SelectionMode.RegionCreation:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RaiseMountains()
        {
            switch (_selectionModeControl.CurrentMode)
            {
                case SelectionMode.Tile:
                    var tile = _selectionDisplayControl.selectedTile;
                    if (tile != null)
                    {
                        switch (tile.terrain)
                        {
                            case TerrainType.Flat:
                                if (_sessionManager.SpendPoints(RaiseDoubleCost))
                                {
                                    tile.ChangeTerrain(TerrainType.Mountainous);
                                    raiseMountainsButton.interactable = false;
                                }

                                break;
                            case TerrainType.Hilly:
                                if (_sessionManager.SpendPoints(RaiseTerrainCost))
                                {
                                    tile.ChangeTerrain(TerrainType.Mountainous);
                                    raiseMountainsButton.interactable = false;
                                }

                                break;
                            case TerrainType.Mountainous:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                    break;
                case SelectionMode.Area:
                    var area = _selectionDisplayControl.SelectedArea;
                    if (area != null)
                    {
                        var flatTiles = area.tiles.Where(t => t.terrain == TerrainType.Flat).ToList();
                        var hillyTiles = area.tiles.Where(t => t.terrain == TerrainType.Hilly).ToList();
                        var cost = ((flatTiles.Count * RaiseDoubleCost) + (hillyTiles.Count * RaiseTerrainCost)) *
                                   AreaRaiseTerrainDiscount;
                        if (_sessionManager.SpendPoints(cost))
                        {
                            flatTiles.ForEach(t => t.ChangeTerrain(TerrainType.Mountainous));
                            hillyTiles.ForEach(t => t.ChangeTerrain(TerrainType.Mountainous));
                            raiseMountainsButton.interactable = false;
                        }
                    }

                    break;
                case SelectionMode.Region:
                    var region = _selectionDisplayControl.SelectedRegion;
                    if (region != null)
                    {
                        var flatTiles = region.areas.SelectMany(a => a.tiles).Where(t => t.terrain == TerrainType.Flat)
                            .ToList();
                        var hillyTiles = region.areas.SelectMany(a => a.tiles)
                            .Where(t => t.terrain == TerrainType.Hilly).ToList();
                        var cost = ((flatTiles.Count * RaiseDoubleCost) + (hillyTiles.Count * RaiseTerrainCost)) *
                                   RegionRaiseTerrainDiscount;
                        if (_sessionManager.SpendPoints(cost))
                        {
                            flatTiles.ForEach(t => t.ChangeTerrain(TerrainType.Mountainous));
                            hillyTiles.ForEach(t => t.ChangeTerrain(TerrainType.Mountainous));
                            raiseMountainsButton.interactable = false;
                        }
                    }

                    break;
                case SelectionMode.AreaCreation:
                    break;
                case SelectionMode.RegionCreation:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SubmitTypeChange()
        {
            switch (_selectionModeControl.CurrentMode)
            {
                case SelectionMode.Tile:
                    if (_selectionDisplayControl.selectedTile != null)
                        if (_sessionManager.SpendPoints(ChangeTypeCost))
                            _selectionDisplayControl.selectedTile.ChangeType(true);
                    break;
                case SelectionMode.Area:
                    var area = _selectionDisplayControl.SelectedArea;
                    if (area != null)
                    {
                        var cost = area.tiles.Count * ChangeTypeCost * AreaChangeTypeDiscount;
                        if (_sessionManager.SpendPoints(cost))
                        {
                            area.tiles.ForEach(t => t.ChangeType(false));
                            area.tiles.ForEach(t => t.UpdateSprite());
                        }
                    }

                    break;
                case SelectionMode.Region:
                    var region = _selectionDisplayControl.SelectedRegion;
                    if (region != null)
                    {
                        var tiles = region.areas.SelectMany(a => a.tiles).ToList();
                        var cost = tiles.Count() * ChangeTypeCost * RegionChangeTypeDiscount;
                        if (_sessionManager.SpendPoints(cost))
                        {
                            tiles.ForEach(t => t.ChangeType(false));
                            tiles.ForEach(t => t.UpdateSprite());
                        }
                    }

                    break;
                case SelectionMode.AreaCreation:
                    break;
                case SelectionMode.RegionCreation:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnTileSelectionChange(object sender, SelectedWorldTile selected)
        {
            var tile = selected.Tile;
            _terrainFeatureDropdown.UpdateElements(tile);
            if (tile == null)
            {
                DeactivateAll();
                return;
            }
            
            switch (tile.type)
            {
                case TileType.Continental:
                    raiseSelectionButton.interactable = false;
                    submergeSelectionButton.interactable = true;
                    break;
                case TileType.Oceanic:
                    raiseSelectionButton.interactable = true;
                    submergeSelectionButton.interactable = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (tile.terrain)
            {
                case TerrainType.Flat:
                    raiseHillsButton.interactable = true;
                    raiseMountainsButton.interactable = true;
                    break;
                case TerrainType.Hilly:
                    raiseHillsButton.interactable = false;
                    raiseMountainsButton.interactable = true;
                    break;
                case TerrainType.Mountainous:
                    raiseHillsButton.interactable = false;
                    raiseMountainsButton.interactable = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnAreaSelectionChange(object sender, SelectedWorldArea area)
        {
            if (area.Area == null)
            {
                DeactivateAll();
                return;
            }

            if (area.Area.IsContinental())
            {
                raiseSelectionButton.interactable = false;
                submergeSelectionButton.interactable = true;
            }
            else if (area.Area.IsOceanic())
            {
                raiseSelectionButton.interactable = true;
                submergeSelectionButton.interactable = false;
            }
            else
            {
                raiseSelectionButton.interactable = true;
                submergeSelectionButton.interactable = true;
            }

            if (area.Area.tiles.Any(t => t.terrain == TerrainType.Flat))
            {
                raiseHillsButton.interactable = true;
                raiseMountainsButton.interactable = true;
            }
            else if (area.Area.tiles.Any(t => t.terrain == TerrainType.Hilly))
            {
                raiseHillsButton.interactable = false;
                raiseMountainsButton.interactable = true;
            }
            else
            {
                raiseHillsButton.interactable = false;
                raiseMountainsButton.interactable = false;
            }
        }

        private void OnRegionSelectionChange(object sender, SelectedWorldRegion region)
        {
            if (region.Region == null)
            {
                DeactivateAll();
                return;
            }

            if (region.Region.areas.Any(a => a.IsCoastal()))
            {
                raiseSelectionButton.interactable = true;
                submergeSelectionButton.interactable = true;
            }
            else if (region.Region.areas.Any(a => a.IsContinental()))
            {
                raiseSelectionButton.interactable = false;
                submergeSelectionButton.interactable = true;
            }
            else
            {
                raiseSelectionButton.interactable = true;
                submergeSelectionButton.interactable = false;
            }

            if (region.Region.areas.SelectMany(a => a.tiles).Any(t => t.terrain == TerrainType.Flat))
            {
                raiseHillsButton.interactable = true;
                raiseMountainsButton.interactable = true;
            }
            else if (region.Region.areas.SelectMany(a => a.tiles).Any(t => t.terrain == TerrainType.Hilly))
            {
                raiseHillsButton.interactable = false;
                raiseMountainsButton.interactable = true;
            }
            else
            {
                raiseHillsButton.interactable = false;
                raiseMountainsButton.interactable = false;
            }
        }

        private void DeactivateAll()
        {
            raiseSelectionButton.interactable = false;
            submergeSelectionButton.interactable = false;
            raiseHillsButton.interactable = false;
            raiseMountainsButton.interactable = false;
            createTerrainFeature.interactable = false;
        }
    }
}