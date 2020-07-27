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

        private GameObject _playerEditorPanel;
        private GameObject _deityEditorPanel;

        private void Awake()
        {
            var backGroundCanvas = transform.parent.parent;
            _playerEditorPanel = Instantiate(playerEditorPrefab, backGroundCanvas).transform.GetChild(0).gameObject;
            _deityEditorPanel = Instantiate(deityEditorPrefab, backGroundCanvas).transform.GetChild(0).gameObject;
        }

        private void Start()
        {
            newGame.onClick.AddListener(NewGame);
            settings.onClick.AddListener(OnClickSettings);
            createDeity.onClick.AddListener(CreateDeity);
            quit.onClick.AddListener(QuitApplication);
            
            _playerEditorPanel.SetActive(false);    
            _deityEditorPanel.SetActive(false);
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
            _deityEditorPanel.SetActive(true);
            gameObject.SetActive(false);
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