using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerClickHandler
{
    public GameObject Canvas;
    public GameObject Character;
    
    private bool isDragging = false;
    private bool isOverDropZone = false;
    private GameObject dropZone;
    private GameObject startParent;
    private Vector2 startPosition;

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

    private void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
        CreateLandStatsComponents();
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

        // get these fields from current card
        pawn.GetComponent<Image>().sprite = Resources.Load<Sprite>("Characters/YumikoSplash");

        pawn.AddComponent<CharacterStats>();

        CharacterStats characterStats = pawn.GetComponent<CharacterStats>();
        characterStats.ComponentConstructor("melee", 10, 10);

        pawn.transform.SetParent(dropZone.transform, false);
        Destroy(this.gameObject);

        return characterStats;
    }


    private void UpdateCastleStats(GameObject playerCastle, CharacterStats characterStats)
    {
        LandStats castleStats = playerCastle.GetComponent<LandStats>();

        if (characterStats.type.Equals("melee"))
        {
            castleStats.playerUnits.melee.Add(characterStats);
            castleStats.playerUnits.meleeDamage += characterStats.dmgValue;
        }
        else if (characterStats.type.Equals("range"))
        {
            castleStats.playerUnits.rangeDamage += characterStats.dmgValue;
        }
        castleStats.playerUnits.allUnits.Add(characterStats);
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
        if(transform.parent.name.Equals("PlayerSpellArea") && GetComponent<RectTransform>().rotation == new Quaternion(0,0,0,1))
        {
            transform.Rotate(0, 0, -90);
        }
    }
}
