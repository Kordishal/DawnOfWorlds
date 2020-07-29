using Input.World;
using Meta;
using UnityEngine;
using UnityEngine.UI;

namespace Input.PowerControls
{
    public class PowerButtonScript : MonoBehaviour
    {
        public GameObject parent;
        public GameObject powerPanel;
        public SelectionMode mode;
        public SelectionModeControl selectionModeControl;

        private GameObject _panel;

        private void OnButtonClick()
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
                selectionModeControl.ChangeSelectionMode(mode);
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

                _panel.SetActive(true);
                selectionModeControl.ChangeSelectionMode(mode);
            }
        }


        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }
    }
}