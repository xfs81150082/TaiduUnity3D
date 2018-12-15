using System.Collections;
using System.Collections.Generic;
using TumoCommon.Model;
using UnityEngine;
using UnityEngine.UI;

public enum InfoType
{
    Name,
    HeadPortrait,
    Level,
    Power,
    Exp,
    Diamond,
    Coin,
    Energy,
    Toughen,
    HP,
    Damage,
    Equip,
    All
}

public enum PlayerType
{
    Warrior,                    //战士
    FemaleAssassin              //刺客
}

public class PlayerInfo : MonoBehaviour {
    private static PlayerInfo _intance;

    #region  property 属性

    private string _headPortrait;   //头像
    private string _name;           //姓名
    private int _level = 1;         //等级
    private int _power = 1;         //战斗力
    private int _exp = 0;           //经验数
    private int _diamond;           //钻石数
    private int _coin;              //金币数
    private int _energy;            //体力数
    private int _toughen;           //历练数
    private int _hp;                //生命值，后增
    private int _damage;            //伤害值，后增
    private PlayerType _playerType; //角色类型


    public InventoryItem helmInventoryItem;
    public InventoryItem clothInventoryItem;
    public InventoryItem weaponInventoryItem;
    public InventoryItem shoesInventoryItem;
    public InventoryItem necklaceInventoryItem;
    public InventoryItem braceletInventoryItem;
    public InventoryItem ringInventoryItem;
    public InventoryItem wingInventoryItem;




    #endregion

    #region
    public float energyTimer = 0;
    public float toughenTimer = 0;
    
    public delegate void OnPlayerInfoChangedEvent(InfoType type);
    public event OnPlayerInfoChangedEvent OnPlayerInfoChanged;

    private RoleController roleController;
    private InventoryItemDBController inventoryItemDbController;

    #endregion

    #region get&set方法

