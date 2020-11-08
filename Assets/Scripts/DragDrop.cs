using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Managers;
using Serializables;

public class Response
{
    public string userId;
    public string command;
    public string type;
    public int cardId;
}

public class DragDrop : MonoBehaviour, IPointerClickHandler
{
    public GameObject Canvas;
    public GameObject Character;
    
    private bool isDragging = false;
    private bool isOverDropZone = false;
    private GameObject dropZone;
    private GameObject startParent;
    private Vector2 startPosition;

    private void Start()
    {
        Canvas = GameObject.Find("Main Canvas");

        MainManager.Instance.gameManager.ListenForMoves(move =>
        {

            switch (move.command)
            {
                case "ENDTURN":
                    RoundLogic.myTurn = true;
                    break;

                case "ADD":
                    if(move.type.Equals("UNIT"))
                    {
                        GameObject castleZone = GameObject.Find("EnemyCastle");

                        // based on move.cardId
                        UpdateCastleStats(castleZone, CreateCharacterFromCard());
                    }
                    else // "SPELL"
                    {
                        // generare card cu date din db

                        // GameObject.Find("EnemySpellAre").transform.SetParent(card, false);
                    }
                    break;


                case "TAP":
                    GameObject enemyZone = GameObject.Find("EnemySpellAre");
                    for (int i = 0; i < enemyZone.transform.childCount; i++)
                    {
                        Transform child = enemyZone.transform.GetChild(i);
                        if (child.gameObject.GetComponent<CardScript>().card.dbID.Equals(move.cardId))
                        {
                            child.transform.Rotate(0, 0, 90);
                            break;
                        }
                    }
                    break;
            }

            Debug.Log(move.command);
        });
    }

    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverDropZone = true;
        dropZone = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        dropZone = null;

    }

    public void StartDrag()
    {
        startParent = transform.parent.gameObject;
        startPosition = transform.position;
        isDragging = true;
    }

    public class CharacterStats : MonoBehaviour
    {
        public string type;
        public int hpValue;
        public int dmgValue;

        public void ComponentConstructor(string type, int hpValue, int dmgValue)
        {
            this.type = type;
            this.hpValue = hpValue;
            this.dmgValue = dmgValue;
        }
    }


    public CharacterStats CreateCharacterFromCard()
    {
        GameObject pawn = Instantiate(Character, new Vector2(0, 0), Quaternion.identity);

        Card card = this.GetComponent<CardScript>().card;
        pawn.GetComponent<Image>().sprite = card.sprite;

        pawn.AddComponent<CharacterStats>();

        CharacterStats characterStats = pawn.GetComponent<CharacterStats>();
        characterStats.ComponentConstructor(card.details, card.hpValue, card.dmgValue);

        pawn.transform.SetParent(dropZone.transform, false);

        // add unit
        MainManager.Instance.gameManager.SendMove(new Move { userId = MainManager.Instance.currentUserId, command = "ADD", type="UNIT", cardId=1234 });

        Debug.Log("send move");

        Destroy(this.gameObject);

        return characterStats;
    }


    private void UpdateCastleStats(GameObject playerCastle, CharacterStats characterStats)
    {
        LandStats castleStats = playerCastle.GetComponent<LandStats>();

        if (characterStats.type.Equals("melee"))
        {
            castleStats.units.melee.Add(characterStats);
            castleStats.units.meleeDamage += characterStats.dmgValue;
        }
        else if (characterStats.type.Equals("range"))
        {
            castleStats.units.rangeDamage += characterStats.dmgValue;
        }
        castleStats.units.allUnits.Add(characterStats);
    }

    public void EndDrag()
    {
        isDragging = false;
        if (isOverDropZone)
        {
            if (dropZone.name.Equals("PlayerCastle"))
            {
                if (dropZone.transform.childCount < 9)
                {
                    UpdateCastleStats(dropZone, CreateCharacterFromCard());
                }
                else // there is no place in the Castle
                {
                    transform.position = startPosition;
                    transform.SetParent(startParent.transform, false);
                }
            }
            else // SpellArea
            {
                // add spell
                MainManager.Instance.gameManager.SendMove(new Move { userId = MainManager.Instance.currentUserId, command = "ADD", type = "SPELL", cardId = 3456 });

                transform.SetParent(dropZone.transform, false);
            }
        } else // no droppable area selected
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData) // tap spell
    {

        if (transform.parent.name.Equals("PlayerSpellArea") && GetComponent<RectTransform>().rotation == new Quaternion(0,0,0,1))
        {
            transform.Rotate(0, 0, -90);
            MainManager.Instance.gameManager.SendMove(new Move { userId = MainManager.Instance.currentUserId, command = "TAP", type = "SPELL", cardId = 2345 });

        }
    }
}
