using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class KnapsackRoleEquip : MonoBehaviour {

    private InventoryItem it;

    private Image _enghtImage;
    private Image EnghtImage
    {
        get
        {
            if (_enghtImage == null)
            {
                _enghtImage = this.GetComponent<Image>();
            }
            return _enghtImage;
        }
    }


    private void Awake()
    {
       
    }

    public void SetID(int id)
    {
        Inventory inventory = null;
        bool isExit = InventoryManager._instance.inventoryDict.TryGetValue(id, out inventory);
        if (isExit)
        {
            //EnghtImage.overrideSprite.name = inventory.Icon; //NGUI上对应的原码
            EnghtImage.overrideSprite = Resources.Load(inventory.Icon, typeof(Sprite)) as Sprite;           
        }
    }
    public void SetInventoryItem(InventoryItem it)
    {
        if (it == null) return;
        this.it = it;
        EnghtImage.overrideSprite = Resources.Load(it.Inventory.Icon, typeof(Sprite)) as Sprite;
    }

    public void Clear()
    {
        it = null;
        EnghtImage.overrideSprite = Resources.Load("bg_道具", typeof(Sprite)) as Sprite;
    }

    public void OnPress(bool isPress)    {      

        if (isPress == false && it != null)
        {        
            object[] objectArray = new object[3];
            objectArray[0] = it;
            objectArray[1] = false;
            objectArray[2] = this;            
            GameObject.Find("Knapsack").SendMessage("OnInventoryClick", objectArray);          

        }
    }


}
