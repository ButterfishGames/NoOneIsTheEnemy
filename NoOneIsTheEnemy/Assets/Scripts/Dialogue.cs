using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [Tooltip("List of dialogue lines for the dialogue")]
    public DialogueLine[] lines;

    [Tooltip("Whether dialogue ends in a branch or the end of a conversation")]
    public DialogueType type;

    [Tooltip("Branch option text. Only used if Type is Branch. Should have 2-3 branches")]
    public string[] branches;

    [Tooltip("Dialogues that happen after branches. Only used if Type is Branch. Should have the same number as number of branches.")]
    public Dialogue[] branchDialogues;
}

[System.Serializable]
public struct DialogueLine
{
    [Tooltip("The name to display with this line")]
    public string name;

    [Tooltip("The text of the dialogue line")]
    [TextArea(3, 10)]
    public string line;
}

public enum DialogueType
{
    branch,
    end
}