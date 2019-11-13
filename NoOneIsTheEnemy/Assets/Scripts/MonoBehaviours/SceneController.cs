using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public Character sceneChar;

    // Start is called before the first frame update
    void Start()
    {
        sceneChar.StartDialogue();
    }
}
