using System.Collections.Generic;
using Model.Geo.Features.Climate;
using UnityEngine;

namespace Model.Geo.Organization
{
    public class WorldArea : MonoBehaviour
    {
        public string areaName;
        public List<WorldTile> tiles;
        public WorldRegion worldRegion;

        public Climate climate;

        public override string ToString()
        {
            return areaName;
        }
    }
}