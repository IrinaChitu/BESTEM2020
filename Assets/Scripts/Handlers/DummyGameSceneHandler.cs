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
	        }
			MainManager.Instance.gameManager.ListenForMoves(move =>
			{
				textInput.text = move.command;
				myTurn = true;
			});
		}

	    void OnDestroy()
	    {
			MainManager.Instance.gameManager.StopListenForMoves();
		}

	    public void ButtonPressed()
	    {
			// asta ar trebui sa se intample la end turn
	    	var randomMessage = RandomString(5);
	    	if (myTurn)
	    	{
	    		MainManager.Instance.gameManager.SendMove(new Move{userId = MainManager.Instance.currentUserId, command = randomMessage});
				myTurn = false;
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