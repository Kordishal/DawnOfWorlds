using System.Linq;
using Meta;
using Model.Geo.Organization;
using UnityEngine;
using UnityEngine.UI;

namespace Input.World
{
    public class RegionCreationControl : MonoBehaviour
    {
        public InputField nameInputField;

        public Button saveButton;
        public Button cancelButton;

        private GameObject _world;
        public GameObject worldRegionPrefab;

        private RegionBuilder _regionBuilder;

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

        public bool AddArea(WorldArea area)
        {
            return _regionBuilder.AddArea(area);
        }

        public bool RemoveArea(WorldArea area)
        {
            return _regionBuilder.RemoveArea(area);
        }

        private void SaveName(string text)
        {
            if (!_regionBuilder.ChangeName(text))
            {
                Debug.Log("Invalid name for an area: " + text);
                // TODO: Implement a warning for invalid names (currently only empty names).   
            }
        }

        private void Cancel()
        {
            _selectionDisplayControl.ClearSelection(_regionBuilder.GetAreas().SelectMany(area => area.tiles));
            _regionBuilder = null;
            _modeControl.ChangeSelectionMode(SelectionMode.Region);
            gameObject.SetActive(false);
        }

        private void Save()
        {
            if (!_regionBuilder.IsValid()) return;
            var newRegion = Instantiate(worldRegionPrefab, _world.transform);
            var worldRegion = newRegion.GetComponent<WorldRegion>();
            _regionBuilder.Build(worldRegion);
            _modeControl.ChangeSelectionMode(SelectionMode.Region);
            _regionBuilder = null;
            _selectionDisplayControl.UpdateSelection(worldRegion.areas[0].tiles[0]);
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _regionBuilder = new RegionBuilder();
        }
    }
}