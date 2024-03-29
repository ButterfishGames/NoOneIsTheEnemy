﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager singleton;

    public GameObject branchButtonPrefab;
    private SpriteRenderer charSprite;

    private Queue<string> sentences;
    private Queue<string> names;
    private Queue<int> sprites;

    public Character currentCharacter;
    private Dialogue currentDialogue;

    private bool primed;
    private bool isBranched;
    public bool dying;

    // Start is called before the first frame update
    private void Start()
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
        sprites = new Queue<int>();

        currentCharacter = null;
        currentDialogue = null;

        dying = false;
    }

    private void Update()
    {
        if ((Input.GetButtonDown("Submit") || Input.GetButtonDown("Mouse0")) && EventSystem.current.currentSelectedGameObject == null && !GameController.singleton.loading && !isBranched)
        {
            primed = true;
        }

        if ((Input.GetButtonUp("Submit") || Input.GetButtonDown("Mouse0")) && EventSystem.current.currentSelectedGameObject == null && primed)
        {
            DisplayNextLine();
            primed = false;
        }
    }

    public void StartDialogue(Character character, Dialogue dialogue)
    {
        isBranched = false;
        sentences.Clear();
        names.Clear();
        sprites.Clear();

        charSprite = GameObject.Find("Char").GetComponent<SpriteRenderer>();

        GameObject branchPanel = GameObject.Find("BranchPanel");
        Button[] buttons = branchPanel.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            Destroy(button.gameObject);
        }

        foreach (DialogueLine line in dialogue.lines)
        {
            names.Enqueue(line.name);
            sentences.Enqueue(line.line);
            sprites.Enqueue(line.sprite);
        }

        currentCharacter = character;
        currentDialogue = dialogue;

        GameObject.Find("DialogueBox").GetComponent<Animator>().SetInteger("state", 0);

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

        if (currentCharacter != null)
        {
            charSprite.sprite = currentCharacter.faceSprites[sprites.Dequeue()];
        }

        GameObject.Find("NameText").GetComponent<Text>().text = name;
        GameObject.Find("LineText").GetComponent<Text>().text = line;

        if (currentDialogue.type == DialogueType.branch && sentences.Count == 0)
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        if (currentCharacter != null)
        {
            currentCharacter.met = true;
        }

        if (currentCharacter.relationship <= -150 && !dying)
        {
            currentCharacter.Kill();
            return;
        }

        switch (currentDialogue.type)
        {
            case DialogueType.end:
                if (dying)
                {
                    currentCharacter.Kill();
                    return;
                }
                if (currentCharacter != null && currentCharacter.relationship >= 100)
                {
                    currentCharacter.Love();
                    return;
                }
                else
                {
                    currentDialogue = null;
                    currentCharacter = null;
                    GameController.singleton.LoadScene(GameController.singleton.mapScene);
                }
                break;

            case DialogueType.branch:
                isBranched = true;
                GameObject.Find("DialogueBox").GetComponent<Animator>().SetInteger("state", 1);
                GameObject branchPanel = GameObject.Find("BranchPanel");
                for (int i = 0; i < currentDialogue.branches.Length; i++)
                {
                    GameObject branchButton = Instantiate(branchButtonPrefab, branchPanel.transform);
                    branchButton.GetComponentInChildren<Text>().text = currentDialogue.branches[i];
                    int dIndex = currentDialogue.branchDialogues[i];
                    int relChange = currentDialogue.relationshipChanges[i];
                    branchButton.GetComponentInChildren<Button>().onClick.AddListener(() => currentCharacter.StartDialogue(dIndex, relChange));
                    if (i == 0)
                    {
                        EventSystem.current.SetSelectedGameObject(branchButton);
                    }
                }
                break;

            case DialogueType.kill:
                GameController.singleton.dead = true;
                GameController.singleton.LoadScene(GameController.singleton.endScene);
                break;

            default:
                Debug.Log("ERROR: How did you break an enum?");
                break;
        }
    }

    private string DialogueProcessor(string input)
    {
        string output = input;

        if (currentCharacter != null)
        {
            output = output.Replace("|*CHAR*|", currentCharacter.charName);
        }
        output = output.Replace("|*PLAYER*|", GameController.singleton.playerName);

        return output;
    }
}
