using System;
using JetBrains.Annotations;
using Meta;
using Meta.EventArgs;
using Model.Deity;
using Player.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Menu
{
    public class DeityEditorControl : MonoBehaviour
    {
        public Button mainMenuButton;
        public Button createDeity;
        public Button deleteDeity;
        public Button editDeity;

        public GameObject deityDetailPrefab;
        private DeityDetailControl _deityDetailControl;

        public GameObject deityEditPrefab;
        private DeityEditControl _deityEditControl;

        private DeityViewPortControl _deityViewPortControl;

        private GameObject _mainMenu;

        private DeityFactory _factory;

        private void Start()
        {
            _factory = DeityFactory.GetInstance(Application.persistentDataPath);
            if (_mainMenu == null)
                _mainMenu = GameObject.FindWithTag(Tags.MainMenuComponent);

            _deityDetailControl = Instantiate(deityDetailPrefab, transform).GetComponent<DeityDetailControl>();
            _deityDetailControl.gameObject.SetActive(false);

            _deityEditControl = Instantiate(deityEditPrefab, transform).GetComponent<DeityEditControl>();
            _deityEditControl.gameObject.SetActive(false);
            _deityEditControl.save.onClick.AddListener(Save);
            _deityEditControl.cancel.onClick.AddListener(Cancel);

            _deityViewPortControl = GetComponentInChildren<DeityViewPortControl>();
            UpdateViewPort(null, null);
            _factory.OnDeityListChange += UpdateViewPort;
            _factory.OnCurrentDeityChange += UpdateViews;

            mainMenuButton.onClick.AddListener(OnClickMainMenu);
            createDeity.onClick.AddListener(CreateDeity);
            deleteDeity.onClick.AddListener(DeleteDeity);
            editDeity.onClick.AddListener(EditDeity);
        }

        private void OnClickMainMenu()
        {
            gameObject.SetActive(false);
        }

        private void CreateDeity()
        {
            DeactivateDetailComponent();
            ActivateEditComponent();
            _factory.CurrentDeity = null;
        }

        private void DeleteDeity()
        {
            if (_factory.CurrentDeity == null || _factory.CurrentDeity.identifier == 0) return;
            if (!_factory.DeleteDeity()) return;
            DeactivateEditComponent();
            DeactivateDetailComponent();
        }

        private void Save()
        {
            var newName = _deityEditControl.GetName();
            if (newName == "") return;
            if (_factory.CurrentDeity == null)
            {
                _factory.CreateDeity(newName);
            }
            else
            {
                _factory.UpdateDeity(newName);
            }
            Cancel();
        }

        private void Cancel()
        {
            DeactivateEditComponent();
            if (_factory.CurrentDeity != null)
                ActivateDetailComponent();
        }

        private void EditDeity()
        {
            if (_factory.CurrentDeity == null) return;
            DeactivateDetailComponent();
            ActivateEditComponent();
        }

        private void UpdateViewPort(object sender, EventArgs args)
        {
            _deityViewPortControl.UpdateButtons(_factory, delegate
                {
                    if (!_deityEditControl.gameObject.activeInHierarchy)
                        ActivateDetailComponent();
                },
                delegate
                {
                    DeactivateDetailComponent();
                    DeactivateEditComponent();
                });
        }

        private void UpdateViews(object _, DeityEventArgs currentDeity)
        {
            if (currentDeity.Deity == null) return;
            _deityEditControl.UpdateValues(currentDeity.Deity);
            _deityDetailControl.UpdateValues(currentDeity.Deity);
        }

        private void ActivateEditComponent()
        {
            _deityEditControl.gameObject.SetActive(true);
        }

        private void DeactivateEditComponent()
        {
            _deityEditControl.gameObject.SetActive(false);
        }

        private void ActivateDetailComponent()
        {
            _deityDetailControl.gameObject.SetActive(true);
        }

        private void DeactivateDetailComponent()
        {
            _deityDetailControl.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            if (_mainMenu == null)
                _mainMenu = GameObject.FindWithTag(Tags.MainMenuComponent);
            _mainMenu.SetActive(true);
        }
    }
}