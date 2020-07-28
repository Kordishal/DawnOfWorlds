using UnityEngine;

namespace Input
{
    public class PowerButtonScript : MonoBehaviour
    {
        public GameObject parent;
        public GameObject powerPanel;
        private GameObject _panel;

        public void OnButtonClick()
        {
            if (_panel == null)
            {
                var instance = Instantiate(powerPanel, parent.transform);
                _panel = instance;
                _panel.SetActive(true);
            }
            else
            {
                _panel.SetActive(!_panel.activeInHierarchy);
            }
        }
    }
}