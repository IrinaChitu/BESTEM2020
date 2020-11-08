using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card
{
    public static int ID = 0;
    public string dbID;
    public string title;
    public Sprite sprite;
    public string description;
    public int dmgValue;
    public int hpValue;
    public int manaValue;

    private Sprite[] allChars;

    public Card()
    {
        title = "Title: " + ID.ToString();
        description = "Description " + ID.ToString();
        dmgValue = ID;
        hpValue = ID;
        manaValue = ID;
        ID += 1;
    }
}

public class CardScript : MonoBehaviour
{
    public TMPro.TextMeshProUGUI title;
    public Image image;
    public TMPro.TextMeshProUGUI description;
    public TMPro.TextMeshProUGUI dmgText, hpText, manaText;
    public int dmgValue, hpValue, manaValue;
    public Card card;

    public int quantity = 0;
    public GameObject plusBtn = null;
    public GameObject minusBtn = null;
    public Text quantityText = null;
    public void Awake()
    {
        minusBtn.SetActive(false);
    }

    public void SetCard(Card card)
    {
        this.card = card;
    }

    public Card GetCard()
    {
        return this.card;
    }

    public void SetupFull()
    {
        title.text = card.title;
        image.sprite = card.sprite;
        description.text = card.description;
        dmgValue = card.dmgValue;
        dmgText.text = dmgValue.ToString();
        hpValue = card.hpValue;
        hpText.text = hpValue.ToString();
        manaValue = card.manaValue;
        manaText.text = manaValue.ToString();
    }

    public void SetupPreview()
    {
        title.text = card.title;
        image.sprite = card.sprite;
    }

    public void IncrementQuantity()
    {
        quantity = Mathf.Min(quantity + 1, 999);
        quantityText.text = quantity.ToString();
        if (quantity == 999)
        {
            plusBtn.SetActive(false);
        } else
        {
            minusBtn.SetActive(true);
            plusBtn.SetActive(true);
        }
    }
    public void DecrementQuantity()
    {
        quantity = Mathf.Max(quantity - 1, 0);
        quantityText.text = quantity.ToString();
        if (quantity == 0)
        {
            minusBtn.SetActive(false);
        }
        else
        {
            plusBtn.SetActive(true);
            minusBtn.SetActive(true);
        }
    }
}
