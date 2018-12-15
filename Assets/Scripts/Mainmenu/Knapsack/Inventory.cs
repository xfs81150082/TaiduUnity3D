using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryType
{
    Equip,
    Drug,
    Box
}
public enum EquipType
{
    Helm,
    Cloth,
    Weapon,
    Shoes,
    Necklace,
    Bracelet,
    Ring,
    Wing
}

public class Inventory
{
    #region(声明和赋值)   
    private int id;                         //ID
    private string name;                    //名称
    private string icon;                    //在图集中的名称
    private InventoryType inventoryType;    //物品类型
    private EquipType equipType;            //装备类型  
    private int price = 0;                  //价格
    private int starLevel = 1;              //星级
    private int quality = 1;                //品质
    private int damage = 0;                 //伤害
    private int hp = 0;                     //生命
    private int power = 0;                  //战斗力
    private InfoType infoType;              //作用类型，表示作用在那个属性之上
    private int applyValue;                 //作用值
    private string des;                     //描述
#endregion

    #region(get&set方法)
    public int ID
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }

    }                        //ID
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }

    }                   //名称
    public string Icon
    {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }

    }                   //在图集中的名称
    public InventoryType InventoryType
    {
        get
        {
            return inventoryType;
        }
        set
        {
            inventoryType = value;
        }

    }   //物品类型
    public EquipType EquipType
    {
        get
        {
            return equipType;
        }
        set
        {
            equipType = value;
        }

    }           //装备类型  
    public int Price
    {
        get
        {
            return price;
        }
        set
        {
            price = value;
        }

    }                     //价格
    public int StarLevel
    {
        get
        {
            return starLevel;
        }
        set
        {
            starLevel = value;
        }

    }                 //星级
    public int Quality
    {
        get
        {
            return quality;
        }
        set
        {
            quality = value;
        }

    }                   //品质
    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }

    }                    //伤害
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }

    }                        //生命
    public int Power
    {
        get
        {
            return power;
        }
        set
        {
            power = value;
        }

    }                     //战斗力
    public InfoType InfoType
    {
        get
        {
            return infoType;
        }
        set
        {
            infoType = value;
        }

    }             //作用类型，表示作用在那个属性之上
    public int ApplyValue
    {
        get
        {
            return applyValue;
        }
        set
        {
            applyValue = value;
        }

    }                //作用值
    public string Des
    {
        get
        {
            return des;
        }
        set
        {
            des = value;
        }

    }                    //描述
    #endregion

}
