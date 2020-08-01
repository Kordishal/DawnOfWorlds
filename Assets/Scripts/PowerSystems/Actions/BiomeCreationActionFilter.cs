using System.Collections.Generic;
using System.Linq;
using Model.Geo.Features;
using Model.Geo.Organization;

namespace PowerSystems.Actions
{
    public class BiomeCreationActionFilter : IActionFilter
    {
        private readonly List<BiomeCreationAction> _actions;
        private readonly Dictionary<int, BiomeCreationAction> _actionsMap;
        
        public IAction GetActionByIndex(int index)
        {
            return _actionsMap[index];
        }

        public List<IAction> GetFilteredActions(WorldTile tile)
        {
            var count = 1;
            return _actions
                .Where(a => a.allowedTileType == tile.type)
                .Where(a => a.allowedClimates.Contains(tile.Climate))
                .Where(a => a.allowedTerrainTypes.Contains(tile.terrain))
                .Select(a =>
                {
                    _actionsMap[count] = a;
                    count += 1;
                    return a;
                })
                .Cast<IAction>()
                .ToList();

        }

        public BiomeCreationActionFilter()
        {
            _actions = FeatureManager.GetInstance().LoadBiomes().ToList();
            _actionsMap = new Dictionary<int, BiomeCreationAction>();
        }
    }
}