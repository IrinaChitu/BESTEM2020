using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Managers;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

public class DrawCard : MonoBehaviour
{
    public GameObject cardObject;
    public GameObject PlayerArea;
    public GameObject EnemyArea;
    public string previousDeck;

    GameObject PlayerDeck;

    private Dictionary<string, Card> cards = new Dictionary<string, Card>();
    private List<string> selectedCards = new List<string>();

    public async void Start()
    {
        GetAllCards();
    }

    
    async void GetAllCards()
    {
        Query allCardsQuery = MainManager.Instance.firebaseManager.firestore.Collection("cards");
        QuerySnapshot allCardsQuerySnapshot = await allCardsQuery.GetSnapshotAsync();

        foreach (DocumentSnapshot documentSnapshot in allCardsQuerySnapshot.Documents)
        {
            Dictionary<string, object> cardDict = documentSnapshot.ToDictionary();
            Card card = new Card();
            foreach (KeyValuePair<string, object> pair in cardDict)
            {
                switch (pair.Key)
                {
                    case "name":
                        card.title = (string)pair.Value;
                        break;
                    case "icon":
                        card.sprite = Resources.Load<Sprite>("Characters/" + (string)pair.Value);
                        break;
                    case "type":
                        card.details = (string)pair.Value;
                        break;
                    case "dmg":
                        card.dmgValue = System.Convert.ToInt32(pair.Value);
                        break;
                    case "hp":
                        card.hpValue = System.Convert.ToInt32(pair.Value);
                        break;
                    case "mana":
                        card.manaValue = System.Convert.ToInt32(pair.Value);
                        break;
                }
            }
            cards.Add(documentSnapshot.Id, card);
        }
        GetAllDecks();
    }

    async void GetAllDecks()
    {
        Query allDecksQery = MainManager.Instance.firebaseManager.firestore.Collection("users").Document(MainManager.Instance.currentUserId).Collection("decks");
        QuerySnapshot allDecksQerySnapshot = await allDecksQery.GetSnapshotAsync();
        foreach (DocumentSnapshot documentSnapshot in allDecksQerySnapshot.Documents)
        {
            Debug.Log(documentSnapshot.Id);
            Dictionary<string, object> deckDict = documentSnapshot.ToDictionary();
            bool selected = false;
            foreach (KeyValuePair<string, object> pair in deckDict)
            {
                if (pair.Key == "cards")
                {
                    selectedCards = (pair.Value as List<object>).ConvertAll(input => input.ToString());
                }
                else
                {
                    selected = (bool)pair.Value;
                }
            }
            if (selected)
            {
                previousDeck = documentSnapshot.Id;
                break;
            }
        }
        InitCards();
    }

    async void InitCards()
    {
        GameObject PlayerDeck = GameObject.Find("PlayerDeck");
        foreach (string cardId in selectedCards)
        {
            GameObject playerCard = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.GetComponent<CardScript>().SetCard(cards[cardId]);
            playerCard.GetComponent<CardScript>().SetupPreview();
            playerCard.transform.SetParent(PlayerArea.transform, false);
        }
    }

    public void OnClick()
    {
        GameObject PlayerDeck = GameObject.Find("PlayerDeck");

        for (var i = 0; i < 3; i++)
        {
            Card tmp = new Card();
            GameObject playerCard = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.GetComponent<CardScript>().SetCard(tmp);
            playerCard.GetComponent<CardScript>().SetupPreview();
            // playerCard.GetComponent<Image>().color = new Color32(32, 21, 236, 255);
            playerCard.transform.SetParent(PlayerDeck.transform, false);
        }

        for (var i= 0; i< 5; i++ )
        {
            Card tmp = new Card();
            //GameObject playerCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            //playercard.getcomponent<image>().color = colors[Random.Range(0, colors.Count)];

            GameObject playerCard = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.GetComponent<CardScript>().SetCard(tmp);
            playerCard.GetComponent<CardScript>().SetupPreview();
            // playerCard.GetComponent<Image>().color = new Color32(32, 21, 236, 255);
            playerCard.transform.SetParent(PlayerArea.transform, false);

            tmp = new Card();
            GameObject enemyCard = Instantiate(cardObject, new Vector3(0, 0, 0), Quaternion.identity);
            enemyCard.GetComponent<CardScript>().SetCard(tmp);
            enemyCard.GetComponent<CardScript>().SetupPreview();
            // enemyCard.GetComponent<Image>().color = new Color32(236, 21, 34, 255);
            enemyCard.transform.SetParent(EnemyArea.transform, false);

            // create a tag to differentiate between player and enemy
        }
        
    }
}
