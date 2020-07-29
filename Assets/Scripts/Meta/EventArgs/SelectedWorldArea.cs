using Model.Geo.Organization;

namespace Meta.EventArgs
{
    public class SelectedWorldArea : System.EventArgs
    {
        public readonly WorldArea Area;

        public SelectedWorldArea(WorldArea worldArea)
        {
            Area = worldArea;
        }
    }
}