using Meta;
using UnityEngine;
using UnityEngine.UI;

namespace Input.World
{
    public class SelectionModeControl : MonoBehaviour
    {

        public Button tileSelectionMode;
        public Button areaSelectionMode;
        public Button regionSelectionMode;

        public SelectionMode Mode { get; private set; }

        public void ChangeSelectionMode(SelectionMode mode)
        {
            tileSelectionMode.interactable = true;
            areaSelectionMode.interactable = true;
            regionSelectionMode.interactable = true;
            Mode = mode;
            switch (mode)
            {
                case SelectionMode.Tile:
                    tileSelectionMode.interactable = false;
                    break;
                case SelectionMode.Area:
                    areaSelectionMode.interactable = false;
                    break;
                case SelectionMode.Region:
                    regionSelectionMode.interactable = false;
                    break;
            }
        }

        private void Start()
        {
            tileSelectionMode.onClick.AddListener(delegate { ChangeSelectionMode(SelectionMode.Tile); });
            areaSelectionMode.onClick.AddListener(delegate { ChangeSelectionMode(SelectionMode.Area); });
            regionSelectionMode.onClick.AddListener(delegate { ChangeSelectionMode(SelectionMode.Region); });
            ChangeSelectionMode(SelectionMode.Tile);
        }
    }
}
