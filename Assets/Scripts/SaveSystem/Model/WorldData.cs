using System;
using System.Collections.Generic;
using Model.Geo.Organization;

namespace SaveSystem.Model
{
    [Serializable]
    public class WorldData
    {
        public List<WorldTileData> tileData;
        public List<WorldAreaData> areaData;
        public List<WorldRegionData> regionData;
    }
}