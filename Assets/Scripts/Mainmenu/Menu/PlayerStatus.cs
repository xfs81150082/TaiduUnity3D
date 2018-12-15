using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerStatus : MonoBehaviour {
    public static PlayerStatus _instance;

#region  private赋值
    private Image headImage;
    private Text nameText;
    private Text levelText;
    private Text vipText;
    private Text attackText;
    private Text expText;
    private Slider expSlider;
    private Button changenameButton;
    private Button canelButton;

    private Text zhuansheCountText;
    private Text jingbiCountText;
    private Text jingsheCountText;
    private Text yuntieCountText;

    private Text tiliText;
    private Text tiliTime1Text;
    private Text tiliTime2Text;
    private Text LilianText;
    private Text LilianTime1Text;
    private Text LilianTime2Text;

    private DOTweenAnimation playerstatusDOTweenAnimation;
    
#endregion

#region awake,start,update...
    private void Awake()
    {
        _instance = this;
        headImage = transform.Find("HeadImage").GetComponent<Image>();
        nameText = transform.Find("HeadImage/NameText").GetComponent<Text>();
        levelText = transform.Find("HeadImage/LevelText").GetComponent<Text>();
        vipText = transform.Find("HeadImage/VIPText").GetComponent<Text>();
        attackText = transform.Find("HeadImage/AttackText").GetComponent<Text>();
        expText = transform.Find("HeadImage/ExpText").GetComponent<Text>();
        expSlider = transform.Find("HeadImage/ExpSlider").GetComponent<Slider>();
        changenameButton = transform.Find("HeadImage/ChangeNameButton").GetComponent<Button>();
        canelButton = transform.Find("HeadImage/CanelButton").GetComponent<Button>();

        zhuansheCountText = transform.Find("ZhuanSheCountText").GetComponent<Text>();
        jingbiCountText = transform.Find("JingBiCountText").GetComponent<Text>();
        jingsheCountText = transform.Find("JingSheCountText").GetComponent<Text>();
        yuntieCountText = transform.Find("YunTieCountText").GetComponent<Text>();

        tiliText = transform.Find("TiliText").GetComponent<Text>();
        tiliTime1Text = transform.Find("TiliTime1Text").GetComponent<Text>();
        tiliTime2Text = transform.Find("TiliTime2Text").GetComponent<Text>();
        LilianText = transform.Find("LilianText").GetComponent<Text>();
        LilianTime1Text = transform.Find("LilianTime1Text").GetComponent<Text>();
        LilianTime2Text = transform.Find("LilianTime2Text").GetComponent<Text>();

        PlayerInfo.Intance.OnPlayerInfoChanged += this.OnPlayerInfoChanged;

    }

    // Use this for initialization
    void Start () {
       
        playerstatusDOTweenAnimation = this.GetComponent<DOTweenAnimation>();

    }
	
	// Update is called once per frame
	void Update () {
        UpdateEnergyAndTougherShow();

	}
    #endregion

#region  自定义方法
    void OnDestroy()
    {
        PlayerInfo.Intance.OnPlayerInfoChanged -= this.OnPlayerInfoChanged;
    }
    
    void OnPlayerInfoChanged(InfoType type)
    {
        if (type == InfoType.HeadPortrait || type == InfoType.Name || type == InfoType.Level || type == InfoType.Energy || type == InfoType.Toughen || type == InfoType.All)
        {
            UpdateShow();
        }
    }//当我们的主角信息发生改变的时候，会触发这个方法

    public void UpdateShow()
    {
        PlayerInfo info = PlayerInfo.Intance;  
        headImage.overrideSprite = Resources.Load(info.HeadPortrait, typeof(Sprite)) as Sprite;
        nameText.text = info.Name.ToString();
        levelText.text = info.Level.ToString();
        vipText.text = info.Level.ToString();
        attackText.text = info.Power.ToString();
        int gradeExp = GameController.GetExpByLevel(info.Level + 1);
        expText.text = info.Exp + "/" + gradeExp;
        expSlider.value = info.Exp / (100 + info.Level*10);
        zhuansheCountText.text = info.Diamond.ToString();
        jingbiCountText.text = info.Coin.ToString();
        jingsheCountText.text = info.Diamond.ToString();
        yuntieCountText.text = info.Diamond.ToString();
        tiliText.text = info.Energy + "/100";
        LilianText.text = info.Toughen + "/50";
        UpdateEnergyAndTougherShow();
    }//更新显示

    void UpdateEnergyAndTougherShow()
    {
        PlayerInfo info = PlayerInfo.Intance;
        if (info.Energy>=100){
            tiliTime1Text.text = "00:00:00";
            tiliTime2Text.text = "00:00:00";
        }
        else
        {
            int remainTime = 60 - (int)info.energyTimer;
            string str = remainTime <= 9 ? "0" + remainTime:remainTime.ToString();
            tiliTime1Text.text = "00:00:" + str;
            //首先总的体力为100其中一个体力是在最后的00表示；所以99表示余下的体力。
            int minutes = (99 - info.Energy);
            int hours = minutes / 60;
            minutes = minutes % 60;
            string hoursStr = hours <= 9 ? "0" + hours : hours.ToString();
            string minutesStr = minutes <= 9 ? "0" + minutes : minutes.ToString();
            tiliTime2Text.text = hoursStr + ":" + minutesStr+":00";
        }
        if (info.Toughen >= 50)
        {
            LilianTime1Text.text = "00:00:00";
            LilianTime2Text.text = "00:00:00";
        }
        else
        {
            int reTime = 60 - (int)info.toughenTimer;
            string str = reTime <= 9 ? "0" + reTime : reTime.ToString();
            LilianTime1Text.text = "00:00:" + str;
            int minutes = (49 - info.Toughen);
            int hours = minutes / 60;
            minutes = minutes % 60;
            string hoursStr = hours <= 9 ? "0" + hours : hours.ToString();
            string minutesStr = minutes <= 9 ? "0" + minutes : minutes.ToString();
            LilianTime2Text.text = hoursStr + ":" + minutesStr+":00";

        }

    }

    public void OnShowClick()
    {
        playerstatusDOTweenAnimation.DOPlayForward();
        playerstatusDOTweenAnimation.DOPlayBackwards();
    }
    
    #endregion

}
