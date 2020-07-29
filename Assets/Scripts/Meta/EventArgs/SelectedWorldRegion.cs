using Model.Geo.Organization;

namespace Meta.EventArgs
{
    public class SelectedWorldRegion : System.EventArgs
    {
        public readonly WorldRegion Region;

        public SelectedWorldRegion(WorldRegion worldRegion)
        {
            Region = worldRegion;
        }
    }
}