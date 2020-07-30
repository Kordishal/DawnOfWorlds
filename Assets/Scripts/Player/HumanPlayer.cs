using Model.Deity;

namespace Player
{
    public class HumanPlayer : BasePlayer
    {
        private void Awake()
        {
            Data = new PlayerData();
            DontDestroyOnLoad(gameObject);
            Data.Name = ProfileSettings.Name;
            if (Data.ActiveDeity == null)
            {
                Data.ActiveDeity = new Deity(0, "Odin", 0);
            }
        }
    }
}