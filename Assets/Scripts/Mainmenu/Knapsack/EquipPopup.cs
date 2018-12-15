using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EquipPopup : MonoBehaviour {
    public PowerShow powerShow;

    private DOTweenAnimation doAween;
    private InventoryItem it;
    private InventoryItemUI itUI;
    private KnapsackRoleEquip roleEquip;

    private bool isLeft = true;

    private Image zhuangbeiImage;
    private Text nameText;
    private Text zheliangText;
    private Text dangeText;
    private Text leftText;
    private Text levelText;
    private Text powerText;
    private Text doesText;
    private Text equipdownText;

    private Button equipButton;

    private void Awake()
    {
        //powerShow = GameObject.Find("PowerShowImage").GetComponent<PowerShow>();
        doAween = this.GetComponent<DOTweenAnimation>();
        zhuangbeiImage = transform.Find("ZhuangbeiImage").GetComponent<Image>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        zheliangText = transform.Find("ZheliangText").GetComponent<Text>();
        dangeText = transform.Find("DangeText").GetComponent<Text>();
        leftText = transform.Find("LeftText").GetComponent<Text>();
        levelText = transform.Find("LevelText").GetComponent<Text>();
        powerText = transform.Find("PowerText").GetComponent<Text>();
        doesText = transform.Find("DoesText").GetComponent<Text>();

        equipdownText=transform.Find("EquipUpImage/Text").GetComponent<Text>();

        equipButton = transform.Find("EquipUpImage").GetComponent<Button>();
        
        //equipButton.onClick.AddListener("OnEquipClick()");
    }

    // Use this for initialization
    void Start () {
        gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Show(InventoryItem it ,InventoryItemUI itUI,KnapsackRoleEquip roleEquip,bool isLeft = true)    
    {
        OnForwardClick(); 
        
        this.it = it;
        this.itUI = itUI;
        this.roleEquip = roleEquip;
        Vector3 pos = transform.localPosition;
        this.isLeft = isLeft;
        if (isLeft)
        {            
            transform.localPosition = new Vector3(-Mathf.Abs(pos.x), pos.y, pos.z);
            equipdownText.text = "穿上装备";
        }
        else
        {
            transform.localPosition = new Vector3(Mathf.Abs(pos.x), pos.y, pos.z);
            equipdownText.text = "御下装备";
        }
        
        zhuangbeiImage.overrideSprite = Resources.Load(it.Inventory.Icon, typeof(Sprite)) as Sprite;
        nameText.text = it.Inventory.Name;
        zheliangText.text = it.Inventory.Quality.ToString();
        dangeText.text = it.Inventory.Damage.ToString();
        leftText.text = it.Inventory.HP.ToString();//
        levelText.text = it.Level.ToString();
        powerText.text = it.Inventory.Power.ToString();//
        doesText.text = it.Inventory.Des;//        
    }
    
    void OnForwardClick()
    {
        gameObject.SetActive(true);        
    }
    public void OnClose()
    {
        Close();
        GameObject.Find("Knapsack").SendMessage("DisableButton");
        //transform.parent.SendMessage("");
    }
    public void Close()
    {
        ClearObject();
        gameObject.SetActive(false);
    }

    //点击御下和装备按钮触发
    public void OnEquip()
    {
        int starValue = PlayerInfo.Intance.GetOverailPower();
        if (isLeft)  
        {
            itUI.Clear();                        //清空该装备所在的格子
            PlayerInfo.Intance.DressOn(it);     //从背包装备到身上          
        }
        else
        {
            roleEquip.Clear();                  //把身上的装备清空
            PlayerInfo.Intance.DressOff(it);   //从身上脱下          
        }        
        int endValue = PlayerInfo.Intance.GetOverailPower();
        powerShow.ShowPowerChange(starValue, endValue);
        InventoryUI._instance.SendMessage("UpdateCount");
        OnClose();        
    } 



    //点击了升级按钮
    public void OnUpGrade()
    {
        int coinNeed = (it.Level + 1) * it.Inventory.Price;
        bool isSuccess =PlayerInfo.Intance.GetCoin(coinNeed);
        if (isSuccess)
        {
            it.Level += 1;
            levelText.text = it.Level.ToString();     
            InventoryManager._instance.UpgradeEquip(it);
        }
        else
        {
            //给出提示信息
            MessageManger._instance.ShowMessage("金币不足，无法升级");
        }

    }



    void ClearObject()
    {
        it = null;
        itUI = null;
    }

}
