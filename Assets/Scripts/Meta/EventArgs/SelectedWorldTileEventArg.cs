using Model.Tiles;

namespace Meta.EventArgs
{
    public class SelectedWorldTileEventArg : System.EventArgs
    {
        public readonly WorldTile SelectedTile;

        public SelectedWorldTileEventArg(WorldTile worldTile)
        {
            SelectedTile = worldTile;
        }
    }
}