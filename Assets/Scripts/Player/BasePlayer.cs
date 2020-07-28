using Model.Deity;
using UnityEngine;

namespace Player
{
    public abstract class BasePlayer : MonoBehaviour, IPlayer
    {
        
        protected PlayerData Data;
        public string Name
        {
            get => Data.Name;
            set
            {
                if (Data == null) Data = new PlayerData();
                if (value != Data.Name) return;
                Data.Name = value;
                PlayerPrefs.SetString(PlayerPrefKeys.PlayerName, Data.Name);
                PlayerPrefs.Save();
            }
        }

        public Deity ActiveDeity
        {
            get => Data.ActiveDeity;
            set
            {
                if (Data == null) Data = new PlayerData();
                if (value != Data.ActiveDeity) return;
                Data.ActiveDeity = value;
            }
        } 
        
        public virtual void RefreshPowerPoints()
        {
            var points = Random.Range(1, 7) + Random.Range(1, 7);
            Data.ActiveDeity.currentPowerPoints += points;
            Debug.Log("Player " + Data.ActiveDeity.name + " has gained " + points + " Power Points and has a total of " + Data.ActiveDeity.currentPowerPoints + " Points.");
        }
        
    }
}