using PlayerControls;

public class CastingHandler : ControlHandler
{
    public override void Down()
    {
        if(ControlButtonsPress.DOWN)
            Interface.current.spellBook.Change_Cast_Angle(ControlButtonsAxis.yAxisRaw);
    }

    public override void Interact()
    {
        return;
    }

    public override void Inventory_Button()
    {
        return;
    }

    public override void Left()
    {
        return;
    }

    public override void Right()
    {
        return;
    }

    public override void Switch_Spell()
    {
        return;
    }

    public override void Up()
    {
        if (ControlButtonsPress.UP)
            Interface.current.spellBook.Change_Cast_Angle(ControlButtonsAxis.yAxisRaw);
    }

    public override void Use_Spell()
    {
        if (!ControlButtonsHold.USESPELL)
            Interface.current.spellBook.Cast();
    }
}
