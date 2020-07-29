using System;
using JetBrains.Annotations;
using Meta;
using UnityEngine;
using UnityEngine.UI;

namespace Input.World
{
    public class SelectionModeControl : MonoBehaviour
    {
        public GameObject detailsCanvas;
        public GameObject tileDetailsComponentPrefab;
        public GameObject areaDetailsComponentPrefab;
        public GameObject regionDetailsComponentPrefab;
        public SelectionDisplayControl selectionDisplayControl;

        public Button tileSelectionMode;
        public Button areaSelectionMode;
        public Button regionSelectionMode;

        private TileDetailsControl _tileDetailsControl;
        [CanBeNull] private AreaDetailsControl _areaDetailsControl;
        [CanBeNull] private RegionDetailsControl _regionDetailsControl;

        public SelectionMode CurrentMode { get; private set; }

        public void ChangeSelectionMode(SelectionMode newMode)
        {
            ChangeMode(CurrentMode, false);
            ChangeMode(newMode, true);
            selectionDisplayControl.ChangeSelectionMode(CurrentMode, newMode);
            CurrentMode = newMode;
        }

        private void ChangeMode(SelectionMode mode, bool enable)
        {
            switch (mode)
            {
                case SelectionMode.Tile:
                    tileSelectionMode.interactable = !enable;
                    ChangeTileDetailsComponentStatus(enable);
                    break;
                case SelectionMode.Area:
                    areaSelectionMode.interactable = !enable;
                    ChangeAreaDetailsComponentStatus(enable);
                    break;
                case SelectionMode.Region:
                    regionSelectionMode.interactable = !enable;
                    ChangeRegionDetailsComponentStatus(enable);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        private void Start()
        {
            _tileDetailsControl = GameObject.FindWithTag(Tags.TileDetailsComponent)
                .GetComponent<TileDetailsControl>();
            if (_tileDetailsControl == null)
                throw new Exception(
                    "Could not find TileDetailsComponent.");
            selectionDisplayControl.TileSelectionChange += _tileDetailsControl.OnTileSelectionChange;
            
            tileSelectionMode.onClick.AddListener(delegate { ChangeSelectionMode(SelectionMode.Tile); });
            areaSelectionMode.onClick.AddListener(delegate { ChangeSelectionMode(SelectionMode.Area); });
            regionSelectionMode.onClick.AddListener(delegate { ChangeSelectionMode(SelectionMode.Region); });
            ChangeSelectionMode(SelectionMode.Tile);
        }

        private void ChangeTileDetailsComponentStatus(bool newStatus)
        {
            _tileDetailsControl.SetActive(newStatus);
        }

        private void ChangeAreaDetailsComponentStatus(bool newStatus)
        {
            if (_areaDetailsControl == null)
            {
                _areaDetailsControl = Instantiate(areaDetailsComponentPrefab, detailsCanvas.transform)
                    .GetComponent<AreaDetailsControl>();
                if (_areaDetailsControl == null)
                    throw new Exception(
                        "Could not instantiate AreaDetailsComponent. Did you forget to assign the prefab?");
                selectionDisplayControl.AreaSelectionChange += _areaDetailsControl.OnAreaSelectionChange;
            }

            _areaDetailsControl.SetActive(newStatus);
        }

        private void ChangeRegionDetailsComponentStatus(bool newStatus)
        {
            if (_regionDetailsControl == null)
            {
                _regionDetailsControl = Instantiate(regionDetailsComponentPrefab, detailsCanvas.transform)
                    .GetComponent<RegionDetailsControl>();
                if (_regionDetailsControl == null)
                    throw new Exception(
                        "Could not instantiate RegionDetailsComponent. Did you forget to assign the prefab?");
                selectionDisplayControl.RegionSelectionChange += _regionDetailsControl.OnRegionSelectionChange;
            }

            _regionDetailsControl.SetActive(newStatus);
        }
    }
}