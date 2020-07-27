using System;
using JetBrains.Annotations;
using Meta.EventArgs;
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

        private void OnClickMainMenu()
        {
            gameObject.SetActive(false);
        }

        private void ActivateDetailsViewOnSelection(object sender, ChangedCurrentDeityEventUpdate currentDeityArgs)
        {
            var currentDeity = currentDeityArgs.ChangedCurrentDeity;
            if (currentDeity == null) return;
            if (deityDetail.activeInHierarchy) return;
            deityDetail.SetActive(true);
        } 

        private void CreateDeity()
        {
            deityDetail.SetActive(false);
            deityEdit.SetActive(true);
            _factory.CurrentDeity = null;
        }

        private void DeleteDeity()
        {
            if (_factory.CurrentDeity == null || _factory.CurrentDeity.identifier == 0) return;
            if (!_factory.DeleteDeity(_factory.CurrentDeity.identifier)) return;
            _factory.CurrentDeity = null;
            deityEdit.SetActive(false);
            deityDetail.SetActive(false);
        }

        private void EditDeity()
        {
            if (_factory.CurrentDeity == null) return;
            deityDetail.SetActive(false);
            deityEdit.SetActive(true);
        }

        private void Awake()
        {
            deityDetail = Instantiate(deityDetailPrefab, transform);
            deityEdit = Instantiate(deityEditPrefab, transform);
        }

        private void Start()
        {
            _factory = DeityFactory.GetInstance(Application.persistentDataPath);
            _factory.OnCurrentDeityChange -= ActivateDetailsViewOnSelection;
            _factory.OnCurrentDeityChange += ActivateDetailsViewOnSelection;
            _mainMenu = GameObject.Find("MainMenuComponent");
            mainMenuButton.onClick.AddListener(OnClickMainMenu);
            createDeity.onClick.AddListener(CreateDeity);
            deleteDeity.onClick.AddListener(DeleteDeity);
            editDeity.onClick.AddListener(EditDeity);
        }

        private void OnEnable()
        {
            if (_factory == null) return;
            _factory.OnCurrentDeityChange -= ActivateDetailsViewOnSelection;
            _factory.OnCurrentDeityChange += ActivateDetailsViewOnSelection;
        }

        private void OnDisable()
        {
            if (_mainMenu != null)
                _mainMenu.SetActive(true);
            if (_factory != null)
            {
                _factory.OnCurrentDeityChange -= ActivateDetailsViewOnSelection;
            }
        }
    }
}