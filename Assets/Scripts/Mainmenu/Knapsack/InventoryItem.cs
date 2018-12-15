using System.Collections;
using System.Collections.Generic;
using TumoCommon.Model;
using UnityEngine;

public class InventoryItem
{
    #region ()

    public InventoryItemDB inventoryItemDB;
    private Inventory inventory;
    private int level;
    private int count;
    private bool isDressed = false;
    #endregion ()

    #region(get&set方法)

    public InventoryItem()
    {
        
    }

    public InventoryItem(InventoryItemDB itemDB)
    {
        this.inventoryItemDB = itemDB;
        Inventory inventoryTemp;
        InventoryManager._instance.inventoryDict.TryGetValue(itemDB.InventoryId, out inventoryTemp);
        inventory = inventoryTemp;
        level = itemDB.Level;
        Count = itemDB.Count;
        IsDressed = itemDB.IsDressed;
    }

    public InventoryItemDB CreatInventoryItemDB()
    {
        inventoryItemDB = new InventoryItemDB();
        inventoryItemDB.Count = Count;
        inventoryItemDB.InventoryId = inventory.ID;
        inventoryItemDB.IsDressed = IsDressed;
        inventoryItemDB.Level = level;
        return inventoryItemDB;
    }

    public InventoryItemDB InventoryItemDB
    {
        get{ return inventoryItemDB; }
    }

    public Inventory Inventory
    {
        get
        {
            return inventory;
        }
        set
        {
            inventory = value;
        }
    }
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            inventoryItemDB.Level = level;
        }
    }
    public int Count
    {
        get
        {
            return count;
        }
        set
        {
            count = value;
            inventoryItemDB.Count = Count;
        }
    }
    public bool IsDressed
    {
        get
        {
            return isDressed;
        }
        set
        {
            isDressed = value;
            inventoryItemDB.IsDressed = isDressed;
        }
    }
    #endregion ()

}
