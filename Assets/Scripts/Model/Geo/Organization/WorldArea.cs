using System.Collections.Generic;
using UnityEngine;

namespace Model.Geo.Organization
{
    public class WorldArea : MonoBehaviour
    {
        public string areaName;
        public List<WorldTile> tiles;
        public WorldRegion worldRegion;


        public override string ToString()
        {
            return areaName;
        }
    }
}