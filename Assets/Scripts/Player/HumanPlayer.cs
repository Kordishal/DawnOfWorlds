using Meta;
using Model.Deity;
using UnityEngine;

namespace Player
{
    public class HumanPlayer : BasePlayer
    {
        private void Awake()
        {
            Data = new PlayerData();
            DontDestroyOnLoad(gameObject);
            Data.Name = !PlayerPrefs.HasKey(PlayerPrefKeys.PlayerName) ? Default.PlayerName : PlayerPrefs.GetString(PlayerPrefKeys.PlayerName);
            if (Data.ActiveDeity == null)
            {
                Data.ActiveDeity = new Deity(0, "Odin", 0);
            }
        }
    }
}