using Input.Dropdowns;
using Meta;
using Meta.EventArgs;
using Model.Geo.Features.Climate;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Menu
{
    public class PlayerEditorControl : MonoBehaviour
    {
        public InputField nameInput;
        public ClimateDropdown climateDropdown;
        
        public Button mainMenuButton;
        
        private GameObject _mainMenu;
        
        private void Start()
        {
            nameInput.onEndEdit.AddListener(delegate(string n) { ProfileSettings.Name = n; });
            nameInput.text = ProfileSettings.Name;
            climateDropdown.SetValue(ProfileSettings.DefaultClimate);
            climateDropdown.OnClimateChanged += delegate(object sender, ClimateEventArgs args)
            {
                ProfileSettings.DefaultClimate = args.Climate;
            };
            
            mainMenuButton.onClick.AddListener(delegate { gameObject.SetActive(false); });
        }

        /**
         * <summary>
         * Sets the main menu component in order to re-enable it after the editor is closed.
         * <param name="menu">The main menu component</param>
         * </summary>
         */
        public void SetMainMenu(GameObject menu)
        {
            _mainMenu = menu;
        }
        
        private void OnDisable()
        {
            _mainMenu.SetActive(true);
        }
    }
}