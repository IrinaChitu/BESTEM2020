using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inits : MonoBehaviour
{
    // to be deleted
    public GameObject Character;

    void Start()
    {
        Destroy(GameObject.Find("LandEnemy2").GetComponent<LandStats>());

        CreateLandStatsComponents();

        DragDrop.CharacterStats ePawn1 = CreateEnemyPawn("LandEnemy2");
        DragDrop.CharacterStats ePawn2 = CreateEnemyPawn("LandEnemy2");

        LandStats landEnemy2 = GameObject.Find("LandEnemy2").GetComponent<LandStats>();
        landEnemy2.units.allUnits.Add(ePawn1);
        landEnemy2.units.melee.Add(ePawn1);
        landEnemy2.units.meleeDamage += ePawn1.dmgValue;
        landEnemy2.player = false;

        LandStats landEnemy1 = GameObject.Find("LandEnemy2").GetComponent<LandStats>();
        landEnemy1.units.allUnits.Add(ePawn2);
        landEnemy1.units.melee.Add(ePawn2);
        landEnemy1.units.meleeDamage += ePawn2.dmgValue;
        landEnemy1.player = false;

    }

    public void CreateLandStatsComponents()
    {
        GameObject.Find("PlayerCastle").AddComponent<LandStats>();
        GameObject.Find("LandPlayer1").AddComponent<LandStats>();
        GameObject.Find("LandPlayer2").AddComponent<LandStats>();
        GameObject.Find("LandPlayer2").GetComponent<LandStats>().frontline = true;
        GameObject.Find("LandEnemy2").AddComponent<LandStats>();
        GameObject.Find("LandEnemy1").AddComponent<LandStats>();
        GameObject.Find("EnemyCastle").AddComponent<LandStats>();
    }

    public DragDrop.CharacterStats CreateEnemyPawn(string land)
    {
        GameObject pawn = Instantiate(Character, new Vector2(0, 0), Quaternion.identity);

        // get these fields from current card
        pawn.GetComponent<Image>().sprite = Resources.Load<Sprite>("Characters/AsuriSplash");

        pawn.AddComponent<DragDrop.CharacterStats>();

        DragDrop.CharacterStats characterStats = pawn.GetComponent<DragDrop.CharacterStats>();
        characterStats.ComponentConstructor("melee", 6, 6);

        pawn.transform.SetParent(GameObject.Find(land).transform, false);
        Destroy(this.gameObject);



        return characterStats;


    }

}
