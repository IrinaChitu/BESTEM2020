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

    public LandStats(bool frontline)
    {
        this.frontline = frontline;
    }
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

        Lane.Add(GameObject.Find("EnemyCastle"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();

        Lane.Add(GameObject.Find("LandEnemy1"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();

        Lane.Add(GameObject.Find("LandEnemy2"));
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

        // move units from Land1 to Land2
        for (int i = Lane[1].transform.childCount - 1; i >= 0; i--)
        {
            // advance only if all enemy units are dead
            if (Lane[1].GetComponent<LandStats>().enemyUnits.Count() == 0)
            {
                Lane[1].transform.GetChild(i).SetParent(Lane[2].transform, false);
            }
        }

        // move units from castle to Land1
        for (int i = Lane[0].transform.childCount - 1; i >= 0; i--)
        {
            // advance only if all enemy units are dead
            if (Lane[0].gameObject.GetComponent<LandStats>().enemyUnits.Count() == 0)
            {
                Lane[0].transform.GetChild(i).SetParent(Lane[1].transform, false);
            }
        }
    }

    public void OnClick()
    {
        MoveUnits();
        DrawCardFromDeck();
    }

}
