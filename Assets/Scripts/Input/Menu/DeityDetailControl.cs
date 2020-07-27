using JetBrains.Annotations;
using Meta.EventArgs;
using Model.Deity;
using Player.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Menu
{
    public class DeityDetailControl : MonoBehaviour
    {
        private DeityFactory _factory;

        public Text identifier;
        public Text nameValue;
        public Text powerPoints;

        private void Start()
        {
            if (_factory == null)
                _factory = DeityFactory.GetInstance(Application.persistentDataPath);
            _factory.OnCurrentDeityChange += ChangeCurrentDeity;
        }

        private void ChangeCurrentDeity(object sender, ChangedCurrentDeityEventUpdate changedCurrentDeityEventUpdate)
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

            nameValue.text = currentDeity.name;
            powerPoints.text = currentDeity.currentPowerPoints.ToString();
        }

        private void OnEnable()
        {
            if (_factory == null)
                _factory = DeityFactory.GetInstance(Application.persistentDataPath);
            if (_factory.CurrentDeity != null)
                UpdateValues(_factory.CurrentDeity);
        }
    }
}