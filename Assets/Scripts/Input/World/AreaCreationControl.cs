using Meta;
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

        private GameObject _world;
        public GameObject worldAreaPrefab;

        private AreaBuilder _areaBuilder;

        private SelectionModeControl _modeControl;
        private SelectionDisplayControl _selectionDisplayControl;

        private void Start()
        {
            nameInputField.onEndEdit.AddListener(SaveName);
            saveButton.onClick.AddListener(Save);
            cancelButton.onClick.AddListener(Cancel);
            _world = GameObject.FindWithTag(Tags.World);
            _selectionDisplayControl = GameObject.FindWithTag(Tags.MainCamera).GetComponent<SelectionDisplayControl>();
            _modeControl = GameObject.FindWithTag(Tags.PowerButtonPanel).GetComponent<SelectionModeControl>();
        }

        public bool AddTileSelection(WorldTile tile)
        {
            return _areaBuilder.AddTile(tile);
        }

        public bool RemoveTileSelection(WorldTile tile)
        {
            return _areaBuilder.RemoveTile(tile);
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
            _selectionDisplayControl.ClearSelection(_areaBuilder.GetTiles());
            _areaBuilder = null;
            _modeControl.ChangeSelectionMode(SelectionMode.Area);
            gameObject.SetActive(false);
        }

        private void Save()
        {
            if (!_areaBuilder.IsValid()) return;
            var newArea = Instantiate(worldAreaPrefab, _world.transform);
            var worldArea = newArea.GetComponent<WorldArea>();
            _areaBuilder.Build(worldArea);
            _modeControl.ChangeSelectionMode(SelectionMode.Area);
            _areaBuilder = null;
            _selectionDisplayControl.UpdateSelection(worldArea.tiles[0]);
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _areaBuilder = new AreaBuilder();
        }
    }
}