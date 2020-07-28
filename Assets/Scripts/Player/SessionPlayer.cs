using System;
using Meta;
using Model.Deity;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    public class SessionPlayer : BasePlayer
    {
        
        private void Start()
        {
            Data = new PlayerData();
            DontDestroyOnLoad(gameObject);
            Data.Name = "Session Player 1";
            Data.ActiveDeity = new Deity(2, "Freya", 0);
        }



    }
}
