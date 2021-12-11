using System.Collections.Generic;
using UnityEngine;

public class CompositeDialogueTrigger : DialogueTrigger
{
    [SerializeField] private List<DialogueTrigger> _dialogueList;

    public override void StartDialogue()
    {
        _dialogueList[0].StartDialogue();
        if (_dialogueList.Count != 1)
            _dialogueList.RemoveAt(0);
    }
}
