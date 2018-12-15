using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour {


    private Text nameText;
    private Text doesText;
    private Text upgradeButtonText;
    private Button upgradeButton;
    private Skill skill;

    private void Awake()
    {
        nameText = transform.Find("NameText").GetComponent<Text>();
        doesText = transform.Find("DoesText").GetComponent<Text>();
        upgradeButton = transform.Find("GradeButton").GetComponent<Button>();
        upgradeButtonText = transform.Find("GradeButton/Text").GetComponent<Text>();
        nameText.text = "";
        doesText.text = "";
        DisableUpgradeButton("选择技能");
    }

    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //此处语法不对 
    void DisableUpgradeButton(string text="")
    {
        if (upgradeButton.enabled == true)
        {
            upgradeButton.enabled = false;
        }        
        if (text != "")
        {
            upgradeButtonText.text = text;
        }      
    }
    //此处语法不对 
    void EnableUpgradeButton(string text = "")
    {
        if (upgradeButton.enabled == false)
        {
            upgradeButton.enabled = true;
        }
        if (text != "")
        {
            upgradeButtonText.text = text;
        }


    }
    void OnSkillClick(Skill skill)
    {
        this.skill = skill;
        PlayerInfo info = PlayerInfo.Intance;
        if(((skill.Level + 1) * 500) <= info.Coin)
        {
            if (skill.Level < info.Level)
            {
                EnableUpgradeButton("升级");
            }
            else
            {
                EnableUpgradeButton("角色等级不足");
            }
            
        }
        else
        {
            DisableUpgradeButton("金币不足");
        }
        
           
        nameText.text = skill.Name + "LV." + skill.Level;
        doesText.text = "当前技能的攻击力为：" + (skill.Damage *skill.Level)+ "; 下一级技能的攻击力为：" + (skill.Damage *( skill.Level+1)) + "; 下级需的金币数" + ((skill.Level + 1) * 500);
    }

    public void OnUpgrade()
    {
        PlayerInfo info = PlayerInfo.Intance;
        if (skill.Level < info.Level)
        {
            int coinNeed = ((skill.Level + 1) * 500);
            bool isSuccess = info.GetCoin(coinNeed);
            if (isSuccess)
            {
                skill.Upgrade();
                //同步到数据库 skillDB role
                SkillManager._instance.Upgrade(skill);
                OnSkillClick(skill);
            }
            else
            {
                DisableUpgradeButton("金币不足");
            }
        }
        else
        {
            EnableUpgradeButton("角色等级不足");
        }        
    }

}
