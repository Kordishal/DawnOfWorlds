using System;

namespace Model.Geo.Features.Climate
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