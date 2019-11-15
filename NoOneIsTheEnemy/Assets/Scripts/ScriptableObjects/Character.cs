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

    public int relationship;

    public bool met;

    private void OnEnable()
    {
        read = new bool[dialogues.Length];
        for (int i = 0; i < read.Length; i++)
        {
            read[i] = false;
        }
        relationship = -100;
        met = false;
    }

    public void AdjustRelationship(int amt)
    {
        relationship += amt;
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
            dIndex = hateDialogueIndices[Random.Range(0, hateDialogueIndices.Length)];
        }
        else if (relationship < 10)
        {
            dIndex = neutralDialogueIndices[Random.Range(0, neutralDialogueIndices.Length)];
        }
        else
        {
            dIndex = likedDialogueIndices[Random.Range(0, likedDialogueIndices.Length)];
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
