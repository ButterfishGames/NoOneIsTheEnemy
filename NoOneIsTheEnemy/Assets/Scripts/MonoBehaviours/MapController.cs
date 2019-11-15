using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public Text dayText, moneyText;
    public Image dayUI, energyUI;
    public Sprite daySprite, nightSprite;
    public Sprite[] energySprites;
    public GameObject cantWorkPanel;

    public void UpdateUI()
    {
        if (GameController.singleton.day % 2 == 1)
        {
            dayUI.sprite = daySprite;
            dayText.text = "Day " + Mathf.CeilToInt(GameController.singleton.day/2.0f);
        }
        else
        {
            dayUI.sprite = nightSprite;
            dayText.text = "Night " + (GameController.singleton.day / 2);
        }
        moneyText.text = GameController.singleton.money.ToString();

        energyUI.sprite = energySprites[GameController.singleton.energy];
    }

    public void Travel (int sceneIndex)
    {
        GameController.singleton.energy--; // If not shop scene
        GameController.singleton.LoadScene(sceneIndex);
    }

    public void Work()
    {
        if (GameController.singleton.energy >= 2)
        {
            GameController.singleton.Work();
        }
        else
        {
            cantWorkPanel.SetActive(true);
        }
    }

    public void CloseCantWork()
    {
        cantWorkPanel.SetActive(false);
    }
}
