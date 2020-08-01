using System.Collections.Generic;
using Model.Geo.Features.Terrain;
using UnityEngine;

namespace Model.Powers.ShapeTerrain
{
    [CreateAssetMenu(fileName = "terrainFeature", menuName = "ScriptableObjects/TerrainFeature", order = 2)]
    public class TerrainFeatureAction : ScriptableObject
    {
        public string objectName;
        public int cost;
        public string creationActionName;
        public TileType allowedTileType;
        public List<TerrainType> allowedTerrainTypes;
        public string description;

        public override string ToString()
        {
            return creationActionName;
        }
    }
}