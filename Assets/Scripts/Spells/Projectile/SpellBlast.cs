using System.Collections;
using UnityEngine;

public class SpellBlast : MonoBehaviour
{
    //Spell projectile
    [SerializeField] float speed;
    [SerializeField] float timeToLive;
    public SpellType spellType { get; private set; }


    private void Awake()
    {
        StartCoroutine(Time_To_Live());
        spellType = Interface.current.spellBook.chosenSpell.spellType;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    IEnumerator Time_To_Live()
    {
        yield return new WaitForSeconds(timeToLive);
        Destroy(gameObject);
    }
}