    public string HeadPortrait
    {
        get
        {
            return _headPortrait;
        }
        set
        {
            _headPortrait = value;
        }
    }
    public string Name
    {
        get {
            return _name;
        }
        set
        {
            _name = value;
        }
    }
    public int Level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = value;
        }
        
    }
    public int Power
    {
        get
        {
            return _power;
        }
        set
        {
            _power = value;
        }
    }
    public int Exp
    {
        get
        {
            return _exp;
        }
        set
        {
            _exp = value;
        }
    }
    public int Diamond
    {
        get
        {
            return _diamond;
        }
        set
        {
            _diamond = value;
        }
    }
    public int Coin
    {
        get
        {
            return _coin;
        }
        set
        {
            _coin = value;
        }
    }
    public int Energy
    {
        get
        {
            return _energy;
        }
        set
        {
            _energy = value;
        }
    }
    public int Toughen
    {
        get
        {
            return _toughen;
        }
        set
        {
            _toughen = value;
        }
    }
    public int HP
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;
        }
    }
    public int Damage
    {
        get
        {
            return _damage;
        }
        set
        {
            _damage = value;
        }
    }
    public PlayerType PlayerType
    {
        get
        {
            return _playerType;
        }
        set
        {
            _playerType = value;
        }
    }

    public static PlayerInfo Intance
    {
        get
        {
            return _intance;
        }

        set
        {
            _intance = value;
        }
    }



    #endregion


    #region(awake,start,update...) 

    private void Awake()
    {
        Intance = this;
        this.OnPlayerInfoChanged += OnPlayerInfoChange;
        
        roleController = this.GetComponent<RoleController>();
        inventoryItemDbController = this.GetComponent<InventoryItemDBController>();
    }

    // Use this for initialization
    void Start ()
    {
        Init();
        InventoryManager._instance.OnInventoryChange += this.OnInventoryChange;
	}
	
	// Update is called once per frame
	void Update () {   
        EnergyToughenRecovery();
	}
    #endregion 


 
    #region  方法集合
    void Init()
    {    
        //下列信息在服务器中取得
        Role role = PhotonEngine.Instance.role;
        this.Name = role.Name;
        this.Level = role.Level;
        this.Exp = role.Exp;
        this.Diamond = role.Diamond;  
        this.Coin = role.Coin;        
        this.Energy = role.Energy;
        this.Toughen = role.Toughen;
        if (role.IsMan)
        {
            this.HeadPortrait = "BoyImage";
            _playerType = PlayerType.Warrior;
        }
        else
        {
            this.HeadPortrait = "GirlImage";
            _playerType = PlayerType.FemaleAssassin;
        }

        InitHPDamagePower();

        OnPlayerInfoChanged(InfoType.All);

    }  //属性初始化(应从服务器中调数据)
    
    void InitHPDamagePower()  //(初始血伤害)
    {
        this.HP = this.Level * 100;
        this.Damage = this.Level * 50;
        this.Power = this.HP + this.Damage;
    }

    void EnergyToughenRecovery()
    {
        if (this.Energy < 100)
        {
            energyTimer += Time.deltaTime;
            if (energyTimer > 60)
            {
                this.Energy += 1;
                energyTimer -= 60;
                OnPlayerInfoChanged(InfoType.Energy);
            }
        }
        else
        {
            this.energyTimer = 0;
        }
		if (this.Toughen < 50)
        {
            toughenTimer += Time.deltaTime;
            if (toughenTimer > 60)
            {
                this.Toughen += 1;
                toughenTimer -= 60;
                OnPlayerInfoChanged(InfoType.Toughen);
            }
        }
        else
        {
            this.toughenTimer = 0;
        }

    }//体力和历练自动恢复方法

    public void OnPlayerInfoChange(InfoType infoType)
    {
        if (infoType == InfoType.Name || infoType == InfoType.Energy || infoType == InfoType.Toughen)
        {
            roleController.UpdateRole(PhotonEngine.Instance.role);
        }
    }

    public void UpdateRole()
    {
        roleController.UpdateRole(PhotonEngine.Instance.role);
    }

    public void ChangeName(string newName)
    {
        this.Name = newName;
        PhotonEngine.Instance.role.Name = newName;
        OnPlayerInfoChanged(InfoType.Name);
    }

    void PutonEquip(int id)
    {
        if (id == 0) return;
        Inventory inventory = null;
        InventoryManager._instance.inventoryDict.TryGetValue(id, out inventory);
        this.HP += inventory.HP;
        this.Damage += inventory.Damage;
        this.Power += inventory.Power;
    }

    void PutoffEquip(int id)
    {        
        Inventory inventory = null;
        InventoryManager._instance.inventoryDict.TryGetValue(id, out inventory);
        this.HP -= inventory.HP;
        this.Damage -= inventory.Damage;
        this.Power -= inventory.Power;
    }

    

    #endregion

    public void DressOn(InventoryItem it,bool isSync = true) 
    {
        it.IsDressed = true;
        //首先检测有没有穿上相同的类型的装备
        bool isDressed = false;
        InventoryItem inventoryItemDressed = null;
        switch (it.Inventory.EquipType)
        {
            case EquipType.Bracelet:
                if(braceletInventoryItem != null)
                {
                    isDressed = true;
                    inventoryItemDressed = braceletInventoryItem;                    
                }
                braceletInventoryItem = it;
                break;
            case EquipType.Cloth:
                if (clothInventoryItem != null)
                {
                    isDressed = true;
                    inventoryItemDressed = clothInventoryItem;                    
                }
                clothInventoryItem = it;
                break;
            case EquipType.Helm:
                if (helmInventoryItem != null)
                {
                    isDressed = true;
                    inventoryItemDressed = helmInventoryItem;
                }
                helmInventoryItem = it;
                break;
            case EquipType.Necklace:
                if (necklaceInventoryItem != null)
                {
                    isDressed = true;
                    inventoryItemDressed = necklaceInventoryItem;
                }
                necklaceInventoryItem = it;
                break;
            case EquipType.Ring:
                if (ringInventoryItem != null)
                {
                    isDressed = true;
                    inventoryItemDressed = ringInventoryItem;
                }
                ringInventoryItem = it;
                break;
            case EquipType.Shoes:
                if (shoesInventoryItem != null)
                {
                    isDressed = true;
                    inventoryItemDressed = shoesInventoryItem;
                }
                shoesInventoryItem = it;
                break;
            case EquipType.Weapon:
                if (weaponInventoryItem != null)
                {
                    isDressed = true;
                    inventoryItemDressed = weaponInventoryItem;
                }
                weaponInventoryItem = it;
                break;
            case EquipType.Wing:
                if (wingInventoryItem != null)
                {
                    isDressed = true;
                    inventoryItemDressed = wingInventoryItem;
                }
                wingInventoryItem = it;
                break;
        }
        //有：
        if (isDressed)
        {
            inventoryItemDressed.IsDressed = false;
            InventoryUI._instance.AddInvenetoryItem(inventoryItemDressed);
        }
        if (isSync)
        {
            if (isDressed)
            {
                inventoryItemDbController.UpdateInventoryItemDBList(it.InventoryItemDB,inventoryItemDressed.InventoryItemDB);
            }
            else
            {
                inventoryItemDbController.UpdateInventoryItemDB(it.InventoryItemDB); //更新DB
            }
        }

        OnPlayerInfoChanged(InfoType.Equip);

        //把已经存在的脱掉，放到背包
        //没有：
        //直接穿上装备
    } //穿上装备
    
    public void DressOff(InventoryItem it)
    {
        //it.IsDressed = false;
        switch (it.Inventory.EquipType)
        {
            case EquipType.Bracelet:
                if (braceletInventoryItem != null)
                {
                    braceletInventoryItem = null;
                }               
                break;
            case EquipType.Cloth:
                if (clothInventoryItem != null)
                {
                    clothInventoryItem = null;
                }                
                break;
            case EquipType.Helm:
                if (helmInventoryItem != null)
                {
                     helmInventoryItem = null;
                }               
                break;
            case EquipType.Necklace:
                if (necklaceInventoryItem != null)
                {
                     necklaceInventoryItem = null;
                }               
                break;
            case EquipType.Ring:
                if (ringInventoryItem != null)
                {
                    ringInventoryItem = null;
                }                
                break;
            case EquipType.Shoes:
                if (shoesInventoryItem != null)
                {
                   shoesInventoryItem = null;
                }
                break;
            case EquipType.Weapon:
                if (weaponInventoryItem != null)
                {
                    weaponInventoryItem = null;
                }
                
                break;
            case EquipType.Wing:
                if (wingInventoryItem != null)
                {
                    wingInventoryItem = null;
                }                
                break;
        }

        it.IsDressed = false;  //??

        inventoryItemDbController.UpdateInventoryItemDB(it.InventoryItemDB);;
        InventoryUI._instance.AddInvenetoryItem(it);
        OnPlayerInfoChanged(InfoType.Equip);

    } //脱下装备，放到背包里

    public void InventoryUse(InventoryItem it,int count)
    {
        //使用效果

        //处理物品使用后是否还存在
        it.Count -= count;
        if (it.Count <= 0) 
        {
            InventoryManager._instance.inventoryItemList.Remove(it);
        }
    }

    //取得需要个数的金币数
    public bool GetCoin(int count)
    {
        if (Coin >= count)
        {
            Coin -= count;
            PhotonEngine.Instance.role.Coin = Coin;
            OnPlayerInfoChanged(InfoType.Coin);
            return true;
        }
        return false;    
    }

    public bool GetEnergy(int energy)
    {
        if (Energy > energy)
        {
            Energy -= energy;
            PhotonEngine.Instance.role.Energy = Energy;//是大写E开头的
            OnPlayerInfoChange(InfoType.Energy);
            return true;
        }
            return false;
        
    }

    public void AddCoin(int count)
    {
        this.Coin += count;
        OnPlayerInfoChanged(InfoType.Coin);
    }

    public bool Exchange(int coinChangeCount, int diamondChangeCount)
    {
        if ((Coin + coinChangeCount) >= 0 && (Diamond + diamondChangeCount) >= 0)
        { 
            //可以兑换
            Coin += coinChangeCount;
            Diamond += diamondChangeCount;
            PhotonEngine.Instance.role.Coin = Coin;
            PhotonEngine.Instance.role.Diamond = Diamond;
            OnPlayerInfoChanged(InfoType.All);
            UpdateRole();
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetOverailPower()
    {
        float power = this.Power; 

        if (helmInventoryItem != null)
        {
            power += helmInventoryItem.Inventory.Power * (1 + (helmInventoryItem.Level - 1) / 10f);
        }
        if (clothInventoryItem != null)
        {
            power += clothInventoryItem.Inventory.Power * (1 + (clothInventoryItem.Level - 1) / 10f);
        }
        if (weaponInventoryItem != null)
        {
            power += weaponInventoryItem.Inventory.Power * (1 + (weaponInventoryItem.Level - 1) / 10f);
        }
        if (shoesInventoryItem != null)
        {
            power += shoesInventoryItem.Inventory.Power * (1 + (shoesInventoryItem.Level - 1) / 10f);
        }
        if (necklaceInventoryItem != null)
        {
            power += necklaceInventoryItem.Inventory.Power * (1 + (necklaceInventoryItem.Level - 1) / 10f);
        }
        if (braceletInventoryItem != null)
        {
            power += braceletInventoryItem.Inventory.Power * (1 + (braceletInventoryItem.Level - 1) / 10f);
        }
        if (ringInventoryItem != null)
        {
            power += ringInventoryItem.Inventory.Power * (1 + (ringInventoryItem.Level - 1) / 10f);
        }
        if (wingInventoryItem != null)
        {
            power += wingInventoryItem.Inventory.Power * (1 + (wingInventoryItem.Level - 1) / 10f);
        }
        return (int)power;
    }

    void OnInventoryChange()
    {
        foreach (var inventoryItem in InventoryManager._instance.inventoryItemList)
        {
            if (inventoryItem.Inventory.InventoryType == InventoryType.Equip && inventoryItem.IsDressed == true)
            {
                DressOn(inventoryItem,false);
            }
        }
    }

    void OnDestory()
    {
        if (InventoryManager._instance != null)
        {
            InventoryManager._instance.OnInventoryChange -= this.OnInventoryChange;
        }
    }


}

