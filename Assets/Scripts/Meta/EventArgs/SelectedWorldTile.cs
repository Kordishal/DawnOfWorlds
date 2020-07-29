using Model.Geo.Organization;

namespace Meta.EventArgs
{
    public class SelectedWorldTile : System.EventArgs
    {
        public readonly WorldTile Tile;

        public SelectedWorldTile(WorldTile worldTile)
        {
            Tile = worldTile;
        }
    }
}