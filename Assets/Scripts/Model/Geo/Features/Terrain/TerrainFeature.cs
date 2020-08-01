using System.Collections.Generic;
using UnityEngine;

namespace Model.Geo.Features.Terrain
{
    [CreateAssetMenu(fileName = "terrainFeature", menuName = "ScriptableObjects/TerrainFeature", order = 2)]
    public class TerrainFeature : ScriptableObject
    {
        public string objectName;
        public int cost;
        public string creationActionName;
        public string allowedTileTypes;
        public List<string> allowedTerrainFeatures;
        public string description;

        public override string ToString()
        {
            return objectName;
        }
    }
}