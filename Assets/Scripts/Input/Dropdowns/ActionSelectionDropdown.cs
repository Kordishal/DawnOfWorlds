using System.Linq;
using JetBrains.Annotations;
using Model.Geo.Organization;
using PowerSystems.Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Dropdowns
{
    public class ActionSelectionDropdown : MonoBehaviour
    {
        private Dropdown _dropdown;
        private IActionFilter _actionFilter;
        private void Start()
        {
            _dropdown = GetComponent<Dropdown>();
            _dropdown.interactable = false;
        }
        
        public void SetFilter(IActionFilter actionFilter)
        {
            _actionFilter = actionFilter;
        }

        public void UpdateElements([CanBeNull] WorldTile tile)
        {
            _dropdown.options.Clear();
            if (tile == null)
            {
                _dropdown.interactable = false;
                return;
            }
            var list = _actionFilter.GetFilteredActions(tile);
            if (list.Any())
            {
                foreach (var action in list)
                {
                    _dropdown.options.Add(new Dropdown.OptionData(action.GetName()));
                }

                _dropdown.interactable = true;
            }
            else
            {
                _dropdown.interactable = false;
            }
        }
        
        public bool IsActive() => _dropdown.interactable;

        public IAction CurrentValue() => _actionFilter.GetActionByIndex(_dropdown.value);

    }
}