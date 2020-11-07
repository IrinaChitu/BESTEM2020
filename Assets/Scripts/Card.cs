using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public TMPro.TextMeshProUGUI title;
    public Image image;
    public TMPro.TextMeshProUGUI description;
    public TMPro.TextMeshProUGUI dmgText, hpText, manaText;
    public int dmgValue, hpValue, manaValue;

    void Awake()
    {
        title.text = "Buna siua cuaie";

    }
}
