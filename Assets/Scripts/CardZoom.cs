using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZoom : MonoBehaviour
{
    public GameObject Canvas;

    private GameObject zoomCard;

    public void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
    }

    public void OnHoverEnter()
    {
        // + 100 -> will only work for the lower row; could treat them separately -> instantiate them from drawCards as two different types so that we can particularize them here
        // or choose an absolute value where to show them (eg: middle left)
        zoomCard = Instantiate(gameObject, new Vector2(Input.mousePosition.x, Input.mousePosition.y + 120), Quaternion.identity);
        zoomCard.transform.SetParent(Canvas.transform, false);
        zoomCard.layer = LayerMask.NameToLayer("Zoom"); // alternative: remove the collider from the zoomCard

        RectTransform rect = zoomCard.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(150, 210); // (3*rect.sizeDelta.x, 3*rect.sizeDelta.y)
    }

    public void OnHoverExit()
    {
        Destroy(zoomCard);
    }
}
