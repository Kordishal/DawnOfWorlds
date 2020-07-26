using Model.Deity;
using Player.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Menu
{
    public class DeityEditControl : MonoBehaviour
    {
        public DeityEditorControl parent;

        public Text identifier;
        public InputField nameInput;
        public Text powerPoints;

        public Button save;
        public Button cancel;

        private DeityFactory _factory;
        private void Start()
        {
            parent = GetComponentInParent<DeityEditorControl>();
            _factory = DeityFactory.GetInstance(Application.persistentDataPath);
            save.onClick.AddListener(Save);
            cancel.onClick.AddListener(Cancel);
        }

        public void ChangeCurrentDeity()
        {
            if (parent.currentDeity == null) parent.currentDeity = new Deity();
            if (parent.currentDeity.identifier != 0)
            {
                identifier.text = parent.currentDeity.identifier.ToString();
            }

            nameInput.text = parent.currentDeity.name;
            powerPoints.text = parent.currentDeity.currentPowerPoints.ToString();
        }

        private void Save()
        {
            // TODO: Add message that you can't save without a name!
            if (parent.currentDeity == null) parent.currentDeity = new Deity();
            if (nameInput.text ==  "") return;
            int.TryParse(identifier.text, out parent.currentDeity.identifier);
            parent.currentDeity.name = nameInput.text;
            int.TryParse(powerPoints.text, out parent.currentDeity.currentPowerPoints);
            if (parent.currentDeity.identifier == 0)
            { 
                parent.currentDeity = _factory.CreateDeity(parent.currentDeity.name);
            }
            else
            {
                _factory.UpdateDeity(parent.currentDeity);
            }
            gameObject.SetActive(false);
            parent.deityDetail.SetActive(true);
        }

        private void Cancel()
        {
            gameObject.SetActive(false);
            if (parent.currentDeity != null && parent.currentDeity.identifier != 0)
            {
                parent.deityDetail.SetActive(true);
            }
        }

        private void OnEnable()
        {
            if (parent != null)
            {
                ChangeCurrentDeity();
            }
        }
    }
}