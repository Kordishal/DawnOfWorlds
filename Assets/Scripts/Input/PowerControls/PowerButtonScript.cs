using UnityEngine;

namespace Input.PowerControls
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
                foreach (Transform child in parent.transform)
                {
                    child.gameObject.SetActive(false);
                }

                _panel.SetActive(true);
            }
            else if (_panel.activeInHierarchy)
            {
                _panel.SetActive(false);
            }
            else
            {
                foreach (Transform child in parent.transform)
                {
                    child.gameObject.SetActive(false);
                }

                _panel.SetActive(!_panel.activeInHierarchy);
            }
        }
    }
}