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


    private void Awake()
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


    public void EndDrag()
    {
        isDragging = false;
        if (isOverDropZone)
        {
            if (dropZone.name.Equals("PlayerCastle"))
            {
                if (dropZone.transform.childCount < 9)
                {
                    GameObject pawn = Instantiate(Character, new Vector2(0, 0), Quaternion.identity);
                    pawn.GetComponent<Image>().sprite = Resources.Load<Sprite>("Characters/YumikoSplash"); // get this from current card instead of dirrectly from assets
                    //pawn.tag = "Player"; // used to check for battles
                    pawn.transform.SetParent(dropZone.transform, false);
                    Destroy(this.gameObject);

                } else // there is no place in the Castle
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
