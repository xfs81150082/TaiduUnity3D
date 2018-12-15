using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SystemUI : MonoBehaviour
{
    private static SystemUI _instance;
    public static SystemUI Instance
    {
        get
        {
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    private DOTweenAnimation doTween;
    private bool isAudioOpen = true;
    private Image audioImage;
    private Text audioText;
    private AudioSource audioSource;

    void Awake()
    {
        _instance = this;
        doTween = this.GetComponent<DOTweenAnimation>();
        audioImage = transform.Find("AudioImage").GetComponent<Image>();
        audioText = transform.Find("AudioText").GetComponent<Text>();
        audioSource = this.GetComponent<AudioSource>();
    }

    public void Show()
    {
        
    }

    public void Hide()
    {
        
    }

    public void OnAudioImageClich()
    {
        if (isAudioOpen)
        {
            isAudioOpen = false;
            audioImage.overrideSprite = Resources.Load("pic_音效关闭", typeof(Sprite)) as Sprite;
            audioText.text = "音效关闭";
            audioSource.enabled = false;
        }
        else
        {
            isAudioOpen = true;
            audioImage.overrideSprite = Resources.Load("pic_音效开启", typeof(Sprite)) as Sprite;
            audioText.text = "音效开启";
            audioSource.enabled = true;
        }
    }

    public void OnContactButtonClick()
    {
        Application.OpenURL("https://user.qzone.qq.com/81150082/infocenter");
    }

    public void OnChangeRoleButtonClick()
    {
        Destroy(PhotonEngine.Instance.gameObject);
        //AsyncOperation operation = Application.LoadLevelAsync(0);
        AsyncOperation operation = SceneManager.LoadSceneAsync(0);
        LoadSceneProgressBar._instance.Show(operation);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

}
