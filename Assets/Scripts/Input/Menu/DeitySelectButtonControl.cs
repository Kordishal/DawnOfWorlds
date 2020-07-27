using Meta.EventArgs;
using Model.Deity;
using UnityEngine;
using UnityEngine.UI;

namespace Input.Menu
{
    public class DeitySelectButtonControl : MonoBehaviour
    {
        public Text nameText;
        public Text identifierText;
        public Button button;

        public void UpdateText(Deity deity)
        {
            nameText.text = deity.name;
            identifierText.text = deity.identifier.ToString();
        }

        private int GetIdentifier() => int.Parse(identifierText.text);

        public void ChangeCurrentDeity(object _, DeityEventArgs currentDeityArgs)
        {
            if (currentDeityArgs.Deity != null &&
                currentDeityArgs.Deity.identifier == GetIdentifier())
            {
                button.GetComponent<Image>().color = Color.red;
            }
            else
            {
                button.GetComponent<Image>().color = Color.white;
            }
        }
    }
}