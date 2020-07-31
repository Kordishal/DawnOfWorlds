using System;
using System.Collections.Generic;
using System.Linq;
using Input.World;
using Meta;
using Model.Geo.Features.Terrain;
using Model.Geo.Support;
using UnityEngine;

namespace Model.Geo.Organization
{
    public sealed class WorldMap : MonoBehaviour
    {
        private const int PositionFactor = 7;

        public int width;
        public int height;
        public GameObject tilePrefab;

        private Dictionary<string, Sprite> _sprites;

        private Dictionary<Position, WorldTile> _tiles;

        private void Start()
        {
            var tileDetailsControl = GameObject.FindWithTag(Tags.TileDetailsComponent)
                .GetComponent<TileDetailsControl>();
            if (tileDetailsControl == null)
                throw new Exception(
                    "Could not find TileDetailsComponent.");
            _tiles = new Dictionary<Position, WorldTile>();
            _sprites = new Dictionary<string, Sprite>
            {
                [SpriteTags.Grassland] = Resources.Load<Sprite>("GrasslandSprite"),
                [SpriteTags.DeepOcean] = Resources.Load<Sprite>("DeepOceanSprite"),
                [SpriteTags.ShallowOcean] = Resources.Load<Sprite>("ShallowOceanSprite")
            };

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var tile = Instantiate(tilePrefab, transform);
                    var worldTile = tile.GetComponent<WorldTile>();
                    worldTile.position = new Position(i, j);
                    worldTile.transform.position = new Vector3(i * PositionFactor, j * PositionFactor, 20);
                    worldTile.OnWorldTileChanged += tileDetailsControl.OnTileDataUpdate;
                    worldTile.worldMap = this;
                    _tiles[worldTile.position] = worldTile;
                }
            }
        }


        private IEnumerable<WorldTile> AllNeighbours(WorldTile tile)
        {
            var position = tile.position;
            return from direction in position.AllDirections()
                where _tiles.ContainsKey(direction)
                select _tiles[direction];
        }

        public void UpdateSprite(WorldTile tile)
        {
            if (tile.type == TileType.Continental)
            {
                tile.renderer.sprite = _sprites[SpriteTags.Grassland];
            }
            else
            {
                if (AllNeighbours(tile).Any(t => t.type == TileType.Continental))
                    tile.renderer.sprite = _sprites[SpriteTags.ShallowOcean];
                else
                    tile.renderer.sprite = _sprites[SpriteTags.DeepOcean];
            }
        }

        public void UpdateAllSprites()
        {
            foreach (var value in _tiles.Values)
            {
                UpdateSprite(value);
            }
        }
    }
}