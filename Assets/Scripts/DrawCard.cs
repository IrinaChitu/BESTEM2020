using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawCard : MonoBehaviour
{
    public GameObject cardObject;
    public GameObject PlayerArea;
    public GameObject EnemyArea;

    GameObject PlayerDeck;

    List<Color32> colors = new List<Color32>();

    private void Start()
    {
        colors.Add(new Color32(32, 21, 236, 255));
        colors.Add(new Color32(236, 21, 34, 255));
        
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
