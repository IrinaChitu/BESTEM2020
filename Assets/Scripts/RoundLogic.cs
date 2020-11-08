using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Units
{
    public List<DragDrop.CharacterStats> allUnits;
    public List<DragDrop.CharacterStats> melee;
    public List<DragDrop.CharacterStats> range;
}

public class LandStats: MonoBehaviour
{
    public Units playerUnits = new Units { allUnits = new List<DragDrop.CharacterStats>(), melee = new List<DragDrop.CharacterStats>(), range = new List<DragDrop.CharacterStats>() };
    public Units enemyUnits = new Units { allUnits = new List<DragDrop.CharacterStats>(), melee = new List<DragDrop.CharacterStats>(), range = new List<DragDrop.CharacterStats>() };
    public int totalHP = 0;
    public int totalDamage = 0;
    public bool frontline = false;
}

public class RoundLogic : MonoBehaviour
{
    List<GameObject> Lane = new List<GameObject>();

    // bool myTurn = false;

    void Start()
    {
        Lane.Add(GameObject.Find("PlayerCastle"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();
        
        Lane.Add(GameObject.Find("LandPlayer1"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();
        
        Lane.Add(GameObject.Find("LandPlayer2"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();
        Lane[Lane.Count - 1].gameObject.GetComponent<LandStats>().frontline = true;

        Lane.Add(GameObject.Find("LandEnemy2"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();
      
        Lane.Add(GameObject.Find("LandEnemy1"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();
       
        Lane.Add(GameObject.Find("EnemyCastle"));
        Lane[Lane.Count - 1].AddComponent<LandStats>();

        PopulateLandStats();
        // query database for myTurn
    }

    private void PopulateLandStats()
    {
        for (int cell = Lane.Count - 3; cell >= 0; cell--)
        {
            for (int i = Lane[cell].transform.childCount - 1; i >= 0; i--)
            {
                //Lane[cell].transform.GetChild(i).GetComponent<LandStats>().playerUnits.Add();


            }
        }

        //Lane[land].GetComponent<LandStats>().playerUnits.allUnits.Sort()

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

    private void MeelsAttack()
    {
        //Lane[land].GetComponent<LandStats>().playerUnits.melee[0]

    }

    private void RangeAttack()
    {

    }

    private int Fight(int land) // returns 1 if current player wins | -1 looses | 0 draw
    {
        // in which order?
        
        // first player
        MeelsAttack();
        RangeAttack();

        // second player

        // calculate battle results
        if (Lane[land].GetComponent<LandStats>().enemyUnits.allUnits.Count == 0)
        {
            if (Lane[land].GetComponent<LandStats>().playerUnits.allUnits.Count != 0)
            {
                return 1;
            } else
            {
                return 0;
            }
        }
        return -1;
    }

    private void MoveUnits() // up to frontline
    {

        for (int cell = Lane.Count-3; cell >= 0; cell--)
        {
            // advance only if all enemy units are dead
            if (Lane[cell].GetComponent<LandStats>().enemyUnits.allUnits.Count == 0 || Fight(cell) == 1)
            {
                // advance all units from one land to another
                for (int i = Lane[cell].transform.childCount - 1; i >= 0; i--)
                {
                    Lane[cell].transform.GetChild(i).SetParent(Lane[cell+1].transform, false);
                }
            }

        }

    }

    private void AttackCastle()
    {

    }

    public void OnClick()
    {
        // if it's my turn {...}

        MoveUnits();
        if(Lane[Lane.Count-2].GetComponent<LandStats>().playerUnits.allUnits.Count != 0)
        {
            AttackCastle();
        }
        DrawCardFromDeck();

        // block movement (besides spells?)
    }

}
