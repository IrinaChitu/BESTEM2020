using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Managers;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

using UnityEngine.SceneManagement;
using System.Linq;

public class LeaderboardUser
{
    public string username;
    public int wins;

    public LeaderboardUser()
    {
        this.username = "";
        this.wins = 0;
    }

    public LeaderboardUser(string username, int wins)
    {
        this.username = username;
        this.wins = wins;
    }
}

public class LoadLeaderboardManager : MonoBehaviour
{
    public GameObject leaderbooadUserPrefab;

    private int objectNum = 0;
    private Dictionary<string, LeaderboardUser> users = new Dictionary<string, LeaderboardUser>();
    //private Dictionary<string, List<string>> decks = new Dictionary<string, List<string>>();
    private List<LeaderboardUser> allUsers;

    //// Start is called before the first frame update
    async void Start()
    {
        GetAllUsers();
    }

    async void GetAllUsers()
    {
        Query allUsersQery = MainManager.Instance.firebaseManager.firestore.Collection("users");
        QuerySnapshot allUsersQerySnapshot = await allUsersQery.GetSnapshotAsync();

        List<LeaderboardUser> tempUsers = new List<LeaderboardUser>();
        foreach (DocumentSnapshot documentSnapshot in allUsersQerySnapshot.Documents)
        {
            Debug.Log(documentSnapshot.Id);
            Dictionary<string, object> userDict = documentSnapshot.ToDictionary();
            LeaderboardUser user = new LeaderboardUser();
            foreach (KeyValuePair<string, object> pair in userDict)
            {
                switch (pair.Key)
                {
                    case "Username":
                        user.username = (string)pair.Value;
                        break;
                    case "Wins":
                        user.wins = System.Convert.ToInt32(pair.Value);
                        break;
                    default:
                        break;
                }
            }

            tempUsers.Add(user);
            //users.Add(documentSnapshot.Id, user);

            //Debug.Log(allUsernames);
            //Debug.Log(allWins);
             Debug.Log("Done!");
            //decks.Add(documentSnapshot.Id, selectedCards);
            //AddChild();
        }

        List<LeaderboardUser> sortedUsers = tempUsers.OrderBy(u => u.wins).Reverse().ToList();

        RenderUserOnLeaderboard(sortedUsers);
    }

    void RenderUserOnLeaderboard(List<LeaderboardUser> users)
    {
        foreach (LeaderboardUser user in users)
        {
            //LeaderboardUser user = pair.Value;
            GameObject newLeaderboardUser = Instantiate(leaderbooadUserPrefab, new Vector2(200, 200), Quaternion.identity);
            newLeaderboardUser.GetComponent<LeaderboardElementScript>().SetUserAndWins(user.username, user.wins);
            newLeaderboardUser.transform.SetParent(this.transform, false);
        }
    }

    //async void GetAllCards()
    //{
    //    Query allCardsQuery = MainManager.Instance.firebaseManager.firestore.Collection("cards");
    //    QuerySnapshot allCardsQuerySnapshot = await allCardsQuery.GetSnapshotAsync();

    //    foreach (DocumentSnapshot documentSnapshot in allCardsQuerySnapshot.Documents)
    //    {
    //        Dictionary<string, object> cardDict = documentSnapshot.ToDictionary();
    //        Card card = new Card();
    //        foreach (KeyValuePair<string, object> pair in cardDict)
    //        {
    //            switch (pair.Key)
    //            {
    //                case "name":
    //                    card.title = (string)pair.Value;
    //                    break;
    //                case "icon":
    //                    card.sprite = Resources.Load<Sprite>("Characters/" + (string)pair.Value);
    //                    break;
    //                case "type": // todo: implement
    //                    break;
    //                case "dmg":
    //                    card.dmgValue = System.Convert.ToInt32(pair.Value);
    //                    break;
    //                case "hp":
    //                    card.hpValue = System.Convert.ToInt32(pair.Value);
    //                    break;
    //                case "mana":
    //                    card.manaValue = System.Convert.ToInt32(pair.Value);
    //                    break;
    //            }
    //        }
    //        cards.Add(documentSnapshot.Id, card);
    //    }
    //    GetAllDecks();
    //}

    //void Update()
    //{
    //    if (Input.GetKeyDown("space"))
    //    {
    //        ++objectNum;
    //        UpdateSize();
    //        AddChild();
    //    }
    //}

    //void UpdateSize()
    //{
    //    int cellX = (int)Mathf.Ceil(gameObject.GetComponent<GridLayoutGroup>().cellSize.x);
    //    int cellY = (int)Mathf.Ceil(gameObject.GetComponent<GridLayoutGroup>().cellSize.y);
    //    int spacingX = (int)Mathf.Ceil(gameObject.GetComponent<GridLayoutGroup>().spacing.x);
    //    int spacingY = (int)Mathf.Ceil(gameObject.GetComponent<GridLayoutGroup>().spacing.y);
    //    int maxCols = gameObject.GetComponent<GridLayoutGroup>().constraintCount;

    //    int rows = (int)Mathf.Ceil((float)objectNum / (float)maxCols);
    //    int heigth = (int)Mathf.Ceil(cellY * rows + (rows == 0 ? 0 : rows - 1) * spacingY);
    //    RectTransform rect = gameObject.GetComponent<RectTransform>();
    //    rect.sizeDelta = new Vector2(rect.sizeDelta.x, heigth);

    //    if (heigth < 880.0f)
    //    {
    //        gameObject.transform.parent.gameObject.GetComponent<ScrollRect>().vertical = false;
    //    }
    //    else
    //    {
    //        gameObject.transform.parent.gameObject.GetComponent<ScrollRect>().vertical = true;
    //    }
    //}

    void AddChild()
    {
        GameObject newUser = Instantiate(leaderbooadUserPrefab, new Vector2(200, 200), Quaternion.identity);
        newUser.transform.SetParent(this.transform, false);
    }

    //void AddChild(string deckId)
    //{
    //    objectNum += 1;
    //    UpdateSize();

    //    selectedCards = new List<Card>();
    //    foreach (string cardId in decks[deckId])
    //    {
    //        selectedCards.Add(cards[cardId]);
    //    }

    //    GameObject newDeck = Instantiate(deckPrefab, new Vector2(200, 200), Quaternion.identity);
    //    newDeck.GetComponent<DeckScript>().Init(deckId, selectedCards);
    //    newDeck.transform.SetParent(this.transform, false);
    //}

    //public async void ClearDeck(string deckId)
    //{
    //    foreach (Transform child in transform)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //    decks = new Dictionary<string, List<string>>();
    //    MainManager.Instance.firebaseManager.firestore.Collection("users").Document(MainManager.Instance.currentUserId).Collection("decks").Document(deckId).DeleteAsync();
    //    GetAllDecks();
    //}

    //public void GoToCardsInventory()
    //{
    //    SceneManager.LoadScene("CardsInventory");
    //}
}
