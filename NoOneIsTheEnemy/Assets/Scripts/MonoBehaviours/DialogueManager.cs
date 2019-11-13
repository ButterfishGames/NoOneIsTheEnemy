using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    private Queue<string> names;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        foreach (DialogueLine line in dialogue.lines)
        {
            names.Enqueue(line.name);
            sentences.Enqueue(line.line);
        }
    }
}
