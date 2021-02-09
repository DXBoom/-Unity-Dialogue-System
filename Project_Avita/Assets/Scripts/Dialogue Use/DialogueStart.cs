using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStart : MonoBehaviour
{
    private DialogueTalk dialogueTalk;

    public void Awake()
    {
        dialogueTalk = GetComponent<DialogueTalk>();
    }

    public void Start()
    {
        dialogueTalk.StartDialogue();
    }
}
