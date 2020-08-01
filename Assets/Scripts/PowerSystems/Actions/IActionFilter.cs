using System.Collections.Generic;
using Model.Geo.Organization;

namespace PowerSystems.Actions
{
    public interface IActionFilter
    {

        IAction GetActionByIndex(int index);
        
        List<IAction> GetFilteredActions(WorldTile tile);
    }
}