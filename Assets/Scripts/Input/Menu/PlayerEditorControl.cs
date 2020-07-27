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

        public GameObject mainMenu;

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