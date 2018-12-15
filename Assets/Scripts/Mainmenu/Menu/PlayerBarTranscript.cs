using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerBarTranscript: MonoBehaviour {
    private Image headImage;
    private Text nameText;
    private Text levelText;
    private Text hpText;
    private Text energyText;
    private Slider hpSlider;
    private Slider energySlider;
    private Button hpPlusButton;
    private Button energyPlusButton;
    private Button stauasButton;   
 

#region
    private void Awake()
    {
        headImage = transform.Find("HeadImage").GetComponent<Image>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        levelText = transform.Find("LevelText").GetComponent<Text>();
        energyText = transform.Find("EnergyCountText").GetComponent<Text>();
        hpText = transform.Find("HpCountText").GetComponent<Text>();
        energySlider = transform.Find("EnergySlider").GetComponent<Slider>();
        hpSlider = transform.Find("HpSlider").GetComponent<Slider>();
        energyPlusButton = transform.Find("EnergyButton").GetComponent<Button>();
        hpPlusButton = transform.Find("HpButton").GetComponent<Button>();
        stauasButton = transform.Find("HeadImage").GetComponent<Button>();

        PlayerInfo.Intance.OnPlayerInfoChanged += this.OnPlayerInfoChanged;      
      
    }    

    // Use this for initialization
    void Start () {
        UpdateShow();
        TranscriptManager.Instance.player.GetComponent<PlayerAttack>().OnPlayerHpChange += this.OnPlayerHpChange;  //空指针？

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    #endregion  awake,start,update...

    void OnDestroy()
    {
        PlayerInfo.Intance.OnPlayerInfoChanged -= this.OnPlayerInfoChanged;
        if (TranscriptManager.Instance.player != null)
        {
            TranscriptManager.Instance.player.GetComponent<PlayerAttack>().OnPlayerHpChange -= this.OnPlayerHpChange;
        }
    }


    void OnPlayerInfoChanged(InfoType type)
    {
        if (type==InfoType.HeadPortrait||type==InfoType.Name||type==InfoType.Level||type==InfoType.Energy||type==InfoType.Toughen||type==InfoType.All)
        {
            UpdateShow();
        }
        
    }//当我们的主角信息发生改变的时候，会触发这个方法

    void UpdateShow()
    {
        PlayerInfo info = PlayerInfo.Intance;        
        headImage.overrideSprite = Resources.Load(info.HeadPortrait, typeof(Sprite)) as Sprite;        
        nameText.text = info.Name.ToString();
        levelText.text = info.Level.ToString();       
        hpText.text = info.HP+"/"+ info.HP;
        hpSlider.value = info.HP/info.HP;
        energyText.text = info.Energy+"/100";
        energySlider.value = info.Energy/100f;

    }//更新显示

    public void OnHeadCilck()
    {
        
    }

    void OnPlayerHpChange(int hp)
    {

        PlayerInfo info = PlayerInfo.Intance;
        hpSlider.value = (float)hp / info.HP;
        hpText.text = hp + "/" + info.HP;
    }



}
