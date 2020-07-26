using UnityEngine;
using UnityEngine.UI;

namespace Input.Menu
{
    public class DeityDetailControl : MonoBehaviour
    {
        public DeityEditorControl parent;

        public Text identifier;
        public Text nameValue;
        public Text powerPoints;

        private void Start()
        {
            parent = GetComponentInParent<DeityEditorControl>();
        }

        public void ChangeCurrentDeity()
        {
            if (parent.currentDeity == null) return;
            if (parent.currentDeity.identifier != 0)
            {
                identifier.text = parent.currentDeity.identifier.ToString();
            }

            nameValue.text = parent.currentDeity.name;
            powerPoints.text = parent.currentDeity.currentPowerPoints.ToString();
        }

        private void OnEnable()
        {
            if (parent == null)
                parent = GetComponentInParent<DeityEditorControl>();
            ChangeCurrentDeity();
        }
    }
}