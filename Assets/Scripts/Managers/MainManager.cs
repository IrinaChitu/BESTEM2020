using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using APIs;

namespace Managers
{
    public class MainManager : MonoBehaviour
    {
        public static MainManager Instance;

        public FirebaseManager firebaseManager;
        
        public string currentUserName;
        public string currentUserId;
        public MatchmakingManager matchmakingManager;
        public GameManager gameManager;

        // public string currentLocalPlayerId; // You can use Firebase Auth to turn this into a userId. Just using the player name for a player id as an example for now!

        private void Awake() => Instance = this;

        private void Start()
        {
        	firebaseManager = GetComponent<FirebaseManager>();
            matchmakingManager = GetComponent<MatchmakingManager>();
            gameManager = GetComponent<GameManager>();
        }
    }
}
