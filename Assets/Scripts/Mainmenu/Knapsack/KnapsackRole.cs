using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnapsackRole : MonoBehaviour {

    private Image roleImage;
    private KnapsackRoleEquip helmEquip;
    private KnapsackRoleEquip clothEquip;
    private KnapsackRoleEquip weaponEquip;
    private KnapsackRoleEquip shoesEquip;
    private KnapsackRoleEquip necklaceEquip;
    private KnapsackRoleEquip braceleEquip;
    private KnapsackRoleEquip ringEquip;
    private KnapsackRoleEquip wingEquip;
    private Text leftText;
    private Text damageText;
    private Text expText;
    private Slider expSlider;


    private void Awake()
    {
        roleImage = transform.Find("RoleImage").GetComponent<Image>();
        helmEquip = transform.Find("HelmImage").GetComponent<KnapsackRoleEquip>();
        clothEquip = transform.Find("ClothImage").GetComponent<KnapsackRoleEquip>();
        weaponEquip = transform.Find("WeaponImage").GetComponent<KnapsackRoleEquip>();
        shoesEquip = transform.Find("ChoesImage").GetComponent<KnapsackRoleEquip>();
        necklaceEquip = transform.Find("NecklaceImage").GetComponent<KnapsackRoleEquip>();
        braceleEquip = transform.Find("BraceleImage").GetComponent<KnapsackRoleEquip>();
        ringEquip = transform.Find("RingImage").GetComponent<KnapsackRoleEquip>();
        wingEquip = transform.Find("WingImage").GetComponent<KnapsackRoleEquip>();
        leftText = transform.Find("LeftText").GetComponent<Text>();
        damageText = transform.Find("DangeText").GetComponent<Text>();
        expText = transform.Find("ExpText").GetComponent<Text>();
        expSlider = transform.Find("ExpSlider").GetComponent<Slider>();
        PlayerInfo.Intance.OnPlayerInfoChanged += this.OnPlayerInfoChanged;

    }
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnDestroy()
    {
        PlayerInfo.Intance.OnPlayerInfoChanged -= this.OnPlayerInfoChanged;
    }


    void OnPlayerInfoChanged(InfoType type)
    {
        if (type == InfoType.All || type == InfoType.HP || type == InfoType.Damage||type==InfoType.Exp||type==InfoType.Equip)
        {
            UpdateShow();
        }
    }

    void UpdateShow()
    {
        PlayerInfo info = PlayerInfo.Intance;

        /*
        helmEquip.SetID(info.HelmID);
        clothEquip.SetID(info.ClothID);
        weaponEquip.SetID(info.WeaponID);
        shoesEquip.SetID(info.ShoesID);
        necklaceEquip.SetID(info.NecklaceID);
        braceleEquip.SetID(info.BraceletID);
        ringEquip.SetID(info.RingID);
        wingEquip.SetID(info.WingID);
        */

        helmEquip.SetInventoryItem(info.helmInventoryItem);
        clothEquip.SetInventoryItem(info.clothInventoryItem);
        weaponEquip.SetInventoryItem(info.weaponInventoryItem);
        shoesEquip.SetInventoryItem(info.shoesInventoryItem);
        necklaceEquip.SetInventoryItem(info.necklaceInventoryItem);
        braceleEquip.SetInventoryItem(info.braceletInventoryItem);
        ringEquip.SetInventoryItem(info.ringInventoryItem);
        wingEquip.SetInventoryItem(info.wingInventoryItem);


        leftText.text = info.HP.ToString();
        damageText.text = info.Damage.ToString();
        expText.text = info.Exp + "/" + GameController.GetExpByLevel(info.Level + 1);
        expSlider.value = (float)info.Exp / GameController.GetExpByLevel(info.Level + 1);

    }

}
