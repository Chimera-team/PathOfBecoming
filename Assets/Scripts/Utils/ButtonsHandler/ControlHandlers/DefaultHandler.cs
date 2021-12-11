using PlayerControls;

public class DefaultHandler : ControlHandler
{
    public override void Down()
    {
        return;
    }

    public override void Interact()
    {
        if (InteractEvent.current && ControlButtonsPress.INTERACT)
            InteractEvent.current.Start_Event();
    }

    public override void Inventory_Button()
    {
        if(ControlButtonsPress.DOWN)
            Interface.current.inventory.Open_Inventory();
    }

    public override void Left()
    {
        Engine.current.playerController.Move(ControlButtonsAxis.xAxisRaw);
    }

    public override void Right()
    {
        return;
    }

    public override void Switch_Spell()
    {
        if (ControlButtonsPress.SWITCHSPELL)
            Interface.current.spellBook.Scroll_Book();
    }

    public override void Up()
    {
        if(ControlButtonsPress.UP)
            Engine.current.playerController.Jump();
    }

    public override void Use_Spell()
    {
        if (ControlButtonsPress.USESPELL)
        {
            Interface.current.spellBook.Reset_Angle();
            Interface.current.spellBook.Start_Casting();
        }
    }
}
