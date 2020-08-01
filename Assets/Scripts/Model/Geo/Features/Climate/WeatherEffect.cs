using System;
using UnityEngine;

namespace Model.Geo.Features.Climate
{
    [CreateAssetMenu(fileName = "terrainFeature", menuName = "ScriptableObjects/WeatherEffect", order = 3)]
    public class WeatherEffect : ScriptableObject
    {
        public string objectName;
        public string description;
        
        public override string ToString()
        {
            return objectName;
        }
    }
}