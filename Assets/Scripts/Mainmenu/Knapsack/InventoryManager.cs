using System.Collections;
using System.Collections.Generic;
using TumoCommon.Model;
using TumoCommon.Tools;
using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    #region(各种声明)
    public static InventoryManager _instance;
    public TextAsset listinfo;
    public Dictionary<int, Inventory> inventoryDict = new Dictionary<int, Inventory>();    
    public List<InventoryItem> inventoryItemList = new List<InventoryItem>();

    public delegate void OnInventoryChangeEvent();
    public event OnInventoryChangeEvent OnInventoryChange;

    private InventoryItemDBController inventoryItemDbController;


    #endregion()

    private void Awake()
    {
        _instance = this;
        inventoryItemDbController = this.GetComponent<InventoryItemDBController>();
        inventoryItemDbController.OnGetInventoryItemDBList += this.OnGetInventoryItemDBList;
        inventoryItemDbController.OnAddInventoryItemDB += this.OnAddInventoryItemDB;
        ReadInventoryInfo();       
    }
    void Start()
    {        
        ReadInventotyItemInfo();
    }

    void Update()
    {
        PickUp();
    }

    void ReadInventoryInfo()
    {
        string str = listinfo.ToString();        
        string[] itemStrArray = str.Split('\n');
        foreach (string itemstr in itemStrArray)
        {
            //ID 名称 图标 类型（Equip，Drug,Box） 装备类型(Helm,Cloth,Weapon,Shoes,Necklace,Bracelet,Ring,Wing)
            string[] proArray = itemstr.Split('|');
            Inventory inventory = new Inventory();
            inventory.ID = int.Parse(proArray[0]);
            inventory.Name = proArray[1];
            inventory.Icon = proArray[2];
            switch (proArray[3])
            {
                case "Equip":
                    inventory.InventoryType = InventoryType.Equip;
                    break;
                case "Drug":
                    inventory.InventoryType = InventoryType.Drug;
                    break;
                case "Box":
                    inventory.InventoryType = InventoryType.Box;
                    break;
            }
            if (inventory.InventoryType == InventoryType.Equip)
            {
                switch (proArray[4])
                {
                    case "Helm":
                        inventory.EquipType = EquipType.Helm;
                        break;
                    case "Cloth":
                        inventory.EquipType = EquipType.Cloth;
                        break;
                    case "Weapon":
                        inventory.EquipType = EquipType.Weapon;
                        break;
                    case "Shoes":
                        inventory.EquipType = EquipType.Shoes;
                        break;
                    case "Necklace":
                        inventory.EquipType = EquipType.Necklace;
                        break;
                    case "Bracelet":
                        inventory.EquipType = EquipType.Bracelet;
                        break;
                    case "Ring":
                        inventory.EquipType = EquipType.Ring;
                        break;
                    case "Wing":
                        inventory.EquipType = EquipType.Wing;
                        break;
                }

            }
            //print(itemstr);
            //信价 星级 品质 伤害 生命 战吉力 作用类型 描述
            inventory.Price = int.Parse(proArray[5]);
            if (inventory.InventoryType == InventoryType.Equip)
            {
                inventory.StarLevel = int.Parse(proArray[6]);
                inventory.Quality = int.Parse(proArray[7]);
                inventory.Damage = int.Parse(proArray[8]);
                inventory.HP = int.Parse(proArray[9]);
                inventory.Power = int.Parse(proArray[10]);
            }
            if (inventory.InventoryType == InventoryType.Drug)
            {
                inventory.ApplyValue = int.Parse(proArray[12]);
            }
            inventory.Des = proArray[13];
            inventoryDict.Add(inventory.ID, inventory);
        }
        
    }//读取文本类型数据，并管理成字典和数组

    void ReadInventotyItemInfo()
    {
        //TOTO需要从服务器上得到信息 更新
        
        //随机生成主角拥用的物品
        /*
        ....
        */

        inventoryItemDbController.GetInventoryItemDB();

        

    }//完成角色背包信息的初始化，和角色随机拥有的物品
     //TOTO需要从服务器上得到信息 更新

    void PickUp()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            int id = Random.Range(1001, 1020);
            Inventory i = null;
            inventoryDict.TryGetValue(id, out i);
            if (i.InventoryType == InventoryType.Equip)
            {
                //先把物品加到服务器，然后再调到UI
                InventoryItemDB itemDb = new InventoryItemDB();
                itemDb.InventoryId = id;
                itemDb.Count = 1;
                itemDb.IsDressed = false;
                itemDb.Level = Random.Range(1, 10);
                inventoryItemDbController.AddInventoryItemDB(itemDb);
            }
            else
            {
                //先判断背包里面此药品是否存在
                InventoryItem it = null;
                bool isExit = false;
                foreach (InventoryItem temp in inventoryItemList)
                {
                    if (temp.Inventory.ID == id)
                    {
                        isExit = true;
                        it = temp;
                        break;
                    }
                }
                if (isExit)
                {
                    it.Count++; 
                    //ToTo同步inventoryItemDB  进行Update
                    inventoryItemDbController.UpdateInventoryItemDB(it.InventoryItemDB);

                }
                else
                {
                    InventoryItemDB itemDb = new InventoryItemDB();
                    itemDb.InventoryId = id;
                    itemDb.Count = 1;
                    itemDb.IsDressed = false;
                    itemDb.Level = Random.Range(1, 10);
                    inventoryItemDbController.AddInventoryItemDB(itemDb);

                }

            }
        }
    }

    public void OnAddInventoryItemDB(InventoryItemDB itemDb)
    {
        InventoryItem it = new InventoryItem(itemDb);
        inventoryItemList.Add(it);

        OnInventoryChange();
    }

    public void OnGetInventoryItemDBList(List<InventoryItemDB> list)
    {
        foreach (var itemDb in list)
        {
            InventoryItem it = new InventoryItem(itemDb);
            inventoryItemList.Add(it);
        }

        OnInventoryChange();
    }

    public void RemoveInventoryItem(InventoryItem it)
    {
        this.inventoryItemList.Remove(it);
    }

    public void UpgradeEquip(InventoryItem it)
    {
       inventoryItemDbController.UpgradeEquip(it.InventoryItemDB);
        
    }

    void OnDestroy()
    {
        if (inventoryItemDbController != null)
        {
            inventoryItemDbController.OnGetInventoryItemDBList -= this.OnGetInventoryItemDBList;
        }
    }

}
