using PlayerControls;

public class InventoryHandler : ControlHandler
{
    public override void Down()
    {
        return;
    }

    public override void Interact()
    {
        if (InteractEvent.current && ControlButtonsPress.INTERACT)
        {
            InteractEvent.current.Start_Event();
            Interface.current.inventory.Close_Inventory();
        }
    }

    public override void Inventory_Button()
    {
        if (ControlButtonsPress.DOWN)
            Interface.current.inventory.Close_Inventory();
    }

    public override void Left()
    {
        if (ControlButtonsPress.LEFT)
            Interface.current.inventory.Scroll_Inventory(ControlButtonsAxis.xAxisRaw);
    }

    public override void Right()
    {
        if (ControlButtonsPress.RIGHT)
            Interface.current.inventory.Scroll_Inventory(ControlButtonsAxis.xAxisRaw);
    }

    public override void Switch_Spell()
    {
        return;
    }

    public override void Up()
    {
        return;
    }

    public override void Use_Spell()
    {
        return;
    }
}
