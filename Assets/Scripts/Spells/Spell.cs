using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Spell : MonoBehaviour
{
    //Spell in spellbook
    public float manaCost;
    public SpellType spellType;
    public float cooldown;

    public GameObject projectile;

    public const string path = "Prefabs/Magic/Spells/";

    bool onCooldown = false;
    Image spellImage;

    private void Start()
    {
        spellImage = GetComponent<Image>();
    }

    public void Cast(Vector3 firePoint, float angle)
    {   
        if (onCooldown || !Interface.current.mana.SpellShot(manaCost))
            return;
        Instantiate(projectile, firePoint, Quaternion.Euler(0,Engine.current.player.transform.GetChild(0).rotation.eulerAngles.y, angle));
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        float _endTime = Time.time + cooldown;
        onCooldown = true;
        do
        {
            spellImage.fillAmount = 1 - ((_endTime - Time.time) / cooldown);
            yield return null;
        }
        while (spellImage.fillAmount < 1);
        onCooldown = false;
    }
}

public enum SpellType { Wind, Fire, Ice, Levitation }

