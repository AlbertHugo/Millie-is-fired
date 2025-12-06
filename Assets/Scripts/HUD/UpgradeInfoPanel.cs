using UnityEngine;
using UnityEngine.UI;
using TMPro;   

public class ItemInfoPanel : MonoBehaviour
{
    public GameObject infoPanel;
    public RawImage itemImage;               // <--- trocado
    public TMP_Text itemDescription;         // <--- trocado

    public Texture2D[] itemImages;       // array de imagens
    public string[] itemDescriptions;    // array de textos

    private void Awake()
    {
        // EXEMPLO COMPLETO COM PLACEHOLDERS
        itemDescriptions = new string[8];

        itemDescriptions[0] = "Simple and direct! A damage up that increases the amount of damage your projectiles do.";
        itemDescriptions[1] = "More HP means more chances to get that paycheck, am I right?";
        itemDescriptions[2] = "Speed! Speed is the SOUL of the office! Wouldn't it be nice to be done with all that paperwork faster?";

        itemDescriptions[3] = "THE RULER!! A generalistic weapon that destroys enemies, but not instant-death obstacles!";
        itemDescriptions[4] = "The STAPLER! A great weapon to deal with more bulky enemies with it's stacking damage!";
        itemDescriptions[5] = "Ah, the paper... A destroyer of instant-death obstacles, but it deals less damage in general.";

        itemDescriptions[6] = "THE PEN! In one simple signature, you wipe all obstacles from the screen! Takes time to recharge, though.";
        itemDescriptions[7] = "With all these budget cuts, it's now OUR time to make some! It's a one-time-use ultimate that caps your HP to one, but your damage is MASSIVELY increased! High risk, high reward.";
    }

    public void ShowInfo(string itemID)
    {
        int index = GetIndex(itemID);

        if (index < 0)
        {
            Debug.LogWarning("Item '" + itemID + "' não encontrado!");
            return;
        }

        itemImage.texture = itemImages[index];
        itemDescription.text = itemDescriptions[index];

        infoPanel.SetActive(true);
    }

    private int GetIndex(string id)
    {
        switch (id)
        {
            case "Damage": return 0;
            case "HP": return 1;
            case "Speed": return 2;

            case "Ruler": return 3;
            case "Stapler": return 4;
            case "Paper": return 5;

            case "Pen": return 6;
            case "Scissors": return 7;
        }

        return -1;
    }

    public void HideInfo()
    {
        infoPanel.SetActive(false);
    }
}
