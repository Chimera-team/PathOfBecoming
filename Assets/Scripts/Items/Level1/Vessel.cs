using UnityEngine;

public class Vessel : Default
{
    [SerializeField] GameObject addToInventory;

    public override void Use()
    {
        inventory.Add_To_Inventory(addToInventory);
        base.Use();
    }
}
