using Model.Geo.Organization;
using UnityEngine;
using UnityEngine.UI;

namespace Input.World
{
    public class AreaCreationControl : MonoBehaviour
    {
        public InputField nameInputField;

        public Button saveButton;
        public Button cancelButton;

        private AreaBuilder _areaBuilder;

        private SelectionModeControl _modeControl;

        private void Start()
        {
            nameInputField.onEndEdit.AddListener(SaveName);
            saveButton.onClick.AddListener(Save);
            cancelButton.onClick.AddListener(Cancel);
        }

        public bool AddTileSelection(WorldTile tile)
        {
            return _areaBuilder.AddTile(tile);
        }

        private void SaveName(string text)
        {
            if (!_areaBuilder.ChangeName(text))
            {
                Debug.Log("Invalid name for an area: " + text);
                // TODO: Implement a warning for invalid names (currently only empty names).   
            }
        }

        private void Cancel()
        {
            gameObject.SetActive(false);
        }

        private void Save()
        {
            if (!_areaBuilder.IsValid()) return;
            // TODO: Implement save

            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _areaBuilder = null;
        }

        private void OnEnable()
        {
            _areaBuilder = new AreaBuilder();
        }
    }
}