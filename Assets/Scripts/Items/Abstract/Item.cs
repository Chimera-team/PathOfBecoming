using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    [SerializeField] Text countText;
    public Items item;

    protected Inventory inventory;

    public const string path = "Prefabs/Items/Inventory/";

    public int stack { get; protected set; }

    public int amount { get; protected set; }

    protected virtual void Set_Item_Parameters()
    {
        amount = 1;
        stack = 1;
    }

    public void Set_Item_Parameters(int amount, int stack)
    {
        this.amount = amount;
        this.stack = stack;
        Show_Amount();
    }

    public virtual void Use()
    {
        Remove_From_Stack();
    }

    protected void Show_Amount()
    {
        if (stack > 1)
            countText.text = amount.ToString();
        else
            countText.text = null;
    }

    public void Add_To_Stack(int value = 1)
    {
        amount = Mathf.Min(stack, amount += value);
        Show_Amount();
    }

    public void Remove_From_Stack(int value = 1)
    {
        if (amount < value)
            return;
        amount -= value;
        Show_Amount();
        if (amount == 0)
            inventory.Remove_From_Inventory(this);

    }

    public void Initialise(Inventory inventory)
    {
        this.inventory = inventory;
        Set_Item_Parameters();
    }

    public ItemData Get_ItemData()
    {
        return new ItemData(Regex.Replace(gameObject.name, "\\(Clone\\)", string.Empty), amount, stack);
    }
}

public enum Items
{
    Text, Book, SmallKey, MediumKey, EpicKey, EmptyVessel, FilledVessel, Special, Stackable
}
