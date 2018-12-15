using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PowerShow : MonoBehaviour {
    public static PowerShow _instance;

    private float starValue = 0;
    private int endValue = 1000;
    private bool isStar = false;
    private Text text;
    private bool isUp = true;
    private int speed = 500;
    private DOTweenAnimation tween;

    private void Awake()
    {
        _instance = this;      
        text = transform.Find("Text").GetComponent<Text>();
        tween = this.GetComponent<DOTweenAnimation>();
        //Event  ShowPowerChange
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isStar)
        {
            ShowTween();
            StartCoroutine(Show(2));            
        }   
    }

    IEnumerator Show(float time)
    {
        //tween.DOPlayForward();
        //gameObject.SetActive(true);  
        yield return new WaitForSeconds(time);
        //tween.DOPlayBackwards();
        gameObject.SetActive(false);
        isStar = false;
    }


    private void ShowTween()
    {        
        if (isUp)
        {
            starValue += speed * Time.deltaTime;
            if (starValue > endValue)
            {
                isStar = false;
                starValue = endValue;
            }
        }
        else
        {
            starValue -= speed * Time.deltaTime;
            if (starValue < endValue)
            {
                isStar = false;
                starValue = endValue;
            }
        }
        text.text = (int)starValue + "";   
    }

    public void OnTweenFinished()
    {
        tween.DOPlayBackwards();
         
    }

    public void ShowPowerChange(float starValue,int endValue)
    {
        gameObject.SetActive(true);
        this.starValue = starValue;
        this.endValue = endValue;
        isStar = true;
        if (endValue > starValue)
        {
            isUp = true;
        }
        else
        {
            isUp = false;           
        }
    }


}
