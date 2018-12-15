using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerBar : MonoBehaviour {
    private Image headImage;
    private Text nameText;
    private Text levelText;
    private Text energyText;
    private Text toughenText;
    private Slider energySlider;
    private Slider toughenSlider;
    private Button energyPlusButton;
    private Button toughenPlusButton;
    private Button stauasButton;   
 

#region
    private void Awake()
    {
        headImage = transform.Find("HeadImage").GetComponent<Image>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        levelText = transform.Find("LevelText").GetComponent<Text>();
        energyText = transform.Find("TiliCountText").GetComponent<Text>();
        toughenText = transform.Find("LilianCountText").GetComponent<Text>();
        energySlider = transform.Find("TiliSlider").GetComponent<Slider>();
        toughenSlider = transform.Find("LiLianSlider").GetComponent<Slider>();
        energyPlusButton = transform.Find("TiliButton").GetComponent<Button>();
        toughenPlusButton = transform.Find("LilianButton").GetComponent<Button>();
        stauasButton = transform.Find("HeadImage").GetComponent<Button>();

        PlayerInfo.Intance.OnPlayerInfoChanged += this.OnPlayerInfoChanged;      
      
    }    

    // Use this for initialization
    void Start () {
        
         
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    #endregion  awake,start,update...

    void OnDestroy()
    {
        PlayerInfo.Intance.OnPlayerInfoChanged -= this.OnPlayerInfoChanged;
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
        energyText.text = info.Energy+"/100";
        energySlider.value = info.Energy/100f;
        toughenText.text = info.Toughen+"/50";
        toughenSlider.value = info.Toughen/50f;

    }//更新显示

    public void OnHeadCilck()
    {
        
    }
}
