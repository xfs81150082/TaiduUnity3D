using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShopUI : MonoBehaviour
{

    public static ShopUI Instance;
    private DOTweenAnimation doTween;

    void Awake()
    {
        Instance = this;
        doTween = this.GetComponent<DOTweenAnimation>();
    }

    public void Show()
    {
        doTween.DOPlayForward();

    }

    public void Hide()
    {
        doTween.DOPlayBackwards();
    }

    public void OnDiamondClick()
    {
        
    }

    public void OnCoinClick()
    {
        
    }

    public void OnBuy(int coinChangeCount, int diamondChangeCount)
    {
        bool isSuccess = PlayerInfo.Intance.Exchange(coinChangeCount, diamondChangeCount);
        if (isSuccess)
        {
           Debug.Log("兑换成功");
        }
        else
        {
            if (coinChangeCount < 0)
            {
                MessageManger._instance.ShowMessage("金币不足");
            }
            else
            {
                MessageManger._instance.ShowMessage("钻石不足");
            }
        }


    }



}
