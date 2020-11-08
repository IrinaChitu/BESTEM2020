using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public GameObject cardPreviewPrefab;
    public GameObject grid;
    public TMPro.TextMeshProUGUI title;
    public string deckId;
    private GameObject cardPreview;

    public void Init(string name, List<Card> cards)
    {
        deckId = name;
        title.text = name;
        foreach (Card card in cards)
        {
            cardPreview = Instantiate(cardPreviewPrefab, new Vector2(0, 0), Quaternion.identity);
            cardPreview.GetComponent<CardScript>().SetCard(card);
            cardPreview.GetComponent<CardScript>().SetupPreview();
            cardPreview.transform.SetParent(grid.transform, false);
            cardPreview.layer = LayerMask.NameToLayer("Zoom");
        }       
    }

    public void DeleteDeck()
    {
        gameObject.transform.parent.gameObject.GetComponent<LoadDeckManager>().ClearDeck(deckId);
    }
}
