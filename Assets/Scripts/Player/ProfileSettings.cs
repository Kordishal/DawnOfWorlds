using Meta;
using Model.Geo.Features.Climate;
using UnityEngine;

namespace Player
{
    public class ProfileSettings
    {
        public static string Name
        {
            get => PlayerPrefs.GetString(PlayerPrefKeys.PlayerName, Default.PlayerName);
            set
            {
                if (value == "") return;
                // TODO: Actual player name checks.
                PlayerPrefs.SetString(PlayerPrefKeys.PlayerName, value);
                PlayerPrefs.Save();
            }
        }

        public static Climate DefaultClimate
        {
            get => (Climate) PlayerPrefs.GetInt(PlayerPrefKeys.DefaultClimate, (int) Climate.Temperate);
            set
            {
                PlayerPrefs.SetInt(PlayerPrefKeys.DefaultClimate, (int) value);
                PlayerPrefs.Save();
            }
        }
    }
}