﻿using UnityEngine;
public class PuzzleStart : InteractEvent
{
    public GameObject puzzle;
    [SerializeField] bool create;

    [SerializeField] DialogueSystem ds;

    public override void Start_Event()
    {
        interactController.Start_Puzzle(this, create);
        ds.SetUI(false);
    }
}
