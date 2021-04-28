﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceToStartPuzzle : MonoBehaviour
{
    ///
    /// проверка места где стоит игрок давать ли возможность запустить головоломку
    ///
    [SerializeField] PuzzleStart PS;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            PS.canUStart = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
            PS.canUStart = false;
    }
}