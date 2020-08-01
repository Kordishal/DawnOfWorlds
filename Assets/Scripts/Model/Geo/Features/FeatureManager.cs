using System.Collections.Generic;
using System.IO;
using Model.Geo.Features.Climate;
using Model.Geo.Features.Terrain;
using Model.Powers.ShapeTerrain;
using UnityEngine;

namespace Model.Geo.Features
{
    public class FeatureManager
    {
        private static FeatureManager _instance;
        public static FeatureManager GetInstance() => _instance ?? (_instance = new FeatureManager());
        private const string WeatherEffectsFolder = "WeatherEffects";
        private const string TerrainFeaturesFolder = "TerrainFeatures";
        
        public IEnumerable<WeatherEffect> LoadWeatherEffects()
        {
            return Resources.LoadAll<WeatherEffect>(WeatherEffectsFolder);
        }

        public IEnumerable<TerrainFeatureAction> LoadTerrainFeatures()
        {
            return Resources.LoadAll<TerrainFeatureAction>(TerrainFeaturesFolder);
        }
    }
}