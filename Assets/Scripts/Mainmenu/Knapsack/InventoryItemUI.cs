using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour {

    private Image itemImage;
    private Text itemText;
    public InventoryItem it;

    private Image ItemImage
    {
        get
        {
            if (itemImage == null)
            {
                itemImage = transform.Find("Image").GetComponent<Image>();
            }
            return itemImage;
        }
       
    }
    private Text ItemText
    {
        get
        {
            if (itemText == null)
            {
                itemText = transform.Find("Text").GetComponent<Text>();
            }
            return itemText;
        }
    }


    public void SetInventoryItem(InventoryItem it)
    {
        this.it = it;
        ItemImage.overrideSprite = Resources.Load(it.Inventory.Icon,typeof(Sprite)) as Sprite;
        if (it.Count <= 1)
        {
            ItemText.text = "";
        }
        else
        {
            ItemText.text = it.Count.ToString();
        }        
    }

    public void Clear()
    {
        it = null;
        ItemText.text = "";
        ItemImage.overrideSprite = Resources.Load("bg_道具",typeof(Sprite)) as Sprite;
    }

    public void OnPress(bool isPress)
    {        
        if (isPress == false && it != null)
        {            
            object[] objectArray = new object[3];
            objectArray[0] = it;
            objectArray[1] = true; 
            objectArray[2] = this;
            GameObject.Find("Knapsack").SendMessage("OnInventoryClick", objectArray); 
        }
    }   

    public void ChangeCount(int count)
    {
        if (it.Count - count <= 0)
        {
            Clear();
        }else if (it.Count - count == 1)
        {
            itemText.text = "";
        }
        else
        {
            itemText.text = (it.Count - count).ToString();
        }
    }

}
