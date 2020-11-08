using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Serializables;

public struct Units
{
    public List<DragDrop.CharacterStats> allUnits;
    public List<DragDrop.CharacterStats> melee;

    public int meleeDamage;
    public int rangeDamage;
}

public class LandStats: MonoBehaviour
{
    public bool player = true; // false => enemy
    public Units units = new Units { allUnits = new List<DragDrop.CharacterStats>(), melee = new List<DragDrop.CharacterStats>(), meleeDamage = 0, rangeDamage = 0 };
    public bool frontline = false;
}

public class RoundLogic : MonoBehaviour
{
    List<GameObject> Lane = new List<GameObject>();

    public static bool myTurn = false;

    void Start()
    {
        Lane.Add(GameObject.Find("PlayerCastle"));
        Lane.Add(GameObject.Find("LandPlayer1"));
        Lane.Add(GameObject.Find("LandPlayer2"));
        Lane.Add(GameObject.Find("LandEnemy2"));
        Lane.Add(GameObject.Find("LandEnemy1"));
        Lane.Add(GameObject.Find("EnemyCastle"));

        // query database for myTurn
        var firstPlayerId = MainManager.Instance.gameManager.GetFirstPlayerId();

        if (firstPlayerId == MainManager.Instance.currentUserId)
        {
            myTurn = true;
        }
        else
        {
            myTurn = false;
        }
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

    public void SendToGrave(int land)
    {
        for (int i=0; i< Lane[land].gameObject.transform.childCount; i++)
        {
            Transform child = Lane[land].gameObject.transform.GetChild(i);
            if (child.gameObject.GetComponent<DragDrop.CharacterStats>().hpValue == 0)
            {
                if(Lane[land].GetComponent<LandStats>().player == true)
                {
                    child.SetParent(GameObject.Find("PlayerGraveyard").transform, false);

                } else
                {
                    child.SetParent(GameObject.Find("EnemyGraveyard").transform, false);
                }
            }
        }
            
    }

    private bool SameUnits(DragDrop.CharacterStats a, DragDrop.CharacterStats b)
    {
        return a.type == b.type && a.hpValue == b.hpValue && a.dmgValue == b.dmgValue;
    }

    private void MeleesPhase(int land)
    {

        LandStats landPlayer = Lane[land-1].GetComponent<LandStats>();
        LandStats landEnemy = Lane[land].GetComponent<LandStats>();
        int damagePlayer = landPlayer.units.meleeDamage;
        int damageEnemy = landEnemy.units.meleeDamage;

        landPlayer.units.melee.Sort((p1, p2) => p2.hpValue.CompareTo(p1.hpValue));
        landPlayer.units.melee.Reverse();

        landEnemy.units.melee.Sort((p1, p2) => p2.hpValue.CompareTo(p1.hpValue));
        landEnemy.units.melee.Reverse();

        //Debug.Log($"landPlayer: {landPlayer.player}, landEnemy: {landEnemy.player}, damagePlayer: {damagePlayer}, damageEnemy: {damageEnemy}");
        //for (int i = 0; i < landPlayer.units.melee.Count; i++)
        //{
        //    Debug.Log($"dmg{i}: {landPlayer.units.melee[i].dmgValue}, hp{i}: {landPlayer.units.melee[i].hpValue}");
        //}
        //for (int i = 0; i < landEnemy.units.melee.Count; i++)
        //{
        //    Debug.Log($"dmg{i}: {landEnemy.units.melee[i].dmgValue}, hp{i}: {landEnemy.units.melee[i].hpValue}");
        //}

        for (int i=0; i<landEnemy.units.melee.Count; i++)
        {
            if (damagePlayer >= landEnemy.units.melee[i].hpValue)
            {
                damagePlayer -= landEnemy.units.melee[i].hpValue;
                landEnemy.units.melee[i].hpValue = 0;

                for (int j=0; j<landEnemy.units.allUnits.Count; j++)
                {
                    if (SameUnits(landEnemy.units.allUnits[j], landEnemy.units.melee[i]))
                    {
                        SendToGrave(land);
                        landEnemy.units.allUnits.RemoveAt(j); // ToDo: send card to graveyard
                        break;
                    }
                }
                landEnemy.units.meleeDamage -= landEnemy.units.melee[i].dmgValue;
                landEnemy.units.melee.RemoveAt(i);
                i--;
            } 
            else
            {
                for (int j = 0; j < landEnemy.units.allUnits.Count; j++)
                {
                    if (SameUnits(landEnemy.units.allUnits[j], landEnemy.units.melee[i]))
                    {
                        landEnemy.units.allUnits[j].hpValue -= damagePlayer;
                        break;
                    }
                }
                damagePlayer = 0;
            }
        }
 
        for (int i = 0; i < landPlayer.units.melee.Count; i++)
        {
            if (damageEnemy >= landPlayer.units.melee[i].hpValue)
            {
                damageEnemy -= landPlayer.units.melee[i].hpValue;
                landPlayer.units.melee[i].hpValue = 0;

                for (int j = 0; j < landPlayer.units.allUnits.Count; j++)
                {
                    if (SameUnits(landPlayer.units.allUnits[j], landPlayer.units.melee[i]))
                    {
                        SendToGrave(land-1);
                        landPlayer.units.allUnits.RemoveAt(j); // ToDo: send card to graveyard
                        break;
                    }
                }
                landPlayer.units.meleeDamage -= landPlayer.units.melee[i].dmgValue;
                landPlayer.units.melee.RemoveAt(i);
                i--;
            }
            else
            {
                for (int j = 0; j < landPlayer.units.allUnits.Count; j++)
                {
                    if (SameUnits(landPlayer.units.allUnits[j], landPlayer.units.melee[i]))
                    {
                        landPlayer.units.allUnits[j].hpValue -= damageEnemy;
                        break;
                    }
                }
                damageEnemy = 0;
            }
        }

        //Debug.Log($"LandEnemy: {landEnemy.units.melee.Count} ; Lane: {Lane[land].GetComponent<LandStats>().units.melee.Count}");
        //Debug.Log($"Component: {GameObject.Find("LandEnemy2").GetComponent<LandStats>().units.melee.Count}");
        //Debug.Log($"landPlayer: {landPlayer.player}, landEnemy: {landEnemy.player}, damagePlayer: {damagePlayer}, damageEnemy: {damageEnemy}");
        //for (int i = 0; i < landPlayer.units.melee.Count; i++)
        //{
        //    Debug.Log($"dmg{i}: {landPlayer.units.melee[i].dmgValue}, hp{i}: {landPlayer.units.melee[i].hpValue}");
        //}
        //for (int i = 0; i < landEnemy.units.melee.Count; i++)
        //{
        //    Debug.Log($"dmg{i}: {landEnemy.units.melee[i].dmgValue}, hp{i}: {landEnemy.units.melee[i].hpValue}");
        //}
        //UpdateComponent(land-1, landPlayer.units);

    }

    private void RangeAttack(int land)
    {
        LandStats landPlayer = Lane[land - 1].GetComponent<LandStats>();
        LandStats landEnemy = Lane[land].GetComponent<LandStats>();
        int damagePlayer = landPlayer.units.rangeDamage;
        int damageEnemy = landEnemy.units.rangeDamage;

        landPlayer.units.allUnits.Sort((p1, p2) => p2.hpValue.CompareTo(p1.hpValue));
        landPlayer.units.allUnits.Reverse();

        landEnemy.units.allUnits.Sort((p1, p2) => p2.hpValue.CompareTo(p1.hpValue));
        landEnemy.units.allUnits.Reverse();

        for (int i = 0; i < landEnemy.units.allUnits.Count; i++)
        {
            if (damagePlayer >= landEnemy.units.allUnits[i].hpValue)
            {
                damagePlayer -= landEnemy.units.allUnits[i].hpValue;
                landEnemy.units.allUnits[i].hpValue = 0;

                for (int j = 0; j < landEnemy.units.melee.Count; j++)
                {
                    if (SameUnits(landEnemy.units.allUnits[i], landEnemy.units.melee[j]))
                    {
                        landEnemy.units.meleeDamage -= landEnemy.units.melee[j].dmgValue;
                        SendToGrave(land);
                        landEnemy.units.melee.RemoveAt(j); // ToDo: send card to graveyard
                        break;
                    }
                }

                if (landEnemy.units.allUnits[i].type.Equals("range"))
                {
                    landEnemy.units.rangeDamage -= landEnemy.units.allUnits[i].dmgValue;
                }
                landEnemy.units.allUnits.RemoveAt(i);
                i--;
            }
            else
            {
                landEnemy.units.allUnits[i].hpValue -= damagePlayer;
                //for (int j = 0; j < landEnemy.units.melee.Count; j++)
                //{
                //    if (landEnemy.units.allUnits[i] == landEnemy.units.melee[j])
                //    {
                //        landEnemy.units.melee[j].hpValue -= damagePlayer;
                //        break;
                //    }
                //}
                damagePlayer = 0;
            }
        }
        //UpdateComponent(land, landEnemy.units);


        for (int i = 0; i < landPlayer.units.allUnits.Count; i++)
        {
            if (damageEnemy >= landPlayer.units.allUnits[i].hpValue)
            {
                damageEnemy -= landPlayer.units.allUnits[i].hpValue;
                landPlayer.units.allUnits[i].hpValue = 0;

                for (int j = 0; j < landPlayer.units.melee.Count; j++)
                {
                    if (SameUnits(landPlayer.units.allUnits[i], landPlayer.units.melee[j]))
                    {
                        landPlayer.units.meleeDamage -= landPlayer.units.melee[j].dmgValue;
                        SendToGrave(land - 1);
                        landPlayer.units.melee.RemoveAt(j); // ToDo: send card to graveyard
                        //Lane[land-1].GetComponent<LandStats>().units.melee.RemoveAt(j);
                        break;
                    }
                }

                if (landPlayer.units.allUnits[i].type.Equals("range"))
                {
                    landPlayer.units.rangeDamage -= landPlayer.units.allUnits[i].dmgValue;
                }
                landPlayer.units.allUnits.RemoveAt(i);
                //Lane[land - 1].GetComponent<LandStats>().units.allUnits.RemoveAt(i);
                i--;
            }
            else
            {
                landPlayer.units.allUnits[i].hpValue -= damageEnemy;
                //for (int j = 0; j < landPlayer.units.melee.Count; j++)
                //{
                //    if (landPlayer.units.allUnits[i] == landPlayer.units.melee[j])
                //    {
                //        landPlayer.units.melee[j].hpValue -= damageEnemy;
                //        break;
                //    }
                //}
                damageEnemy = 0;
            }
        }
        //UpdateComponent(land-1, landPlayer.units);

    }

    private int Fight(int land) // returns 1 if current player wins | -1 looses | 0 draw
    {

        if (Lane[land - 1].GetComponent<LandStats>().player == true && Lane[land - 1].GetComponent<LandStats>().units.allUnits.Count != 0 &&
            Lane[land].GetComponent<LandStats>().player == false && Lane[land].GetComponent<LandStats>().units.allUnits.Count != 0)
        {
            MeleesPhase(land);
            // RangeAttack(land);

            // calculate battle results
            if (Lane[land].GetComponent<LandStats>().units.allUnits.Count == 0)
            {
                if (Lane[land - 1].GetComponent<LandStats>().units.allUnits.Count > 0)
                {
                    return 1;
                } else
                {
                    return 0;
                }
            }
        }
        return -1;
    }

    private void UpdateComponent(int cell, Units units)
    {
        LandStats prevLandStats = Lane[cell].GetComponent<LandStats>();
        Destroy(Lane[cell].GetComponent<LandStats>());
        LandStats curLandStatsLane = Lane[cell].AddComponent<LandStats>();
        curLandStatsLane.player = prevLandStats.player;
        curLandStatsLane.units = prevLandStats.units;
        curLandStatsLane.frontline = prevLandStats.frontline;
    }

    private void CopyComponent(int cell)
    {
        LandStats prevLandStats = Lane[cell - 1].GetComponent<LandStats>();
        Destroy(Lane[cell].GetComponent<LandStats>());
        LandStats curLandStatsLane = Lane[cell].AddComponent<LandStats>();
        curLandStatsLane.player = prevLandStats.player;
        curLandStatsLane.units = prevLandStats.units;
        curLandStatsLane.frontline = prevLandStats.frontline;
    }

    private void MoveUnits() // up to frontline
    {

        for (int cell = Lane.Count-2; cell > 0; cell--)
        {
            // advance only if next land is mine  or all enemy units are dead or go to fight
            if ((Lane[cell - 1].GetComponent<LandStats>().player && (Lane[cell].GetComponent<LandStats>().units.allUnits.Count == 0 || Lane[cell].GetComponent<LandStats>().player)) || Fight(cell) == 1)
            {
                Debug.Log("Ce puii mei e: " + cell.ToString());
                Debug.Log(Lane[cell].GetComponent<LandStats>().player);
                Debug.Log(Lane[cell].GetComponent<LandStats>().units.allUnits.Count);
                Debug.Log("MEELS");
                // advance all units from one land to another
                if (cell > 0)
                {
                    for (int i = Lane[cell - 1].transform.childCount - 1; i >= 0; i--)
                    {
                        Transform child = Lane[cell - 1].transform.GetChild(i);
                        child.SetParent(Lane[cell].transform, false);
                    }

                    CopyComponent(cell);

                }
            }
        }
        Debug.Log("MARIAN@");
    }

    private void AttackCastle()
    {
        int totalDamage = Lane[Lane.Count - 2].GetComponent<LandStats>().units.meleeDamage + Lane[Lane.Count - 2].GetComponent<LandStats>().units.rangeDamage;

        string HP = GameObject.Find("EnemyHero").GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
        int heroHP = Convert.ToInt32(HP.Split(' ')[1]);

        if (heroHP > totalDamage)
        {
            heroHP -= totalDamage;
            GameObject.Find("EnemyHero").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = $"HP: {heroHP} / TotalHP: 15"; // TBD
        }

    }

    public void OnClick()
    {
        if (myTurn)
        {
            MoveUnits();
            if (Lane[Lane.Count - 2].GetComponent<LandStats>().player && Lane[Lane.Count - 2].GetComponent<LandStats>().units.allUnits.Count != 0)
            {
                AttackCastle();
            }
            DrawCardFromDeck();

            Destroy(GameObject.Find("PlayerCastle").GetComponent<LandStats>());
            GameObject.Find("PlayerCastle").AddComponent<LandStats>();

            MainManager.Instance.gameManager.SendMove(new Move { userId = MainManager.Instance.currentUserId, command = "ENDTURN", type="", cardId=0 });
            myTurn = false;
        }

        // block movement (besides spells?)
    }

}
