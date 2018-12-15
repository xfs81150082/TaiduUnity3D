using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    public static InventoryUI _instance;

    private Text countText;

    private int count = 0;

    public List<InventoryItemUI> itemUIList = new List<InventoryItemUI>();

    private void Awake()
    {
        _instance = this;
        countText = transform.Find("CountText").GetComponent<Text>();
        InventoryManager._instance.OnInventoryChange += this.OnInventoryChange;
        

    }
    private void OnDestroy()
    {
        InventoryManager._instance.OnInventoryChange -= this.OnInventoryChange;
    }

    void OnInventoryChange()
    {
        UpdateShow();
    }
    void UpdateShow()
    {
        int temp = 0;
        for (int i = 0; i < InventoryManager._instance.inventoryItemList.Count; i++)
        {            
            InventoryItem it = InventoryManager._instance.inventoryItemList[i];   
            if (it.IsDressed == false)
            {
                itemUIList[temp].SetInventoryItem(it);//放入背包
                temp++;
            }           
        }
        count = temp;
        for (int i = temp; i < itemUIList.Count; i++)
        {           
            itemUIList[i].Clear();
        }
        countText.text = count + "/32";
    }

    public void UpdateCount()
    {
        count = 0;
        foreach (InventoryItemUI itUI in itemUIList)
        {
            if(itUI.it != null)
            {
                count++;
            }
        }
        countText.text = count + "/32";
    }

    //TOTO
    public void AddInvenetoryItem(InventoryItem it)
    {
        foreach(InventoryItemUI itUI in itemUIList)
        {
            if (itUI.it == null)
            {
                itUI.SetInventoryItem(it);
                count++;
                break;
            }
        }
        countText.text = count + "/32";
    }

    public void OnClearUp()
    {
        UpdateShow();

    }



}
