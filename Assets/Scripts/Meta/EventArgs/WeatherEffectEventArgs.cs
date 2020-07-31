using Model.Geo.Features.Climate;

namespace Meta.EventArgs
{
    public class WeatherEffectEventArgs : System.EventArgs
    {
        public WeatherEffect WeatherEffect { get; private set; }

        public WeatherEffectEventArgs(WeatherEffect weatherEffect)
        {
            WeatherEffect = weatherEffect;
        }
    }
}