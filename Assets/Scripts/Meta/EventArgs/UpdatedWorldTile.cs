using Model.Geo.Organization;

namespace Meta.EventArgs
{
    public class UpdatedWorldTile : System.EventArgs
    {
        public readonly WorldTile Tile;

        public UpdatedWorldTile(WorldTile worldTile)
        {
            Tile = worldTile;
        }
    }
}