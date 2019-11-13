using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    public Text dayText, moneyText, energyText;

    public void UpdateUI()
    {
        dayText.text = "Day " + GameController.singleton.day;
        moneyText.text = "Money: " + GameController.singleton.money;
        energyText.text = "Energy: " + GameController.singleton.energy + "/" + GameController.singleton.dailyEnergy;
    }

    public void Travel (int sceneIndex)
    {
        GameController.singleton.energy--; // If not shop scene
        GameController.singleton.LoadScene(sceneIndex);
    }
}
