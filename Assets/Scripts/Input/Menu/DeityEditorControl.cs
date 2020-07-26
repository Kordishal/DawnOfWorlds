using JetBrains.Annotations;
using Model.Deity;
using Player.Data;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Input.Menu
{
    public class DeityEditorControl : MonoBehaviour
    {

        public Button mainMenuButton;
        public Button createDeity;
        public Button deleteDeity;
        public Button editDeity;

        public GameObject deityEditPrefab;
        public GameObject deityEdit;
        public GameObject deityDetailPrefab;
        public GameObject deityDetail;
        private GameObject _mainMenu;

        private DeityFactory _factory;

        [CanBeNull]
        public Deity currentDeity;
        
        private void OnClickMainMenu()
        {
            gameObject.SetActive(false);
        }

        private void CreateDeity()
        {
            currentDeity = new Deity();
            deityEdit.SetActive(true);
        }

        private void DeleteDeity()
        {
            if (currentDeity == null || currentDeity.identifier == 0) return;
            _factory.DeleteDeity(currentDeity.identifier);
            currentDeity = null;
            deityEdit.SetActive(false);
            deityDetail.SetActive(false);
        }

        private void EditDeity()
        {
            
        }
        
        private void Awake()
        {
            deityDetail = Instantiate(deityDetailPrefab, transform);
            deityDetail.SetActive(false);
            deityEdit = Instantiate(deityEditPrefab, transform);
            deityEdit.SetActive(false);   
        }

        private void Start()
        {
            _factory = DeityFactory.GetInstance(Application.persistentDataPath);
            _mainMenu = GameObject.Find("MainMenuComponent");
            mainMenuButton.onClick.AddListener(OnClickMainMenu);
            createDeity.onClick.AddListener(CreateDeity);
            deleteDeity.onClick.AddListener(DeleteDeity);
            editDeity.onClick.AddListener(EditDeity);
        }
        
        private void OnDisable()
        {
            if (_mainMenu != null)
                _mainMenu.SetActive(true);
        }
    }
}