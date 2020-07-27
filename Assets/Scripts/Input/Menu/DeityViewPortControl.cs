using System;
using System.Collections.Generic;
using Model.Deity;
using Player.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Menu
{
    public class DeityViewPortControl : MonoBehaviour
    {
        public GameObject deityButtonPrefab;
        public GameObject content;

        private DeityFactory _factory;

        private List<DeitySelectButtonControl> _buttons;

        private void Start()
        {
            _factory = DeityFactory.GetInstance(Application.persistentDataPath);
            _factory.OnDeityListChange += OnDeityUpdate;
            OnDeityUpdate(null, null);
        }

        private void SelectDeityOnClick(int identifier)
        {
            _factory.CurrentDeity = _factory.GetDeity(identifier);
        }

        private void OnDeityUpdate(object sender, EventArgs eventArgs)
        {
            if (_buttons == null) _buttons = new List<DeitySelectButtonControl>();
            foreach (var button in _buttons)
            {
                _factory.OnCurrentDeityChange -= button.ChangeCurrentDeity;
                Destroy(button);
            }
            _buttons.Clear();
            var count = 1;
            foreach (var deity in _factory.GetDeities())
            {
                var deityButton = Instantiate(deityButtonPrefab, content.transform);
                var control = deityButton.GetComponent<DeitySelectButtonControl>();
                control.UpdateText(deity);
                control.button.onClick.AddListener(delegate { SelectDeityOnClick(deity.identifier); });
                _factory.OnCurrentDeityChange += control.ChangeCurrentDeity;
                var deityButtonRect = deityButton.GetComponent<RectTransform>();
                deityButtonRect.anchoredPosition = new Vector2(0, 30 + (-60 * count));
                deityButtonRect.offsetMax = new Vector2(0, deityButtonRect.offsetMax.y);
                deityButtonRect.offsetMin = new Vector2(0, deityButtonRect.offsetMin.y);
                count += 1;
            }

            var contentRect = content.GetComponent<RectTransform>();
            contentRect.offsetMin = new Vector2(contentRect.offsetMin.x, -60 * (count - 1));
        }
    }
}