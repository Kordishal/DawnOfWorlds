using System;
using UnityEngine;

namespace Model.Geo.Features.Terrain
{
    [CreateAssetMenu(fileName = "biome", menuName = "ScriptableObjects/Biome", order = 1)]
    public class Biome : ScriptableObject
    {
        public string biomeName;
        public string description;

        public override string ToString()
        {
            return biomeName;
        }
    }
}