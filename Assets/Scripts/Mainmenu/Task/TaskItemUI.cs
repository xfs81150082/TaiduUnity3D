using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskItemUI : MonoBehaviour {

    private Image typeImage;
    private Image iconImage;
    private Text nameText;
    private Text doesText;
    private Text coinText;
    private Text zhuansheText;
    private Image coinImage;
    private Image zhuansheImage;
    private Button rewardButton;
    private Button combotButton;

    private Task task;

    private void Awake()
    {
        typeImage = transform.Find("TypeImage").GetComponent<Image>();
        iconImage = transform.Find("IconImage").GetComponent<Image>();
        coinImage = transform.Find("CoinImage").GetComponent<Image>();
        zhuansheImage = transform.Find("zhuansheImage").GetComponent<Image>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        doesText = transform.Find("DoesText").GetComponent<Text>();
        coinText = transform.Find("CoinText").GetComponent<Text>();
        zhuansheText = transform.Find("ZhuansheText").GetComponent<Text>();
        rewardButton = transform.Find("RewardButton").GetComponent<Button>();
        combotButton = transform.Find("CombotButton").GetComponent<Button>();

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTask(Task task)
    {
        this.task = task;
        task.OnTaskChange += this.OnTaskChange;
        UpdateShow();

    }

    void UpdateShow()
    {
        switch (task.TaskType)
        {
            case TaskType.Main:
                typeImage.overrideSprite = Resources.Load("pic_主线", typeof(Sprite)) as Sprite;
                break;
            case TaskType.Reward:
                typeImage.overrideSprite = Resources.Load("pic_奖赏", typeof(Sprite)) as Sprite;
                break;
            case TaskType.Daily:
                typeImage.overrideSprite = Resources.Load("pic_日常", typeof(Sprite)) as Sprite;
                break;
        }
        iconImage.overrideSprite = Resources.Load(task.Icon, typeof(Sprite)) as Sprite;
        nameText.text = task.Name;
        doesText.text = task.Does;
        if (task.Cion > 0 && task.Diamond > 0)
        {
            coinImage.overrideSprite= Resources.Load("金币", typeof(Sprite)) as Sprite;
            coinText.text = task.Cion.ToString();
            zhuansheImage.overrideSprite = Resources.Load("钻石", typeof(Sprite)) as Sprite;
            zhuansheText.text = task.Diamond.ToString();
        }
        else if (task.Cion > 0)
        {
            coinImage.overrideSprite = Resources.Load("金币", typeof(Sprite)) as Sprite;
            coinText.text = task.Cion.ToString();
            zhuansheImage.gameObject.SetActive(false);
            zhuansheText.gameObject.SetActive(false);

        }else if (task.Diamond > 0)
        {
            coinImage.gameObject.SetActive(false);
            coinText.gameObject.SetActive(false);
            zhuansheImage.overrideSprite = Resources.Load("钻石", typeof(Sprite)) as Sprite;
            zhuansheText.text = task.Diamond.ToString();
        }

        switch (task.TaskProgress)
        {
            case TaskProgress.NoStart:
                rewardButton.gameObject.SetActive(false);
                combotButton.GetComponentInChildren<Text>().text = "下一步";
                break;
            case TaskProgress.Accept:
                rewardButton.gameObject.SetActive(false);
                combotButton.GetComponentInChildren<Text>().text = "战斗";
                break;
            case TaskProgress.Complete:
                combotButton.gameObject.SetActive(false);
                rewardButton.GetComponentInChildren<Text>().text = "领取奖赏";
                break;
        }



    }

    public void OnCombat()
    {
        TaskUI._instance.Hide();
        TaskManager._inatance.OnExcuteTask(task);

    }

    public void OnReward()
    {

    }

    void OnTaskChange()
    {
        UpdateShow();
    }


}
