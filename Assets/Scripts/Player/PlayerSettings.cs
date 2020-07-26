using System;
using UnityEngine;

namespace Player
{
    public class PlayerSettings : MonoBehaviour
    {
        private const string DefaultName = "Player";
        

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                PlayerPrefs.SetString(PlayerPrefKeys.PlayerName, _name);
                PlayerPrefs.Save();
            }
        }

        private void Awake()
        {
            _name = !PlayerPrefs.HasKey(PlayerPrefKeys.PlayerName) ? DefaultName : PlayerPrefs.GetString(PlayerPrefKeys.PlayerName);
        }
    }
}
