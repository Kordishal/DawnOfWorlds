using Meta;
using Model.Deity;
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

        public string GetName()
        {
            return nameInput.text;
        }

        public void UpdateValues(Deity currentDeity)
        {
            if (currentDeity == null)
            {
                identifier.text = 0.ToString();
                nameInput.text = "";
                powerPoints.text = 0.ToString();
            }
            else
            {
                identifier.text = currentDeity.identifier.ToString();
                nameInput.text = currentDeity.name;
                powerPoints.text = currentDeity.currentPowerPoints.ToString();
            }
        }
    }
}