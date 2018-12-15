using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knapsack : MonoBehaviour {
    public static Knapsack _instance;

    private Text priceText;
    private Button chuosheButton;

    private EquipPopup equipPopup;
    private InventoryPopup inventoryPopup;
    private InventoryItemUI itUI;

    


    private void Awake()
    {
        _instance = this;
        priceText = transform.Find("InventoryImage/JiaGeText").GetComponent<Text>();
        chuosheButton= transform.Find("InventoryImage/ChuoSheImage").GetComponent<Button>();
        equipPopup = GameObject.Find("EquipPopupImage").GetComponent<EquipPopup>();
        inventoryPopup = GameObject.Find("InventoryPopupImage").GetComponent<InventoryPopup>();

    }
    private void Start()
    {
        DisableButton();
        
    }

    public void OnInventoryClick(object[] objectArray)
    {
        InventoryItem it = objectArray[0] as InventoryItem;
        bool isLeft = (bool)objectArray[1];

        if (it.Inventory.InventoryType == InventoryType.Equip)
        {
            InventoryItemUI itUI = null;
            KnapsackRoleEquip roleEquip = null;
           
            if (isLeft)
            {
                itUI = objectArray[2] as InventoryItemUI;
            }
            else
            {
                 roleEquip = objectArray[2] as KnapsackRoleEquip;
            }
            inventoryPopup.Close();
            equipPopup.Show(it, itUI,roleEquip, isLeft);
            
        }
        else
        {
            InventoryItemUI itUI = objectArray[2] as InventoryItemUI;
            equipPopup.Close();
            inventoryPopup.Show(it, itUI);
        }

        if (it.Inventory.InventoryType == InventoryType.Equip && isLeft == true || it.Inventory.InventoryType != InventoryType.Equip) 
        {
            this.itUI = objectArray[2] as InventoryItemUI;
            EnableButton();
            priceText.text = (this.itUI.it.Inventory.Price*this.itUI.it.Count).ToString();
        }

    }

    private void DisableButton()
    {
        priceText.text = "";
        //chuosheButton.interactable = false;
        chuosheButton.enabled = false;
        
    }
    private void EnableButton()
    {
        //chuosheButton.interactable = true;
        chuosheButton.enabled = true;
    }

    public void OnSale()
    {
        int price = int.Parse(priceText.text);
        PlayerInfo.Intance.AddCoin(price);
        InventoryManager._instance.RemoveInventoryItem(itUI.it);
        itUI.Clear();

        equipPopup.Close();
        inventoryPopup.Close();
        DisableButton();
    }

    public void Show()
    {

    }
    public void HideSelf()
    {

    }

}
