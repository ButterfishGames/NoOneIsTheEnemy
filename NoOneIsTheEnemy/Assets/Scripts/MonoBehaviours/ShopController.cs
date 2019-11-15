using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    private int selected;
    public int genericCost, specialCost;
    public GameObject receiverPanel;
    public Button hGift, pGift, mGift;
    public Text moneyText;

    private void Start()
    {
        selected = 0;
        UpdateUI();
    }

    public void Select(int gift)
    {
        int cost = gift < 4 ? genericCost : specialCost;
        if (cost <= GameController.singleton.money)
        {
            GameController.singleton.money -= cost;
            selected = gift;
            UpdateUI();
            receiverPanel.SetActive(true);
        }
        else
        {
            DialogueManager.singleton.StartDialogue(null, new Dialogue("Chi", "You don't have enough for that."));
        }
    }

    public void Give(int character)
    {
        if (selected <= 0)
        {
            Debug.Log("You can't give somebody nothing!");
        }
        else
        {
            GameController.singleton.Give(character, selected);
        }
    }

    private void UpdateUI()
    {
        moneyText.text = GameController.singleton.money.ToString();
        hGift.interactable = !GameController.singleton.bigGiftsPurchased[0];
        pGift.interactable = !GameController.singleton.bigGiftsPurchased[1];
        mGift.interactable = !GameController.singleton.bigGiftsPurchased[2];
    }
}
