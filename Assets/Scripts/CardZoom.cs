using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZoom : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject cardPreview;

    private GameObject zoomCard;

    public void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
    }

    public void OnHoverEnter()
    {
        float scaleValue = 2.0f;
        // + 100 -> will only work for the lower row; could treat them separately -> instantiate them from drawCards as two different types so that we can particularize them here
        // or choose an absolute value where to show them (eg: middle left)
        // zoomCard = Instantiate(gameObject, new Vector2(Input.mousePosition.x, Input.mousePosition.y + 120), Quaternion.identity);
        zoomCard = Instantiate(cardPreview, new Vector2(200, 200), Quaternion.identity);
        zoomCard.GetComponent<CardScript>().SetCard(GetComponent<CardScript>().GetCard());
        zoomCard.GetComponent<CardScript>().SetupFull();
        zoomCard.transform.SetParent(Canvas.transform, false);
        zoomCard.layer = LayerMask.NameToLayer("Zoom"); // alternative: remove the collider from the zoomCard
    }

    public void OnHoverExit()
    {
        Destroy(zoomCard);
    }
}
