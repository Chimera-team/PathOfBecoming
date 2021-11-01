
using UnityEngine;

public class Level2 : Engine
{
    protected override void Start_Level()
    {
        GameObject player = Resources.Load<GameObject>("Prefabs/Characters/Player(Lit)");
        GameObject fairy = Resources.Load<GameObject>("Prefabs/Characters/Fairy(Lit)");
        Spawn_Characters(player, fairy);
        Connect_Fairy_to_Player();
        userInterface.spellBook.Learn_Spell(Resources.Load<GameObject>("Prefabs/Magic/Spells/Lighter(Spell)"));
userInterface.spellBook.Learn_Spell(Resources.Load<GameObject>("Prefabs/Magic/Spells/Whirlwind(Spell)"));

        Show_Scene(() => {
            userInterface.inventory.Enable_Inventory();
            userInterface.spellBook.Enable_Spellbook();
            dialogueSystem.SetUI(true);
            });
    }
}
