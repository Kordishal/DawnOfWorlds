using System.Collections.Generic;
using System.Linq;
using Model.Geo.Features.Climate;
using Model.Geo.Features.Terrain;
using UnityEngine;

namespace Model.Geo.Organization
{
    public class WorldArea : MonoBehaviour
    {
        public string areaName;
        public List<WorldTile> tiles;
        public WorldRegion worldRegion;

        public Climate climate;


        public bool IsContinental() => tiles.All(t => t.type != TileType.Oceanic);
        public bool IsOceanic() => tiles.All(t => t.type != TileType.Continental);
        public bool IsCoastal() => !IsContinental() && !IsOceanic();
        
        public override string ToString()
        {
            return areaName;
        }
    }
}