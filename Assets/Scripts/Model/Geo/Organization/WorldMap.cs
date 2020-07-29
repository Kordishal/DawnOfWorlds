using System;
using System.Collections.Generic;
using Input.World;
using Meta;
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
                ["grassland"] = Resources.Load<Sprite>("GrasslandSprite")
            };

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var tile = Instantiate(tilePrefab, transform);
                    var worldTile = tile.GetComponent<WorldTile>();
                    worldTile.position = new Position(i, j);
                    worldTile.transform.position = new Vector3(i * 7, j * 7, 20);
                    worldTile.worldMap = this;
                    worldTile.WorldTileChanged += tileDetailsControl.OnTileDataUpdate;
                    _tiles[worldTile.position] = worldTile;
                }
            }
        }
    }
}