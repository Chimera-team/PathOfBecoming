﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeItems : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] GameObject whatToSpawn;
    [SerializeField] string WhatToTrade;
    int choosenSlot = -1;
    [SerializeField] GameObject[] whatActivate;
    private void OnMouseDown()
    {

        for(int i = 0; i<inventory.isChosen.Length;i++)
        {
            if(inventory.isChosen[i])
            {
                choosenSlot = i;
                break;
            }
        }
        if (choosenSlot!=-1 && inventory.slots[choosenSlot].transform.GetChild(0).gameObject.name == WhatToTrade + "(Clone)")
        {
            inventory.SlotDropped(choosenSlot);
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (inventory.isFull[i] == false)
                {
                    inventory.isFull[i] = true;
                    Instantiate(whatToSpawn, inventory.slots[i].transform);
                    for(int k =0; k<whatActivate.Length;k++)
                    {
                        whatActivate[k].SetActive(true);
                    }
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
