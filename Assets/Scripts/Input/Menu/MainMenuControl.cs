using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Menu
{
    public class MainMenuControl : MonoBehaviour
    {
        public GameObject playerEditorPrefab;
        public GameObject deityEditorPrefab;

        public Button newGame;
        public Button settings;
        public Button createDeity;
        public Button quit;

        [CanBeNull] private GameObject _playerEditorPanel;
        [CanBeNull] private GameObject _deityEditorPanel;

        private Transform _uiCanvas;

        private void Awake()
        {
            _uiCanvas = transform.parent.parent;
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
            if (_playerEditorPanel == null)
            {
                _playerEditorPanel = Instantiate(playerEditorPrefab, _uiCanvas).transform.GetChild(0).gameObject;
                _playerEditorPanel.GetComponent<PlayerEditorControl>().SetMainMenu(gameObject);
            }

            _playerEditorPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        private void NewGame()
        {
        }

        private void CreateDeity()
        {
            if (_deityEditorPanel == null)
                _deityEditorPanel = Instantiate(deityEditorPrefab, _uiCanvas).transform.GetChild(0).gameObject;
            _deityEditorPanel.GetComponent<DeityEditorControl>().mainMenu = gameObject;
            gameObject.SetActive(false);
            _deityEditorPanel.SetActive(true);
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