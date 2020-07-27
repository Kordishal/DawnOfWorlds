using Model.Deity;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Menu
{
    public class DeityDetailControl : MonoBehaviour
    {
        public Text identifier;
        public Text nameValue;
        public Text powerPoints;

        public void UpdateValues(Deity currentDeity)
        {
            if (currentDeity.identifier != 0)
            {
                identifier.text = currentDeity.identifier.ToString();
            }

            nameValue.text = currentDeity.name;
            powerPoints.text = currentDeity.currentPowerPoints.ToString();
        }
    }
}