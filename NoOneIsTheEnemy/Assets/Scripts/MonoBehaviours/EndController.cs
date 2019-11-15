using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndController : MonoBehaviour
{
    public Text endText;
    private bool finished;

    [TextArea(3, 10)]
    public string zeroMessage, allMessage, betweenMessage;

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
        else if (!finished)
        {
            int numRomanced = 0;

            foreach (Character character in GameController.singleton.characters)
            {
                if (character.relationship >= 100)
                {
                    numRomanced++;
                }
            }

            if (numRomanced == 0)
            {
                string message = zeroMessage;
            }
            else if (numRomanced == GameController.singleton.characters.Length)
            {
                string message = allMessage;
            }
            else
            {
                string message = betweenMessage;
            }
        }

        if (Input.GetButtonUp("Submit"))
        {
            GameController.singleton.LoadScene(0);
        }
    }
}
