using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MessageManger : MonoBehaviour {
    public static MessageManger _instance;

    private Text messageText;
    private DOTweenAnimation messageDotween;
    private bool isSetActive = true;

    private void Awake()
    {
        _instance = this;

        messageText = transform.Find("MessageText").GetComponent<Text>();
        messageDotween = this.GetComponent<DOTweenAnimation>();

        gameObject.SetActive(false);

    }

    

    public void ShowMessage(string message, float time=1)
    {
        gameObject.SetActive(true); 
        StartCoroutine(Show(message, time));
    }

    IEnumerator Show(string message, float time)
    {                
        isSetActive = true;
        messageDotween.DOPlayForward();
        messageText.text = message;
        yield return new WaitForSeconds(time);
        messageDotween.DOPlayBackwards();        
        isSetActive = false;
    }

    public void OnTweenFinished()
    {
        if(isSetActive = false)
        {
            gameObject.SetActive(false);
        }
    }


}
