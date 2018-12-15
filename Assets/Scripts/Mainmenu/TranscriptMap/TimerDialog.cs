using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TimerDialog : MonoBehaviour
{
    public static TimerDialog instance;
    private DOTweenAnimation doTween;

    private Text timeText;
    private Text dialogText;

    public float time = 20f;
    private float timer = 0;
    private bool isStart = false;

    void Awake()
    {
        instance = this;
	    doTween = this.GetComponent<DOTweenAnimation>();
	    timeText = transform.Find("TimeText").GetComponent<Text>();
	    dialogText = transform.Find("DialogText").GetComponent<Text>();  
    }

    // Use this for initialization
    void Start ()
	{
	    doTween.DOPlayForward();
	}
	
	// Update is called once per frame
	void Update () {
	    if (isStart)
	    {
	        timer += Time.deltaTime;
	        int remainTime = (int) (time - timer);
	        timeText.text = remainTime.ToString();
	        if (timer > time)
	        {
	            timer = 0;
	            isStart = false;
                OnTimeEnd();
                
	        }
	    }
	}

    public void StartTimer()
    {
        doTween.DOPlayBackwards();
        
        timer = 0;
        isStart = true;
    }

    public void HideTimer()
    {
        doTween.DOPlayForward();
        
        isStart = false;
    }

    public void OnCancelButtonClick()
    {
        HideTimer();
        transform.parent.SendMessage("ShowTranscriptMapDialog");
    }

    public void OnTimeEnd()
    {
        HideTimer();
        transform.parent.SendMessage("ShowTranscriptMapDialog");
    }


}
