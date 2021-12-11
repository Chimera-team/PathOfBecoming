using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using AnimationUtils.TransformUtils;

public class Spellbook : MonoBehaviour
{
    [SerializeField] private Transform _chosenItemPlace;
    [SerializeField] private Transform _heapPlace;

    CanvasGroup spellBookCG;

    List<Spell> spellList = new List<Spell>();

    public Spell chosenSpell { get; private set; }
    private int _chosenSpellIndex = 0;

    private float _castAngle 
    { 
        set
        {
            Engine.current.playerController.spellDirection.localRotation = Quaternion.Euler(0, 0, value);
        }
        get
        {
            return Engine.current.playerController.spellDirection.rotation.eulerAngles.z;
        }
    }
    private const float angleChange = 45f;

    private void Awake()
    {
        spellBookCG = GetComponent<CanvasGroup>();
    }

    public void Learn_Spell(GameObject prefab)
    {
        Spell spell = Instantiate(prefab, _heapPlace).GetComponent<Spell>();
        spellList.Add(spell);
        if (spellList.Count == 1)
        {
            spell.transform.Move_To(_chosenItemPlace.position, 0.2f, false);
            chosenSpell = spell;
        }
    }

    public void Scroll_Book()
    {
        if (spellList.Count <= 1)
            return;
        chosenSpell.transform.Move_To(_heapPlace.position, 0.2f, false);
        ++_chosenSpellIndex;
        if (_chosenSpellIndex == spellList.Count)
            _chosenSpellIndex -= spellList.Count;
        chosenSpell = spellList[_chosenSpellIndex];
        chosenSpell.transform.Move_To(_chosenItemPlace.position, 0.2f, false);
    }

    public void Start_Casting()
    {
        Engine.current.playerController.Change_Controls<CastingHandler>();
    }

    public void Cast()
    {
        Engine.current.playerController.Change_Controls<DefaultHandler>();
        chosenSpell.Cast(Engine.current.playerController.firePoint.position, _castAngle);
        Engine.current.playerController.spellDirection.gameObject.SetActive(false);
    }

    public void Enable_Spellbook()
    {
        spellBookCG.alpha = 1;
        spellBookCG.blocksRaycasts = true;
        spellBookCG.interactable = true;
    }

    public void Reset_Angle()
    {
        Engine.current.playerController.spellDirection.gameObject.SetActive(true);
        _castAngle = 0f;
    }

    public void Change_Cast_Angle(int direction)
    {
        _castAngle += angleChange * direction;
        if (_castAngle > 90)
            _castAngle = Mathf.Clamp(_castAngle, 360 - angleChange, 360);
        else
            _castAngle = Mathf.Clamp(_castAngle, 0, angleChange);
    }

    public void Load_State(SpellBookData data)
    {
        for (int i = 0; i < data.spellNames.Length; i++)
        {
            GameObject prefab = Resources.Load<GameObject>(Spell.path + data.spellNames[i]);
            Learn_Spell(prefab);
        }
    }

    public SpellBookData Save_State()
    {
        string[] data = new string[spellList.Count];
        for (int i = 0; i < data.Length; i++)
            data[i] = Regex.Replace(spellList[i].name, "\\(Clone\\)", string.Empty);
        return new SpellBookData(data);
    }
}
