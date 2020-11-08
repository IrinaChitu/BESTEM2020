using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardElementScript : MonoBehaviour
{
    public TMPro.TextMeshProUGUI username;
    public TMPro.TextMeshProUGUI wins;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUserAndWins(string username, int wins)
    {
        this.username.text = username;
        this.wins.text = wins.ToString();
    }
}
