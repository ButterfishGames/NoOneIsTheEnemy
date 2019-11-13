using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager singleton;

    private Queue<string> sentences;
    private Queue<string> names;

    private Character currentCharacter;
    private Dialogue currentDialogue;

    // Start is called before the first frame update
    void Start()
    {
        if (singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }

        sentences = new Queue<string>();
        names = new Queue<string>();

        currentCharacter = null;
        currentDialogue = null;
    }

    public void StartDialogue(Character character, Dialogue dialogue)
    {
        sentences.Clear();
        names.Clear();

        foreach (DialogueLine line in dialogue.lines)
        {
            names.Enqueue(line.name);
            sentences.Enqueue(line.line);
        }

        currentCharacter = character;
        currentDialogue = dialogue;

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (currentDialogue == null)
        {
            return;
        }

        else if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string name = DialogueProcessor(names.Dequeue());
        string line = DialogueProcessor(sentences.Dequeue());

        GameObject.Find("NameText").GetComponent<Text>().text = name;
        GameObject.Find("LineText").GetComponent<Text>().text = line;
    }

    private void EndDialogue()
    {
        currentDialogue.End();

        currentDialogue = null;
        currentCharacter = null;
    }

    private string DialogueProcessor(string input)
    {
        string output = input;

        output = output.Replace("|*CHAR*|", currentCharacter.charName);
        output = output.Replace("|*PLAYER*|", GameController.singleton.playerName);

        return output;
    }
}
