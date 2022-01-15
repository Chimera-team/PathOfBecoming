﻿using UnityEngine;
using UnityEngine.UI;
using System;

public class Interface : MonoBehaviour
{
    [SerializeField] protected Engine engine;
    public Image curtain;
    public Transform canvas;
    public Inventory inventory;
    public Spellbook spellBook;
    public ManaCounter mana;
    public Image healthBar;
    public CanvasGroup interfacePanel;
    public ChoicePanel choicePanel;
    public CanvasGroup eyeOfHassle;
    public static Interface current;

    private void Awake()
    {
        current = this;
    }

    public void Spawn_UI_Object(GameObject prefab)
    {
        Instantiate(prefab, canvas);
    }

    public void Enable_Interface(bool enable)
    {
        interfacePanel.alpha = Convert.ToInt32(enable);
        interfacePanel.interactable = enable;
        interfacePanel.blocksRaycasts = enable;
    }
}
