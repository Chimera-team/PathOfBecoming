using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class PlaceForItem : InteractEvent
{
    [SerializeField] Transform placePosition;
    [SerializeField] DialogueTrigger failDialog;
    [SerializeField] DialogueTrigger noItemDialog;
    [SerializeField] ItemExecusion[] interactWith;

    bool attachedItem = false;

    public override void Start_Event()
    {
        if(!Interface.current.inventory.chosenItem && noItemDialog)
        {
            Examine();
            return;
        }
        if (!Interface.current.inventory.chosenItem || !(Engine.current.playerController.buttonsControl is InventoryHandler))
        {
            Cannot_Interact();
            return;
        }
        foreach (ItemExecusion item in interactWith)
            if (item.item == Interface.current.inventory.chosenItem.item)
            {
                Interface.current.inventory.chosenItem.Use();
                item.execution?.Invoke();
                onSuccess?.Invoke();
                return;
            }
        Cannot_Interact();
    }

    void Cannot_Interact()
    {
        if(failDialog)
            failDialog.StartDialogue();
        onFail?.Invoke();
    }

    private void Examine()
    {
        noItemDialog.StartDialogue();
    }

    public void Place_Item(GameObject item)
    {
        if (placePosition && !attachedItem)
        {
            GameObject clone = Instantiate(item, placePosition);
            if (clone.TryGetComponent(out Collectible staff))
                staff.Attach(this);
            attachedItem = true;
        }
    }

    public virtual void On_Detach()
    {
        attachedItem = false;
    }
}

[Serializable]
class ItemExecusion
{
    public Items item;
    public UnityEvent execution;
}
