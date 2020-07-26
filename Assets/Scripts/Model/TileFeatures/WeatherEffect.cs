using System;

namespace Model.TileFeatures
{
    [Serializable]
    public class WeatherEffect
    {
        public string name;
        public string description;
        
        public override string ToString()
        {
            return name;
        }
    }
}