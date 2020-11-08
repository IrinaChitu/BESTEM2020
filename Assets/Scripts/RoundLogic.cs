using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Units
{
    public List<DragDrop.CharacterStats> allUnits;
    public List<DragDrop.CharacterStats> melee;

    public int meleeDamage;
    public int rangeDamage;
}

public class LandStats: MonoBehaviour
{
    public Units playerUnits = new Units { allUnits = new List<DragDrop.CharacterStats>(), melee = new List<DragDrop.CharacterStats>(), meleeDamage = 0, rangeDamage = 0 };
    public Units enemyUnits = new Units { allUnits = new List<DragDrop.CharacterStats>(), melee = new List<DragDrop.CharacterStats>(), meleeDamage = 0, rangeDamage = 0 };
    public bool frontline = false;
}

public class RoundLogic : MonoBehaviour
{
    List<GameObject> Lane = new List<GameObject>();

    // bool myTurn = false;

    void Start()
    {
        Lane.Add(GameObject.Find("PlayerCastle"));
        Lane.Add(GameObject.Find("LandPlayer1"));
        Lane.Add(GameObject.Find("LandPlayer2"));
        Lane.Add(GameObject.Find("LandEnemy2"));
        Lane.Add(GameObject.Find("LandEnemy1"));
        Lane.Add(GameObject.Find("EnemyCastle"));

        // query database for myTurn
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

    private void MeelsAttack(int land, int turn) // can it be one for both?
    {
        if (turn == 0)
        {
            LandStats landStats = Lane[land].GetComponent<LandStats>();
            int damage = landStats.playerUnits.meleeDamage;
            int i = 0;
            while (damage >=0)
            {
                int meleeHp = landStats.enemyUnits.melee[i].hpValue;
                if (damage > meleeHp)
                {
                    landStats.enemyUnits.melee[i].hpValue = 0;
                    damage -= meleeHp;
                    i++;
                }
                else
                {
                    landStats.enemyUnits.melee[i].hpValue -= damage;
                    break;
                }
            }
            landStats.enemyUnits.melee.Sort();
            landStats.enemyUnits.allUnits.Sort();
        }
        else
        {
            LandStats landStats = Lane[land].GetComponent<LandStats>();
            int damage = landStats.enemyUnits.meleeDamage;
            int i = 0;
            while (damage >= 0)
            {
                int meleeHp = landStats.playerUnits.melee[i].hpValue;
                if (damage > meleeHp)
                {
                    landStats.playerUnits.melee[i].hpValue = 0;
                    damage -= meleeHp;
                    i++;
                }
                else
                {
                    landStats.playerUnits.melee[i].hpValue -= damage;
                    break;
                }
            }
            landStats.playerUnits.melee.Sort();
            landStats.playerUnits.allUnits.Sort();
        }
    }

    private void RangeAttack(int land, int turn)
    {
        if (turn == 0)
        {
            LandStats landStats = Lane[land].GetComponent<LandStats>();
            int damage = landStats.playerUnits.rangeDamage;
            int i = 0;
            while (damage >= 0)
            {
                int unitHp = landStats.enemyUnits.allUnits[i].hpValue;
                if (damage > unitHp)
                {
                    landStats.enemyUnits.melee[i].hpValue = 0;
                    damage -= unitHp;
                    i++;
                }
                else
                {
                    landStats.enemyUnits.melee[i].hpValue -= damage;
                    break;
                }
            }
            landStats.enemyUnits.melee.Sort();
            landStats.enemyUnits.allUnits.Sort();

        }
        else
        {
            LandStats landStats = Lane[land].GetComponent<LandStats>();
            int damage = landStats.enemyUnits.rangeDamage;
            int i = 0;
            while (damage >= 0)
            {
                int unitHp = landStats.playerUnits.allUnits[i].hpValue;
                if (damage > unitHp)
                {
                    landStats.playerUnits.melee[i].hpValue = 0;
                    damage -= unitHp;
                    i++;
                }
                else
                {
                    landStats.playerUnits.melee[i].hpValue -= damage;
                    break;
                }
            }
            landStats.playerUnits.melee.Sort();
            landStats.playerUnits.allUnits.Sort();
        }
    }

    private int Fight(int land) // returns 1 if current player wins | -1 looses | 0 draw
    {
        // in which order?
        
        // first player
        MeelsAttack(land, 0); // probably sent who's turn it is
        RangeAttack(land, 0);

        // second player
        MeelsAttack(land, 1);
        RangeAttack(land, 1);

        // calculate battle results
        if (Lane[land].GetComponent<LandStats>().enemyUnits.allUnits.Count == 0)
        {
            if (Lane[land].GetComponent<LandStats>().playerUnits.allUnits.Count > 0)
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
                    Transform child = Lane[cell].transform.GetChild(i);
                    child.SetParent(Lane[cell+1].transform, false);
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

        Destroy(GameObject.Find("PlayerCastle").GetComponent<LandStats>());
        GameObject.Find("PlayerCastle").AddComponent<LandStats>();

        // block movement (besides spells?)
    }

}
