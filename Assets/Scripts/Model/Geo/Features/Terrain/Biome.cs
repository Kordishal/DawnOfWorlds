using System;
using UnityEngine;

namespace Model.Geo.Features.Terrain
{
    [Serializable]
    public class Biome
    {
        public string name;
        public BiomeCategory category;
        public override string ToString()
        {
            return name;
        }
    }
}