using System.Collections.Generic;
using System.Linq;

namespace Model.Geo.Organization
{
    public class AreaBuilder
    {
        private const int MinTiles = 5;
        private const int MaxTiles = 12;

        private string _name;

        private readonly List<WorldTile> _tiles;

        public AreaBuilder()
        {
            _tiles = new List<WorldTile>();
        }
        
        public IEnumerable<WorldTile> GetTiles() => _tiles;

        public bool ChangeName(string name)
        {
            if (name == "") return false;
            _name = name;
            return true;
        }
        
        /**
         * Adds a tile to the area. A tile can only be added if it is
         * either the first tile or the number of tiles does not exceed the maximum number of tiles
         * and the tile is neighbouring an already added tile.
         *
         * <param name="tile">The tile to be added to the area.</param>
         * <returns>Whether the tile was added or not.</returns>
         */
        public bool AddTile(WorldTile tile)
        {
            if (IsEmpty())
            {
                _tiles.Add(tile);
                return true;
            }

            if (_tiles.Count >= MaxTiles) return false;
            if (!HasNeighbour(tile)) return false;
            _tiles.Add(tile);
            return true;
        }

        public bool RemoveTile(WorldTile tile)
        {
            return _tiles.Remove(tile);
        }

        public void Build(WorldArea area)
        {
            area.areaName = _name;
            area.tiles = _tiles;
            foreach (var worldTile in _tiles)
            {
                worldTile.worldArea = area;
            }
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(_name)) return false;
            return _tiles.Count >= MinTiles;
        }
        
        private bool IsEmpty()
        {
            return _tiles.Count == 0;
        }

        private bool HasNeighbour(WorldTile tile)
        {
            return _tiles.Any(worldTile => worldTile.position.IsNeighbour(tile.position));
        }
    }
}