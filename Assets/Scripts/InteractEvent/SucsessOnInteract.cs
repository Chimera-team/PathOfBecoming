
public class SucsessOnInteract : InteractEvent
{
    public override void Start_Event()
    {
        if (Engine.current.playerController.buttonsControl is InventoryHandler)
            onFail?.Invoke();
        else
            onSuccess?.Invoke();
    }
}
