using PlayerControls;

public class PuzzleHandler : ControlHandler
{
    public override void Down()
    {
        return;
    }

    public override void Interact()
    {
        if (ControlButtonsPress.INTERACT)
            Puzzle.current.Solve_Puzzle();
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
        if (ControlButtonsPress.SWITCHSPELL)
            Puzzle.current.Fail_Puzzle();
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
