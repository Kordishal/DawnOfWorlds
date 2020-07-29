using System.Collections.Generic;
using UnityEngine;

namespace Model.Geo.Organization
{
    public class WorldRegion : MonoBehaviour
    {
        public string regionName;
        public List<WorldArea> areas;
        
        public override string ToString()
        {
            return regionName;
        }
    }
}