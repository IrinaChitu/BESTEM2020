using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Units
{
    public int melee;
    public int range;
    public int Count()
    {
        return melee + range;
    }
}

public class LandStats: MonoBehaviour
{
    public Units playerUnits = new Units { melee = 0, range = 0 };
    public Units enemyUnits = new Units { melee = 0, range = 0 };
    public int totalHP = 0;
    public int totalDamage = 0;
    public bool frontline = false;
}

public class RoundLogic : MonoBehaviour
{
    List<GameObject> Lane = new List<GameObject>();

    void Start()
    {
        Lane.Add(GameObject.Find("PlayerCastle"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();
        
        Lane.Add(GameObject.Find("LandPlayer1"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();
        
        Lane.Add(GameObject.Find("LandPlayer2"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();
        //Lane[Lane.Count - 1].gameObject.GetComponent<LandStats>().frontline = true;

        Lane.Add(GameObject.Find("LandEnemy2"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();
      
        Lane.Add(GameObject.Find("LandEnemy1"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();
       
        Lane.Add(GameObject.Find("EnemyCastle"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();
    }


    private void DrawCardFromDeck()
    {
        GameObject PlayerDeck = GameObject.Find("PlayerDeck");
        GameObject PlayerArea = GameObject.Find("PlayerArea");

        // if there are card in the deck | hand cards limit
        if (PlayerDeck.transform.childCount > 0 && PlayerArea.transform.childCount < 10)
        {
            GameObject newCard = PlayerDeck.transform.GetChild(PlayerDeck.transform.childCount - 1).gameObject;
            newCard.transform.SetParent(PlayerArea.transform, false);
        }
    }

    private void Fight()
    {

    }

    private void MoveUnits() // up to frontline
    {
        for (int cell = Lane.Count-3; cell>=0; cell--)
        {
            // advance units from one land to another
            for (int i = Lane[cell].transform.childCount - 1; i >= 0; i--)
            {
                // advance only if all enemy units are dead
                if (Lane[cell].GetComponent<LandStats>().enemyUnits.Count() == 0)
                {
                    Lane[cell].transform.GetChild(i).SetParent(Lane[cell+1].transform, false);
                }
            }
        }

    }

    public void OnClick()
    {
        MoveUnits();
        DrawCardFromDeck();
    }

}
