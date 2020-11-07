using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawCard : MonoBehaviour
{
    public GameObject Card;
    public GameObject PlayerArea;
    public GameObject EnemyArea;

    GameObject PlayerDeck;

    List<GameObject> cards = new List<GameObject>();
    List<Color32> colors = new List<Color32>();

    private void Start()
    {
        cards.Add(Card);
        cards.Add(Card);
        colors.Add(new Color32(32, 21, 236, 255));
        colors.Add(new Color32(236, 21, 34, 255));
        
        PlayerDeck = GameObject.Find("PlayerDeck");
    }

    public void Update()
    {
        
        if (PlayerDeck.transform.childCount >0 && PlayerArea.transform.childCount < 5)
        {
            GameObject newCard = PlayerDeck.transform.GetChild(PlayerDeck.transform.childCount - 1).gameObject;
            newCard.transform.SetParent(PlayerArea.transform, false);
        }
    }

    public void OnClick()
    {
        for (var i = 0; i < 3; i++)
        {
            GameObject playerCard = Instantiate(cards[0], new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.GetComponent<Image>().color = new Color32(32, 21, 236, 255);
            playerCard.transform.SetParent(PlayerDeck.transform, false);
        }

            for (var i= 0; i< 5; i++ )
        {
            //GameObject playerCard = Instantiate(cards[Random.Range(0, cards.Count)], new Vector3(0, 0, 0), Quaternion.identity);
            //playercard.getcomponent<image>().color = colors[Random.Range(0, colors.Count)];

            GameObject playerCard = Instantiate(cards[0], new Vector3(0, 0, 0), Quaternion.identity);
            playerCard.GetComponent<Image>().color = new Color32(32, 21, 236, 255);
            playerCard.transform.SetParent(PlayerArea.transform, false);

            GameObject enemyCard = Instantiate(Card, new Vector3(0, 0, 0), Quaternion.identity);
            enemyCard.GetComponent<Image>().color = new Color32(236, 21, 34, 255);
            enemyCard.transform.SetParent(EnemyArea.transform, false);

            // create a tag to differentiate between player and enemy
        }
        
    }
}
