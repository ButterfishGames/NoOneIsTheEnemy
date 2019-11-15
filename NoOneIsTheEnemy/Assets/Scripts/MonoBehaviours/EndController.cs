using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndController : MonoBehaviour
{
    public Text endText;
    private bool finished;

    private void Start()
    {
        finished = false;
    }

    private void Update()
    {
        if (GameController.singleton.dead && DialogueManager.singleton != null && !finished)
        {
            string message = DialogueManager.singleton.currentCharacter.deaths[Random.Range(0, DialogueManager.singleton.currentCharacter.deaths.Length)];
            endText.text = message;
            finished = true;
        }

        if (Input.GetButtonUp("Submit"))
        {
            GameController.singleton.LoadScene(0);
        }
    }
}
