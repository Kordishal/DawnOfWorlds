using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Meta;
using Meta.EventArgs;
using Model.Geo.Organization;
using Model.Geo.Support;
using UnityEngine;

namespace Input.World
{
    public class SelectionDisplayControl : MonoBehaviour
    {
        public WorldMap map;
        public GameObject selectionTilePrefab;

        public SelectionModeControl selectionModeControl;

        [CanBeNull] public WorldTile selectedTile;

        [CanBeNull] public WorldArea SelectedArea => selectedTile == null ? null : selectedTile.worldArea;

        [CanBeNull]
        public WorldRegion SelectedRegion => selectedTile == null ? null :
            selectedTile.worldArea == null ? null : selectedTile.worldArea.worldRegion;

        private Dictionary<Position, SelectionDisplayTile> _selectionTiles;

        private WorldTile _selectionTile;
        private AreaCreationControl _areaCreationControl;
        private RegionCreationControl _regionCreationControl;

        public void UpdateSelection([CanBeNull] WorldTile newSelection)
        {
            if (newSelection == selectedTile) return;
            ChangeSelection(selectionModeControl.CurrentMode, selectedTile, false);
            ChangeSelection(selectionModeControl.CurrentMode, newSelection, true);
            selectedTile = newSelection;
        }

        public void ChangeSelectionMode(SelectionMode oldMode, SelectionMode newMode)
        {
            ChangeSelection(oldMode, selectedTile, false);
            ChangeSelection(newMode, selectedTile, true);
        }

        public void ClearSelection(IEnumerable<WorldTile> tiles)
        {
            foreach (var worldTile in tiles)
            {
                _selectionTiles[worldTile.position].SetActive(false);
            }

            selectedTile = null;
        }


        private void Start()
        {
            _selectionTiles = new Dictionary<Position, SelectionDisplayTile>();
            for (var i = 0; i < map.width; i++)
            {
                for (var j = 0; j < map.height; j++)
                {
                    var tile = Instantiate(selectionTilePrefab, map.gameObject.transform);
                    tile.name = "Selection Tile (" + i + ", " + j + ")";
                    var selectionTile = tile.GetComponent<SelectionDisplayTile>();
                    selectionTile.position = new Position(i, j);
                    selectionTile.transform.position = new Vector3(i * 7, j * 7, 10);
                    _selectionTiles[selectionTile.position] = selectionTile;
                    selectionTile.SetActive(false);
                }
            }
        }

        private void ChangeSelection(SelectionMode mode, [CanBeNull] WorldTile worldTile, bool select)
        {
            switch (mode)
            {
                case SelectionMode.Tile:
                    if (worldTile != null)
                    {
                        _selectionTiles[worldTile.position].SetActive(select);
                    }

                    OnTileSelectionChanged(worldTile);
                    break;
                case SelectionMode.Area:
                    if (worldTile != null && worldTile.worldArea != null)
                    {
                        foreach (var tile in worldTile.worldArea.tiles)
                        {
                            _selectionTiles[tile.position].SetActive(select);
                        }

                        OnAreaSelectionChanged(worldTile.worldArea);
                    }
                    else
                        OnAreaSelectionChanged(null);

                    break;
                case SelectionMode.Region:
                    if (worldTile != null && worldTile.worldArea != null && worldTile.worldArea.worldRegion != null)
                    {
                        foreach (var tile in worldTile.worldArea.worldRegion.areas.SelectMany(area => area.tiles))
                        {
                            _selectionTiles[tile.position].SetActive(select);
                        }

                        OnRegionSelectionChanged(worldTile.worldArea.worldRegion);
                    }
                    else
                        OnRegionSelectionChanged(null);

                    break;
                case SelectionMode.AreaCreation:
                    if (!select) return;
                    if (worldTile == null) return;
                    if (_areaCreationControl == null)
                    {
                        _areaCreationControl = GameObject.FindWithTag(Tags.AreaCreationComponent)
                            .GetComponent<AreaCreationControl>();
                    }

                    if (_selectionTiles[worldTile.position].IsActive)
                    {
                        if (_areaCreationControl.RemoveTileSelection(worldTile))
                        {
                            _selectionTiles[worldTile.position].SetActive(false);
                        }
                    }
                    else
                    {
                        if (_areaCreationControl.AddTileSelection(worldTile))
                        {
                            _selectionTiles[worldTile.position].SetActive(true);
                        }
                    }

                    break;
                case SelectionMode.RegionCreation:
                    if (!select) return;
                    if (worldTile == null) return;
                    if (worldTile.worldArea == null) return;
                    if (_regionCreationControl == null)
                    {
                        _regionCreationControl = GameObject.FindWithTag(Tags.RegionCreationComponent)
                            .GetComponent<RegionCreationControl>();
                    }

                    if (_selectionTiles[worldTile.position].IsActive)
                    {
                        if (_regionCreationControl.RemoveArea(worldTile.worldArea))
                        {
                            ClearSelection(worldTile.worldArea.tiles);
                        }
                    }
                    else
                    {
                        if (_regionCreationControl.AddArea(worldTile.worldArea))
                        {
                            foreach (var tile in worldTile.worldArea.tiles)
                            {
                                _selectionTiles[tile.position].SetActive(true);
                            }
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectionModeControl.CurrentMode),
                        selectionModeControl.CurrentMode,
                        null);
            }
        }

        public event EventHandler<SelectedWorldTile> TileSelectionChange;

        private void OnTileSelectionChanged(WorldTile e)
        {
            TileSelectionChange?.Invoke(this, new SelectedWorldTile(e));
        }

        public event EventHandler<SelectedWorldArea> AreaSelectionChange;

        private void OnAreaSelectionChanged(WorldArea e)
        {
            AreaSelectionChange?.Invoke(this, new SelectedWorldArea(e));
        }

        public event EventHandler<SelectedWorldRegion> RegionSelectionChange;

        private void OnRegionSelectionChanged(WorldRegion e)
        {
            RegionSelectionChange?.Invoke(this, new SelectedWorldRegion(e));
        }
    }
}