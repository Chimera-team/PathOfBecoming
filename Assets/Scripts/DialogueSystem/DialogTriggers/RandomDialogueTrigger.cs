using UnityEngine;

public class RandomDialogueTrigger : DialogueTrigger
{
    [SerializeField] int[] dialogues;

    public override void StartDialogue()
    {
        Engine.current.dialogueSystem.StartDialogue(dialogues[Random.Range(0, dialogues.Length)], onTrigger);
    }
}
