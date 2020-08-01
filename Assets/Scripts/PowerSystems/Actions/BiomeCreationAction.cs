using System.Collections.Generic;
using Model.Geo.Features.Climate;
using Model.Geo.Features.Terrain;
using UnityEngine;

namespace PowerSystems.Actions
{
    [CreateAssetMenu(fileName = "biomeCreationAction", menuName = "ScriptableObjects/Biomes", order = 4)]
    public class BiomeCreationAction : ScriptableObject, IAction
    {
        public string objectName;
        public string actionName;
        public TileType allowedTileType;
        public List<TerrainType> allowedTerrainTypes;
        public List<Climate> allowedClimates;
        public BiomeCategory category;

        public override string ToString()
        {
            return actionName;
        }

        public string GetName()
        {
            return actionName;
        }
    }
}