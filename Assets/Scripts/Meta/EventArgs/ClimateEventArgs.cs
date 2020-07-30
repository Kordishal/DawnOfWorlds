using Model.Geo.Features.Climate;

namespace Meta.EventArgs
{
    public class ClimateEventArgs : System.EventArgs
    {
        public Climate Climate { get; private set; }

        public ClimateEventArgs(Climate climate)
        {
            Climate = climate;
        }
    }
}