using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string fileName;
    public string playerName;
    public int money;
    public int day;

    public int[] relationships;

    public SaveData(int fileNum, string pName, int moneyAmt, int dayNum, Character[] characters)
    {
        fileName = "vds_save_" + fileNum + ".dat";

        playerName = pName;
        money = moneyAmt;
        day = dayNum;

        List<int> relList = new List<int>();
        foreach (Character character in characters)
        {
            relList.Add(character.relationship);
        }

        relationships = relList.ToArray();
    }
}
