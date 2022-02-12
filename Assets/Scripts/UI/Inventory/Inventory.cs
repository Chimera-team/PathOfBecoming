﻿using System;
using System.Collections.Generic;
using UnityEngine;
using AnimationUtilsAsync.TransformUtils;
public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform _chosenItemPlace;
    [SerializeField] private Transform _heapPlace;
    [SerializeField] private ItemDescription _chosenItemDesc;


    List<Item> inventoryList = new List<Item>();

    Item _chosenItem;
    public Item chosenItem 
    { 
        get => _chosenItem;
        private set 
        {
            _chosenItem = value;
            if (_chosenItem)
                _chosenItemDesc.Set_Description(_chosenItem.description);
            else
                _chosenItemDesc.Hide_Description();
        } 
    }
    private int _chosenItemIndex = 0;

    CanvasGroup inventoryCG;

    bool _swap1 = true;
    bool _swap2 = true;

    bool _swapped
    {
        get { return _swap1 && _swap2; }
        set
        {
            _swap1 = value;
            _swap2 = value;
        }
    }

    private void Awake()
    {
        inventoryCG = GetComponent<CanvasGroup>();
    }

    public InventoryData SaveInvetnoryData()
    {
        ItemData[] items = new ItemData[inventoryList.Count];
        for (int i = 0;i< items.Length; i++)
            items[i] = inventoryList[i].Get_ItemData();
        return new InventoryData(items);
    }

    public void LoadInventoryData(InventoryData data)
    {
       for(int i = 0; i < data.items.Length; i++)
       {
            ItemData item = data.items[i];
            GameObject prefab = Resources.Load<GameObject>(Item.path + item.itemName);
            Add_To_Inventory(prefab);
            inventoryList[i].Set_Item_Parameters(item.amount, item.stack);
       }
    }

    public bool Add_To_Inventory(GameObject item)
    {
        if (!item.TryGetComponent(out Item itemScript)) //invalid item
            return false;

        int itemIndex = Get_Item_Index_By_Type(itemScript.GetType());

        if (itemIndex != -1) //we can add item to stack
        {
            inventoryList[itemIndex].Add_To_Stack();
            return true;
        }

        //create new item in inventory
        itemScript = Instantiate(item, _heapPlace).GetComponent<Item>();
        itemScript.Initialise(this);
        if (itemScript.item == Items.EyeOfHassle)
            Engine.current.Collect_Eye_Of_Hassle((EyeOfHassle)itemScript);
        inventoryList.Add(itemScript);
        return true;
    }

    public void Add_to_Inventory_Trigger(GameObject item)
    {
        Add_To_Inventory(item);
    }

    public bool Remove_From_Inventory(Item item)
    {
        int itemIndex = Get_Item_Index(item);
        if (itemIndex == -1)
            return false;
        Destroy(inventoryList[itemIndex].gameObject);
        inventoryList.RemoveAt(itemIndex);
        return true;
    }

    public void Remove_From_Inventory_Trigger(GameObject item)
    {
        Remove_From_Inventory(item.GetComponent<Item>().GetType(), true);
    }

    public bool Remove_From_Inventory(Type type, bool force = false)
    {
        int itemIndex = Get_Item_Index_By_Type(type, force);
        if (itemIndex == -1)
            return false;
        Destroy(inventoryList[itemIndex].gameObject);
        inventoryList.RemoveAt(itemIndex);
        return true;
    }

    int Get_Item_Index_By_Type(Type type, bool force = false)
    {
        for (int i = 0; i < inventoryList.Count; i++)
            if (inventoryList[i].GetType() == type && 
                (inventoryList[i].amount < inventoryList[i].stack || force))
                return i;
        return -1;
    }

    int Get_Item_Index(Item item)
    {
        for (int i = 0; i < inventoryList.Count; i++)
            if (inventoryList[i] == item)
                return i;
        return -1;
    }

    public void Enable_Inventory()
    {
        inventoryCG.alpha = 1;
        inventoryCG.blocksRaycasts = true;
        inventoryCG.interactable = true;
    }

    public void Scroll_Inventory(int direction)
    {
        int items = inventoryList.Count;
        if (items == 0)
        {
            _chosenItemIndex = 0;
            chosenItem = null;
            return;
        }
        else if (items == 1)
        {
            return;
        }
        if (!_swapped)
        return;

        _swapped = false;
        chosenItem.transform.Move_To(_heapPlace.position, 0.2f, () => _swap1 = true);
        _chosenItemIndex += direction;
        if (_chosenItemIndex >= inventoryList.Count)
            _chosenItemIndex -= inventoryList.Count;
        else if (_chosenItemIndex < 0)
            _chosenItemIndex += inventoryList.Count;
        chosenItem = inventoryList[_chosenItemIndex];
        chosenItem.transform.Move_To(_chosenItemPlace.position, 0.2f, () => _swap2 = true);
    }

    public void Open_Inventory()
    {
        if (inventoryList.Count == 0 || !inventoryCG.interactable)
            return;
        _chosenItemIndex = 0;
        _swapped = false;
        chosenItem = inventoryList[_chosenItemIndex];
        chosenItem.transform.Move_To(_chosenItemPlace.position, 0.2f, () => _swapped = true);
        Engine.current.playerController.Change_Controls<InventoryHandler>();
    }

    public void Close_Inventory()
    {
        if (!_swapped)
            return;
        chosenItem.transform.Move_To(_heapPlace.position, 0.2f, () => _swapped = true);
        if (Engine.current.playerController.buttonsControl is InventoryHandler)
            Engine.current.playerController.Change_Controls<DefaultHandler>();
        chosenItem = null;
    }
}
