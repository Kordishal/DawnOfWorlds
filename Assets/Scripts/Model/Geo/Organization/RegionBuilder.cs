using System.Collections.Generic;
using System.Linq;

namespace Model.Geo.Organization
{
    public class RegionBuilder
    {
        private const int MinAreas = 2;
        private const int MaxAreas = 7;

        private string _name;

        private readonly List<WorldArea> _areas;

        public RegionBuilder()
        {
            _areas = new List<WorldArea>();
        }
        
        public IEnumerable<WorldArea> GetAreas() => _areas;

        public bool ChangeName(string name)
        {
            if (name == "") return false;
            _name = name;
            return true;
        }
        
        /**
         * Adds an area to the region. An area cannot be added to the region if is already part of a region.
         * The area must be either the first area or the number of areas does not exceed the maximum number of areas
         * and the area is neighbouring an already added area.
         *
         *
         * <param name="area">The area to be added to the region.</param>
         * <returns>Whether the area was added or not.</returns>
         */
        public bool AddArea(WorldArea area)
        {
            if (area.worldRegion != null) return false;
            if (IsEmpty())
            {
                _areas.Add(area);
                return true;
            }

            if (_areas.Count >= MaxAreas) return false;
            if (!HasNeighbour(area)) return false;
            _areas.Add(area);
            return true;
        }

        public bool RemoveArea(WorldArea area)
        {
            return _areas.Remove(area);
        }

        public void Build(WorldRegion region)
        {
            region.regionName = _name;
            region.areas = _areas;
            foreach (var worldArea in _areas)
            {
                worldArea.worldRegion = region;
            }
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(_name)) return false;
            return _areas.Count >= MinAreas;
        }
        
        private bool IsEmpty()
        {
            return _areas.Count == 0;
        }

        private bool HasNeighbour(WorldArea area)
        {
            foreach (var worldArea in _areas)
            {
                foreach (var worldTile in worldArea.tiles)
                {
                    if (area.tiles.Any(tile => tile.position.IsNeighbour(worldTile.position)))
                    {
                        return true;
                    }
                }
                
            }
            return false;
        }
    }
}