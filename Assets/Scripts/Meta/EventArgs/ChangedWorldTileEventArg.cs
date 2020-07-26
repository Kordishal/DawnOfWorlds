using Model.Tiles;

namespace Meta.EventArgs
{
    public class ChangedWorldTileEventArg : System.EventArgs
    {
        public readonly WorldTile ChangedWorldTile;

        public ChangedWorldTileEventArg(WorldTile worldTile)
        {
            ChangedWorldTile = worldTile;
        }
    }
}