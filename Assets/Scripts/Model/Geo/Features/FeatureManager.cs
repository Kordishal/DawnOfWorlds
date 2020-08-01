using System.Collections.Generic;
using System.IO;
using Model.Geo.Features.Climate;
using UnityEngine;

namespace Model.Geo.Features
{
    public class FeatureManager
    {
        private static FeatureManager _instance;
        public static FeatureManager GetInstance() => _instance ?? (_instance = new FeatureManager());


        private const string SOFolder = "ScriptableObjects";
        private const string TFFolder = "TerrainFeatures";
        
        public WeatherEffect LoadWeatherEffects()
        {
            var path = Path.Combine(SOFolder, TFFolder, "SmallIsland");
            return Resources.Load<WeatherEffect>(path);
        }
    }
}