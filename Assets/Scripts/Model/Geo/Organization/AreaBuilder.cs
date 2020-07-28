using System.Collections.Generic;

namespace Model.Geo.Organization
{
    public class AreaBuilder
    {

        private const int MinTiles = 5;
        private const int MaxTiles = 12;

        private string _name;
        
        private List<WorldTile> _tiles;

        public AreaBuilder()
        {
            _tiles = new List<WorldTile>();
        }

        public void AddName(string name)
        {
            _name = name;
        }

        public bool AddTile(WorldTile tile)
        {
            if (_tiles.Count < 12)
            {
                _tiles.Add(tile);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveTile(WorldTile tile)
        {
            return _tiles.Remove(tile);
        }

        public bool CreateArea(WorldArea area)
        {
            return false;
        }


        public bool IsValid()
        {
            if (string.IsNullOrEmpty(_name)) return false;
            if (_tiles.Count < MinTiles) return false;
            return true;
        }
    }
}