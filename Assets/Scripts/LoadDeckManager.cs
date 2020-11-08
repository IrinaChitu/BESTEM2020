using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Managers;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

using UnityEngine.SceneManagement;

public class LoadDeckManager : MonoBehaviour
{
    public GameObject deckPrefab;
    public string previousDeck;

    private int objectNum = 0;
    private Dictionary<string, Card> cards = new Dictionary<string, Card>();
    private Dictionary<string, List<string>> decks = new Dictionary<string, List<string>>();
    private List<Card> selectedCards;

    //// Start is called before the first frame update
    async void Start()
    {
        GetAllCards();
    }

    async void GetAllDecks()
    {
        Query allDecksQery = MainManager.Instance.firebaseManager.firestore.Collection("users").Document(MainManager.Instance.currentUserId).Collection("decks");
        QuerySnapshot allDecksQerySnapshot = await allDecksQery.GetSnapshotAsync();

        foreach (DocumentSnapshot documentSnapshot in allDecksQerySnapshot.Documents)
        {
            Debug.Log(documentSnapshot.Id);
            Dictionary<string, object> deckDict = documentSnapshot.ToDictionary();
            List<string> selectedCards = new List<string>();
            bool selected = false;
            foreach (KeyValuePair<string, object> pair in deckDict)
            {
                if (pair.Key == "cards")
                {
                    selectedCards = (pair.Value as List<object>).ConvertAll(input => input.ToString());
                } else
                {
                    selected = (bool)pair.Value;
                    if (selected)
                    {
                        previousDeck = documentSnapshot.Id;
                    }
                }
            }
            Debug.Log("Done!");
            decks.Add(documentSnapshot.Id, selectedCards);
            AddChild(documentSnapshot.Id);
        }
        UpdateSelected(previousDeck);
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
        GameObject newDeck = Instantiate(deckPrefab, new Vector2(200, 200), Quaternion.identity);
        newDeck.transform.SetParent(this.transform, false);
    }

    void AddChild(string deckId)
    {
        objectNum += 1;
        UpdateSize();

        selectedCards = new List<Card>();
        foreach (string cardId in decks[deckId]) {
            selectedCards.Add(cards[cardId]);
        }

        GameObject newDeck = Instantiate(deckPrefab, new Vector2(200, 200), Quaternion.identity);
        newDeck.GetComponent<DeckScript>().Init(deckId, selectedCards);
        newDeck.transform.SetParent(this.transform, false);
    }

    public async void UpdateSelected(string deckId)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<DeckScript>().selectButton.SetActive(true);
        }
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<DeckScript>().deckId == deckId)
            {
                child.gameObject.GetComponent<DeckScript>().selectButton.SetActive(false);
            }
        }
        DocumentReference deckRef = MainManager.Instance.firebaseManager.firestore.Collection("users").Document(MainManager.Instance.currentUserId).Collection("decks").Document(previousDeck);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "selected", false }
        };
        await deckRef.UpdateAsync(updates);

        deckRef = MainManager.Instance.firebaseManager.firestore.Collection("users").Document(MainManager.Instance.currentUserId).Collection("decks").Document(deckId);
        updates = new Dictionary<string, object>
        {
            { "selected", true }
        };
        await deckRef.UpdateAsync(updates);
        previousDeck = deckId;
    }

    public async void ClearDeck(string deckId)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        decks = new Dictionary<string, List<string>>();
        MainManager.Instance.firebaseManager.firestore.Collection("users").Document(MainManager.Instance.currentUserId).Collection("decks").Document(deckId).DeleteAsync();
        GetAllDecks();
    }

    public void GoToCardsInventory()
    {
        SceneManager.LoadScene("CardsInventory");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
