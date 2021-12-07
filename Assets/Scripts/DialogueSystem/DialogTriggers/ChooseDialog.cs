using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseDialog : DialogueTrigger
{
    [SerializeField] int[] dialogues;

    public override void StartDialogue()
    {
        Engine.current.dialogueSystem.StartDialogue(dialogues[0], onTrigger);
    }
}
