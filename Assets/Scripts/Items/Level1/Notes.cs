using UnityEngine;
public class Notes : Item
{
    public GameObject expanded;

    public override void Use()
    {
        Interface.current.Spawn_UI_Object(expanded);
    }
}
