using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject cardPrefab;

    private int objectNum = 0;
    private List<Card> cards = new List<Card>();

    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    void AddChild()
    {
        GameObject newCard = Instantiate(cardPrefab, new Vector2(200, 200), Quaternion.identity);

        Card tmp = new Card();
        cards.Add(tmp);
        newCard.GetComponent<CardScript>().SetCard(tmp);

        newCard.GetComponent<CardScript>().SetupFull();
        newCard.transform.SetParent(this.transform, false);
    }
}
