using Model.Geo.Features.Terrain;
using PowerSystems.Actions;

namespace Meta.EventArgs
{
    public class TerrainFeaturesEventArgs : System.EventArgs
    {
        public TerrainFeatureAction TerrainFeatureAction { get; }

        public TerrainFeaturesEventArgs(TerrainFeatureAction terrainFeatureAction)
        {
            TerrainFeatureAction = terrainFeatureAction;
        }
    }
}