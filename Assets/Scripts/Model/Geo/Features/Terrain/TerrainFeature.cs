using System;

namespace Model.Geo.Features.Terrain
{
    [Serializable]
    public class TerrainFeature
    {
        public string name;
        public string featureType;
        public string description;

        public override string ToString()
        {
            return name;
        }
    }
}