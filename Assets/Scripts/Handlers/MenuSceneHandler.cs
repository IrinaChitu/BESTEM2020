using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using Managers;

namespace Handlers
{
	public class MenuSceneHandler : MonoBehaviour
	{
	    [Header("Welcoming Message")]
	    public TextMeshProUGUI welcomingMessage;

	    private void Awake()
	    {
	        welcomingMessage.text = "Welcome " + MainManager.Instance.currentUserName + "!";
	    }

	    public void FindMatch()
	    {
	    	SceneManager.LoadScene("MatchmakingScene");
	    }

        public void BuildDeck()
        {
            SceneManager.LoadScene("CardsInventory");
        }
    }
}