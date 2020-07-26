using UnityEngine;
using UnityEngine.UI;

namespace Input.Menu
{
    public class MainMenuControl : MonoBehaviour
    {
        public GameObject playerEditorPrefab;

        public Button newGame;
        public Button settings;
        public Button createDeity;
        public Button quit;

        private GameObject _playerEditorPanel;


        private void Awake()
        {
            _playerEditorPanel = Instantiate(playerEditorPrefab, transform.parent.parent).transform.GetChild(0).gameObject;
            _playerEditorPanel.SetActive(false);
        }

        private void Start()
        {
            newGame.onClick.AddListener(NewGame);
            settings.onClick.AddListener(OnClickSettings);
            createDeity.onClick.AddListener(CreateDeity);
            quit.onClick.AddListener(QuitApplication);
        }

        private void OnClickSettings()
        {
            _playerEditorPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        private void NewGame()
        {
        }

        private void CreateDeity()
        {
            // gameObject.SetActive(false);
        }

        private void QuitApplication()
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}