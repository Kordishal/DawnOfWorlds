using System;
using Meta.EventArgs;
using Model.Deity;
using Player.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Menu
{
    public class DeityEditControl : MonoBehaviour
    {
        public Text identifier;
        public InputField nameInput;
        public Text powerPoints;

        public Button save;
        public Button cancel;

        private DeityFactory _factory;
        private void Start()
        {
            if (_factory == null) 
                _factory = DeityFactory.GetInstance(Application.persistentDataPath);
            _factory.OnCurrentDeityChange += ChangeCurrentDeity;
            save.onClick.AddListener(Save);
            cancel.onClick.AddListener(Cancel);
        }
        
        private void ChangeCurrentDeity(object _, ChangedCurrentDeityEventUpdate changedCurrentDeityEventUpdate)
        {
            var currentDeity = changedCurrentDeityEventUpdate.ChangedCurrentDeity;
            if (currentDeity != null)
            {
                UpdateValues(currentDeity);
            }
        }

        private void UpdateValues(Deity currentDeity)
        {
            if (currentDeity.identifier != 0)
            {
                identifier.text = currentDeity.identifier.ToString();
            }
            nameInput.text = currentDeity.name;
            powerPoints.text = currentDeity.currentPowerPoints.ToString();
        }

        private void Save()
        {
            var currentDeity = _factory.CurrentDeity;
            if (currentDeity == null) currentDeity = new Deity();
            if (nameInput.text ==  "") return;
            int.TryParse(identifier.text, out currentDeity.identifier);
            currentDeity.name = nameInput.text;
            int.TryParse(powerPoints.text, out currentDeity.currentPowerPoints);
            if (currentDeity.identifier == 0)
            { 
                _factory.CreateDeity(currentDeity.name);
            }
            else
            {
                _factory.UpdateDeity(currentDeity);
            }
            gameObject.SetActive(false);
        }

        private void Cancel()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (_factory != null && _factory.CurrentDeity != null)
                UpdateValues(_factory.CurrentDeity);
        }
    }
}