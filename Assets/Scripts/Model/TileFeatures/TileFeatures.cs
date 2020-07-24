using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Model.TileFeatures
{
    public static class TileFeatures
    {
        private static readonly string BasePath = Application.streamingAssetsPath;
        private const string PathWeatherEffects = "tiles/effects/weather.json";
        private const string PathBiomes = "tiles/effects/weather.json";
        public static List<WeatherEffect> LoadWeatherEffects()
        {
            var path = System.IO.Path.Combine(BasePath, PathWeatherEffects);
            var text = File.ReadAllText(path);
            return JsonUtility.FromJson<WeatherEffectCollection>(text).effects;
        }
        public static List<Biome> LoadBiomes()
        {
            var path = System.IO.Path.Combine(BasePath, PathWeatherEffects);
            var text = File.ReadAllText(path);
            return JsonUtility.FromJson<BiomeCollection>(text).biomes;
        }

        [Serializable]
        private class WeatherEffectCollection
        {
            public List<WeatherEffect> effects;
        }

        [Serializable]
        private class BiomeCollection
        {
            public List<Biome> biomes;
        }
    }
}