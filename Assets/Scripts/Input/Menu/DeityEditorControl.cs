using Model.Deity;
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
            
        }

        private void EditDeity()
        {
            
        }
        
        private void Awake()
        {
            _mainMenu = GameObject.Find("MainMenuComponent");
            deityDetail = Instantiate(deityDetailPrefab, transform);
            deityDetail.SetActive(false);
            deityEdit = Instantiate(deityEditPrefab, transform);
            deityEdit.SetActive(false);
        }

        private void Start()
        {
            mainMenuButton.onClick.AddListener(OnClickMainMenu);
            createDeity.onClick.AddListener(CreateDeity);
            deleteDeity.onClick.AddListener(DeleteDeity);
            editDeity.onClick.AddListener(EditDeity);
        }
        
        private void OnDisable()
        {
            _mainMenu.SetActive(true);
        }
    }
}