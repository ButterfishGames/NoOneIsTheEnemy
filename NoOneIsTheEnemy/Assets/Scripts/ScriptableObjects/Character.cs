using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character", order = 1)]
public class Character : ScriptableObject
{
    /* NOTES ON DIALOGUE INDICES
     * 0: Introductory Dialogue
     * 1: Love Dialogue
     * 2-4: Nothing to say Dialogues
     */

    [Tooltip("Character's full name, to be displayed at the top of text box when they talk")]
    public string charName;

    [Tooltip("Input pronouns in the order of this example: they, them, their, theirs, themself. You must include all five, even if they repeat.")]
    public string[] pronouns = new string[5];

    [Tooltip("List of possible dialogues")]
    public Dialogue[] dialogues;

    [Tooltip("DO NOT TOUCH. HANDLED BY SCRIPT")]
    public bool[] read;

    [Tooltip("Array of expression sprites. (Could only contain one)")]
    public Sprite[] faceSprites;

    [Tooltip("The index of the first kill dialogue. All kill dialogues should be at the end of the list")]
    public int firstKillIndex;

    [Tooltip("An array of indices of hate dialogues in the dialogues array")]
    public int[] hateDialogueIndices;

    [Tooltip("An array of indices of neutral dialogues in the dialogues array")]
    public int[] neutralDialogueIndices;

    [Tooltip("An array of indices of liked dialogues in the dialogues array")]
    public int[] likedDialogueIndices;

    [Tooltip("0 should be generic, 1 should be personal, 2 should be wrong personal")]
    public int[] giftDialogueIndices;

    public int relationship;

    public bool met;

    [TextArea(3, 10)]
    public string[] deaths;
    
    public int[] gGiftsReceived;

    private void OnEnable()
    {
        read = new bool[dialogues.Length];
        for (int i = 0; i < read.Length; i++)
        {
            read[i] = false;
        }
        relationship = -100;
        met = false;

        gGiftsReceived = new int[]{ 0, 0, 0 };
    }

    public void StartDialogue()
    {
        int dIndex = 0;

        if (!met)
        {
            dIndex = 0;
        }
        else if (relationship < -60)
        {
            List<int> unreadHate = new List<int>();
            foreach(int index in hateDialogueIndices)
            {
                if (!read[index])
                {
                    unreadHate.Add(index);
                }
            }

            if (unreadHate.ToArray().Length > 0)
            {
                dIndex = unreadHate[Random.Range(0, unreadHate.ToArray().Length)];
            }
            else
            {
                dIndex = 2;
            }
        }
        else if (relationship < 10)
        {
            List<int> unreadNeutral = new List<int>();
            foreach(int index in neutralDialogueIndices)
            {
                if (!read[index])
                {
                    unreadNeutral.Add(index);
                }
            }

            if (unreadNeutral.ToArray().Length > 0)
            {
                dIndex = unreadNeutral[Random.Range(0, unreadNeutral.ToArray().Length)];
            }
            else
            {
                dIndex = 3;
            }
        }
        else
        {
            List<int> unreadLiked = new List<int>();
            foreach(int index in likedDialogueIndices)
            {
                if (!read[index])
                {
                    unreadLiked.Add(index);
                }
            }

            if (unreadLiked.ToArray().Length > 0)
            {
                dIndex = unreadLiked[Random.Range(0, unreadLiked.ToArray().Length)];
            }
            else
            {
                dIndex = 4;
            }
        }

        read[dIndex] = true;
        DialogueManager.singleton.StartDialogue(this, dialogues[dIndex]);
    }

    public void StartDialogue(int dIndex, int relChange)
    {
        relationship += relChange;
        read[dIndex] = true;
        DialogueManager.singleton.StartDialogue(this, dialogues[dIndex]);
    }

    public void Kill()
    {

    }

    public void Love()
    {
        StartDialogue(1, 0);
    }
}
