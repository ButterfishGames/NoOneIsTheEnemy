using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character", order = 1)]
public class Character : ScriptableObject
{
    [Tooltip("Character's full name, to be displayed at the top of text box when they talk")]
    public string charName;

    [Tooltip("Character's shortened name. Usually a first name.")]
    public string shortName;

    [Tooltip("Input pronouns in the order of this example: they, them, their, theirs, themself. You must include all five, even if they repeat.")]
    public string[] pronouns = new string[5];

    [Tooltip("List of possible dialogues")]
    public Dialogue[] dialogues;

    [Tooltip("Array of sprites. (Could only contain one)")]
    public Sprite[] sprites;

    public int relationship;

    private void OnEnable()
    {
        relationship = -100;
    }

    public void AdjustRelationship(int amt)
    {
        relationship += amt;
    }

    public void StartDialogue()
    {
        DialogueManager.singleton.StartDialogue(this, dialogues[0]); // TEST: REPLACE WITH RELATIONSHIP PARSING CODE
    }

    public void StartDialogue(int dIndex, int relChange)
    {
        relationship += relChange;
        DialogueManager.singleton.StartDialogue(this, dialogues[dIndex]);
    }
}
