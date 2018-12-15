using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneProgressBar : MonoBehaviour {
    public static LoadSceneProgressBar _instance;
    private DOTweenAnimation doTween;
    private Slider progressBar;
    private bool isAsyn = false;
    private AsyncOperation ao = null;


    private void Awake()
    {
        _instance = this;
        doTween = this.GetComponent<DOTweenAnimation>();
        progressBar = transform.Find("LoadBarSlider").GetComponent<Slider>();
    }
    
    private void Update()
    {
        if (isAsyn)
        {
            print("到LoadSecne...26行");
            progressBar.value = ao.progress;
            if (progressBar.value >= 1)
            {
                doTween.DOPlayBackwards();
                isAsyn = false;
                print("到LoadSecne...31行");
            }
        }
    }

    public void Show(AsyncOperation ao)
    {
        doTween.DOPlayForward();
        isAsyn = true;
        this.ao = ao;
    }

}
