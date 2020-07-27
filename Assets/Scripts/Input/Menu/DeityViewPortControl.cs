using System;
using System.Collections.Generic;
using System.Linq;
using Meta.EventArgs;
using Player.Data;
using UnityEngine;

namespace Input.Menu
{
    public class DeityViewPortControl : MonoBehaviour
    {
        public GameObject deityButtonPrefab;
        public GameObject content;

        private List<DeitySelectButtonControl> _buttons;
        
        public void UpdateButtons(DeityFactory factory, Action activateDetailsView, Action deactivateDetailsView)
        {
            void SelectDeityOnClick(int identifier)
            {
                if (factory.CurrentDeity != null && factory.CurrentDeity.identifier == identifier)
                {
                    deactivateDetailsView();
                    factory.CurrentDeity = null;
                }
                else
                {
                    factory.CurrentDeity = factory.GetDeity(identifier);
                    activateDetailsView();
                }
            }
            
            if (_buttons == null) _buttons = new List<DeitySelectButtonControl>();
            foreach (var button in _buttons)
            {
                factory.OnCurrentDeityChange -= button.ChangeCurrentDeity;
                Destroy(button.gameObject);
            }
            _buttons.Clear();
            var count = 1;
            var collection = factory.GetDeities().OrderBy(deity => deity.identifier);
            foreach (var deity in collection)
            {
                var deityButton = Instantiate(deityButtonPrefab, content.transform);
                var control = deityButton.GetComponent<DeitySelectButtonControl>();
                control.UpdateText(deity);
                control.button.onClick.AddListener(delegate { SelectDeityOnClick(deity.identifier); });
                factory.OnCurrentDeityChange += control.ChangeCurrentDeity;
                control.ChangeCurrentDeity(null, new DeityEventArgs(factory.CurrentDeity));
                var deityButtonRect = deityButton.GetComponent<RectTransform>();
                deityButtonRect.anchoredPosition = new Vector2(0, 30 + (-60 * count));
                deityButtonRect.offsetMax = new Vector2(0, deityButtonRect.offsetMax.y);
                deityButtonRect.offsetMin = new Vector2(0, deityButtonRect.offsetMin.y);
                _buttons.Add(control);
                count += 1;
            }
            var contentRect = content.GetComponent<RectTransform>();
            contentRect.offsetMin = new Vector2(contentRect.offsetMin.x, -60 * (count - 1));
        }
    }
}