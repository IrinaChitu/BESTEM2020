using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Managers;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public GameObject cardPrefab;

    private int objectNum = 0;
    private List<GameObject> cards = new List<GameObject>();
    // Start is called before the first frame update
    async void Start()
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
                    case "type": // todo: implement
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
            card.dbID = documentSnapshot.Id;
            AddChild(card);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ++objectNum;
            UpdateSize();
            AddChild();
        }
    }

    void UpdateSize()
    {
        int cellX = (int)Mathf.Ceil(gameObject.GetComponent<GridLayoutGroup>().cellSize.x);
        int cellY = (int)Mathf.Ceil(gameObject.GetComponent<GridLayoutGroup>().cellSize.y);
        int spacingX = (int)Mathf.Ceil(gameObject.GetComponent<GridLayoutGroup>().spacing.x);
        int spacingY = (int)Mathf.Ceil(gameObject.GetComponent<GridLayoutGroup>().spacing.y);
        int maxCols = gameObject.GetComponent<GridLayoutGroup>().constraintCount;

        int rows = (int)Mathf.Ceil((float)objectNum / (float)maxCols);
        int heigth = (int)Mathf.Ceil(cellY * rows + (rows == 0 ? 0 : rows - 1) * spacingY);
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, heigth);

        if (heigth < 880.0f)
        {
            gameObject.transform.parent.gameObject.GetComponent<ScrollRect>().vertical = false;
        } else
        {
            gameObject.transform.parent.gameObject.GetComponent<ScrollRect>().vertical = true;
        }
    }

    void AddChild()
    {
        GameObject newCard = Instantiate(cardPrefab, new Vector2(200, 200), Quaternion.identity);

        Card card = new Card();
        newCard.GetComponent<CardScript>().SetCard(card);
        newCard.GetComponent<CardScript>().SetupFull();
        newCard.transform.SetParent(this.transform, false);
        cards.Add(newCard);
    }

    void AddChild(Card card)
    {
        objectNum += 1;
        UpdateSize();

        GameObject newCard = Instantiate(cardPrefab, new Vector2(200, 200), Quaternion.identity);
        newCard.GetComponent<CardScript>().SetCard(card);
        newCard.GetComponent<CardScript>().SetupFull();
        newCard.transform.SetParent(this.transform, false);
        cards.Add(newCard);
    }

    public async void SaveDeck()
    {
        List<string> selectedCards = new List<string>();
        foreach (GameObject obj in cards)
        {
            for (int i = 0; i < obj.GetComponent<CardScript>().quantity; ++i)
            {
                selectedCards.Add(obj.GetComponent<CardScript>().card.dbID);
            }
        }

        foreach (string card in selectedCards)
        {
            Debug.Log(card);
        }
        Dictionary<string, object> deck = new Dictionary<string, object>
        {
            { "cards", selectedCards.ToArray() }
        };
        Debug.Log(deck);
        DocumentReference addedDocRef = await MainManager.Instance.firebaseManager.firestore.Collection("users").Document(MainManager.Instance.currentUserId).Collection("decks").AddAsync(deck);
        Debug.Log("Done: " + addedDocRef.Id);
       
    }

    public void GoToLoadDecks()
    {
        SceneManager.LoadScene("LoadDecks");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
