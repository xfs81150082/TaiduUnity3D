using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TopBar : MonoBehaviour {
    private Text jingbiText;
    private Text baosheText;
    private Button jingbiButton;
    private Button baosheButton;

    private void Awake()
    {
        jingbiText = transform.Find("JingbiText").GetComponent<Text>();
        baosheText = transform.Find("BaosheText").GetComponent<Text>();
        jingbiButton = transform.Find("JingbiButton").GetComponent<Button>();
        baosheButton = transform.Find("BaosheButton").GetComponent<Button>();

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
        if (type == InfoType.Coin || type == InfoType.Diamond || type == InfoType.All)
        {
            UpdateShow();
        }
    }//当我们的主角信息发生改变的时候，会触发这个方法

    void UpdateShow()
    {
        PlayerInfo info = PlayerInfo.Intance;
        jingbiText.text = info.Coin.ToString();
        baosheText.text = info.Diamond.ToString();
        

    }//更新显示


}
