using System;

namespace Model.Geo.Features.Terrain
{
    [Serializable]
    public class Biome
    {
        public string name;
        public string description;

        public Biome(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public override string ToString()
        {
            return name;
        }
    }
}