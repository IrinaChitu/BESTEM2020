using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

using Serializables;
using Managers;

namespace Handlers
{
	public class DummyGameSceneHandler : MonoBehaviour
	{
		private static System.Random random = new System.Random();

		public TextMeshProUGUI textInput;

		public bool myTurn;
	    
	    // Start is called before the first frame update
	    void Start()
	    {
	        var firstPlayerId = MainManager.Instance.gameManager.GetFirstPlayerId();

	        if (firstPlayerId == MainManager.Instance.currentUserId)
	        {
	        	textInput.text = "You are first!";
	        	myTurn = true;
	        }
	        else
	        {
	        	textInput.text = "You are second!";
	        	myTurn = false;

	         	MainManager.Instance.gameManager.ListenForMoves(move => Debug.Log(move.message) );
	        }
	    }

	    // Update is called once per frame
	    void Update()
	    {
	        
	    }

	    public void ButtonPressed()
	    {
	    	var randomMessage = RandomString(5);
	    	if (myTurn)
	    	{
	    		MainManager.Instance.gameManager.SendMove(new Move{message = randomMessage});
	    	}
	    }

		public static string RandomString(int length)
		{
		    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		    return new string(Enumerable.Repeat(chars, length)
		      .Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}