using UnityEngine;

public class AnchootkaInteract : PlaceForItem
{
    [SerializeField] Animation anim;

    public void Disappear()
    {
        anim.Play();
    }
}
