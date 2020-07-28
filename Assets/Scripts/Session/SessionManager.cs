using System.Collections.Generic;
using Meta;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Session
{
    public class SessionManager : MonoBehaviour
    {
        public Button endTurn;
        public GameObject sessionPlayerPrefab;
        public GameObject humanPlayerPrefab;

        private IPlayer _currentPlayer;
        private Queue<IPlayer> _players;

        private void Start()
        {
            _players = new Queue<IPlayer>();
            var humanPlayer = GameObject.FindWithTag(Tags.Player);
            if (humanPlayer == null)
            {
                humanPlayer = Instantiate(humanPlayerPrefab);
                humanPlayer.name = "Human Player (Generated)";
            }
            _currentPlayer = humanPlayer.GetComponent<HumanPlayer>();
            var otherPlayer = Instantiate(sessionPlayerPrefab);
            otherPlayer.name = "Session Player (Other)";
            _players.Enqueue(otherPlayer.GetComponent<SessionPlayer>());
            endTurn.onClick.AddListener(EndTurn);
        }


        private void EndTurn()
        {
            _players.Enqueue(_currentPlayer);
            _currentPlayer = _players.Dequeue();
            _currentPlayer.RefreshPowerPoints();
        }
    }
}