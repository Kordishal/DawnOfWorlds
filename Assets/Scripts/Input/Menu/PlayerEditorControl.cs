using System;
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

        private GameObject mainMenu;
        private void OnNameInput(string input)
        {
            settings.Name = input;
        }
        
        private void OnClickMainMenu()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            settings = GameObject.Find("Player").GetComponent<PlayerSettings>();
            mainMenu = GameObject.Find("MainMenuComponent");
        }

        private void Start()
        {
            nameInput.onEndEdit.AddListener(OnNameInput);
            nameInput.text = settings.Name;
            mainMenuButton.onClick.AddListener(OnClickMainMenu);
        }
        
        private void OnDisable()
        {
            mainMenu.SetActive(true);
        }
    }
}
