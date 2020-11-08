using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using Managers;

namespace Handlers
{
	public class LeaderboardSceneHandler : MonoBehaviour
	{
		private void Awake()
		{
			Debug.Log("Hello leaderboard!");
		}

		public void GoToMainMenu()
        {
			SceneManager.LoadScene("Menu");
		}
	}
}