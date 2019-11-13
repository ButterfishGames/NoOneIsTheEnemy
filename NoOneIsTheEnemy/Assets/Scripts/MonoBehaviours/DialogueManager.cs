using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager singleton;

    public GameObject branchButtonPrefab;

    private Queue<string> sentences;
    private Queue<string> names;

    private Character currentCharacter;
    private Dialogue currentDialogue;

    private bool primed;

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

        currentCharacter = null;
        currentDialogue = null;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit") && EventSystem.current.currentSelectedGameObject == null)
        {
            primed = true;
        }

        if (Input.GetButtonUp("Submit") && EventSystem.current.currentSelectedGameObject == null && primed)
        {
            DisplayNextLine();
            primed = false;
        }
    }

    public void StartDialogue(Character character, Dialogue dialogue)
    {
        sentences.Clear();
        names.Clear();

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

        GameObject.Find("NameText").GetComponent<Text>().text = name;
        GameObject.Find("LineText").GetComponent<Text>().text = line;

        if (currentDialogue.type == DialogueType.branch && sentences.Count == 0)
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        switch (currentDialogue.type)
        {
            case DialogueType.end:
                Debug.Log("Dialogue over");
                currentDialogue = null;
                currentCharacter = null;
                GameController.singleton.LoadScene(GameController.singleton.mapScene);
                break;

            case DialogueType.branch:
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

            default:
                Debug.Log("ERROR: How did you break an enum?");
                break;
        }
    }

    private string DialogueProcessor(string input)
    {
        string output = input;

        output = output.Replace("|*CHAR*|", currentCharacter.charName);
        output = output.Replace("|*PLAYER*|", GameController.singleton.playerName);

        return output;
    }
}
