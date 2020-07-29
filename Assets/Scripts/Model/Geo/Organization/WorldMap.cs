using System;
using System.Collections.Generic;
using Input.World;
using JetBrains.Annotations;
using Meta;
using Meta.EventArgs;
using Model.Geo.Support;
using UnityEngine;

namespace Model.Geo.Organization
{
    public sealed class WorldMap : MonoBehaviour
    {
        public int width;
        public int height;
        public GameObject tilePrefab;
        public GameObject selectionTilePrefab;
        public TileDetailsComponentControl control;

        [CanBeNull] public WorldTile selectedTile;
        [CanBeNull] public WorldArea selectedArea;
        [CanBeNull] public WorldRegion selectedRegion;
        
        private WorldTile _selectionTile;
        
        private Dictionary<string, Sprite> _sprites;


        private Dictionary<Position, WorldTile> _tiles;

        [CanBeNull]
        public WorldTile GetTile(Position position) => _tiles.ContainsKey(position) ? _tiles[position] : null;

        public IEnumerable<WorldTile> GetTiles()
        {
            return _tiles.Values;
        }
        
        public void UpdateSelection([CanBeNull] WorldTile newSelection, SelectionMode mode)
        {
            if (newSelection == null)
            {
                selectedTile = null;
                selectedArea = null;
                selectedRegion = null;
                _selectionTile.SetActive(false);
            }
            else
            {
                selectedTile = newSelection;
                _selectionTile.ChangePosition(selectedTile.Position);
                _selectionTile.SetActive(true);
                if (mode == SelectionMode.Area)
                {
                    if (selectedTile.worldArea != null)
                    {
                        selectedArea = selectedTile.worldArea;
                    }    
                } 
                else if (mode == SelectionMode.Region)
                {
                    if (selectedTile.worldArea != null)
                    {
                        if (selectedTile.worldArea.worldRegion != null)
                        {
                            selectedArea = selectedTile.worldArea;
                            selectedRegion = selectedArea.worldRegion;
                        }
                    }
                }
                OnWorldTileSelected(selectedTile);
            }
        }

        private void Start()
        {
            _tiles = new Dictionary<Position, WorldTile>();
            _sprites = new Dictionary<string, Sprite>
            {
                ["grassland"] = Resources.Load<Sprite>("GrasslandSprite")
            };

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var tile = Instantiate(tilePrefab, transform);
                    var worldTile = tile.GetComponent<WorldTile>();
                    worldTile.ChangePosition(new Position(i, j));
                    worldTile.WorldTileChanged += control.OnWorldTileChanged;
                    _tiles[worldTile.Position] = worldTile;
                    
                }
            }

            _selectionTile = Instantiate(selectionTilePrefab, transform).GetComponent<WorldTile>();
            _selectionTile.SetActive(false);

            WorldTileSelected += control.OnWorldTileSelected;
        }
        
        
        public event EventHandler<SelectedWorldTileEventArg> WorldTileSelected;

        private void OnWorldTileSelected(WorldTile e)
        {
            var args = new SelectedWorldTileEventArg(e);
            WorldTileSelected?.Invoke(this, args);
        }
    }
}