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
    public bool[] bigGiftsPurchased;

    public int[] relationships;
    public bool[] met;
    public bool[][] read;
    public bool[] sGiftReceived;
    public int[][] gGiftsReceived;

    public SaveData(int fileNum, string pName, int moneyAmt, int dayNum, bool[] giftsPurchased, Character[] characters)
    {
        fileName = "vds_save_" + fileNum + ".dat";

        playerName = pName;
        money = moneyAmt;
        day = dayNum;
        bigGiftsPurchased = giftsPurchased;

        List<int> relList = new List<int>();
        List<bool> metList = new List<bool>();
        List<bool[]> readList = new List<bool[]>();
        List<int[]> gGiftsList = new List<int[]>();

        foreach(Character character in characters)
        {
            relList.Add(character.relationship);
            metList.Add(character.met);
            readList.Add(character.read);
            gGiftsList.Add(character.gGiftsReceived);
        }

        relationships = relList.ToArray();
        met = metList.ToArray();
        read = readList.ToArray();
        gGiftsReceived = gGiftsList.ToArray();
    }
}
