using Meta;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Menu
{
    public class PlayerEditorControl : MonoBehaviour
    {

        public InputField nameInput;
        public PlayerSettings settings;
        public Button mainMenuButton;

        private GameObject _mainMenu;
        private void OnNameInput(string input)
        {
            settings.Name = input;
        }
        
        private void OnClickMainMenu()
        {
            gameObject.SetActive(false);
        }
        
        private void Start()
        {
            settings = GameObject.Find("Player").GetComponent<PlayerSettings>();
            _mainMenu = GameObject.FindWithTag(Tags.MainMenuComponent);
            nameInput.onEndEdit.AddListener(OnNameInput);
            nameInput.text = settings.Name;
            mainMenuButton.onClick.AddListener(OnClickMainMenu);
        }
        
        private void OnDisable()
        {
            _mainMenu.SetActive(true);
        }
    }
}
