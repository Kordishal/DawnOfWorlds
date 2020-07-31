using System;
using System.Linq;
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

        public Button raiseSelectionButton;
        private Text _raiseSelectionText;
        public Button submergeSelectionButton;
        private Text _submergeSelectionText;

        private SelectionModeControl _selectionModeControl;
        private SelectionDisplayControl _selectionDisplayControl;

        private SessionManager _sessionManager;

        private void Start()
        {
            _selectionDisplayControl = GameObject.FindWithTag(Tags.MainCamera).GetComponent<SelectionDisplayControl>();
            _selectionModeControl = GameObject.FindWithTag(Tags.PowerButtonPanel).GetComponent<SelectionModeControl>();
            _sessionManager = GameObject.FindWithTag(Tags.SessionManager).GetComponent<SessionManager>();

            _raiseSelectionText = raiseSelectionButton.GetComponentInChildren<Text>();
            _submergeSelectionText = submergeSelectionButton.GetComponentInChildren<Text>();

            _selectionModeControl.OnChangedSelectionMode += delegate(object sender, ChangeSelectionModeEventArgs args)
            {
                switch (args.NewMode)
                {
                    case SelectionMode.Tile:
                        _raiseSelectionText.text = "Raise Selected Tile";
                        _submergeSelectionText.text = "Submerge Selected Tile";
                        break;
                    case SelectionMode.Area:
                        _raiseSelectionText.text = "Raise Selected Area";
                        _submergeSelectionText.text = "Submerge Selected Area";
                        break;
                    case SelectionMode.Region:
                        _raiseSelectionText.text = "Raise Selected Region";
                        _submergeSelectionText.text = "Submerge Selected Region";
                        break;
                    case SelectionMode.AreaCreation:
                        break;
                    case SelectionMode.RegionCreation:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };

            _selectionDisplayControl.TileSelectionChange += OnTileSelectionChange;
            _selectionDisplayControl.AreaSelectionChange += OnAreaSelectionChange;
            _selectionDisplayControl.RegionSelectionChange += OnRegionSelectionChange;

            submergeSelectionButton.onClick.AddListener(delegate
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
                            var cost = area.tiles.Count * ChangeTypeCost *
                                       AreaChangeTypeDiscount;
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
                            var cost = tiles.Count() * ChangeTypeCost *
                                       RegionChangeTypeDiscount;
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
            });
        }

        private void OnTileSelectionChange(object sender, SelectedWorldTile tile)
        {
            if (tile.Tile == null)
            {
                raiseSelectionButton.interactable = false;
                submergeSelectionButton.interactable = false;
                return;
            }

            switch (tile.Tile.type)
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
        }

        private void OnAreaSelectionChange(object sender, SelectedWorldArea area)
        {
            if (area.Area == null)
            {
                raiseSelectionButton.interactable = false;
                submergeSelectionButton.interactable = false;
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
        }

        private void OnRegionSelectionChange(object sender, SelectedWorldRegion region)
        {
            if (region.Region == null)
            {
                raiseSelectionButton.interactable = false;
                submergeSelectionButton.interactable = false;
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
        }
    }
}