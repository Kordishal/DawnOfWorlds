using System;
using Meta;
using Model.Deity;
using UnityEngine;

namespace Player
{
    public class PlayerSettings : MonoBehaviour
    {
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
        
        public Deity selectedDeity;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _name = !PlayerPrefs.HasKey(PlayerPrefKeys.PlayerName) ? Default.PlayerName : PlayerPrefs.GetString(PlayerPrefKeys.PlayerName);
        }
    }
}
