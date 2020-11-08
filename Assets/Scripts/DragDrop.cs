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

    

    private void Start()
    {
        Canvas = GameObject.Find("Main Canvas");
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
