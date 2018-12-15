using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    private static GameOverPanel instance;

    public static GameOverPanel Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    private DOTweenAnimation doTween;
    private Text jiangText;

    // Use this for initialization
    void Start ()
    {
        instance = this;
        doTween = this.GetComponent<DOTweenAnimation>();
        jiangText = transform.Find("JiangText").GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Show(string str)
    {
        jiangText.text = str;
        doTween.DOPlayForward();
    }

    public void Hide()
    {
        doTween.DOPlayBackwards();
    }

    public void OnBackButtonClick()
    {
        Hide();
        Destroy(GameController.Instance.gameObject);
        AsyncOperation operation = SceneManager.LoadSceneAsync("Fabao_002_Village");
        LoadSceneProgressBar._instance.Show(operation);
    }


}
