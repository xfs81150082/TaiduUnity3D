using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TranscriptMapDilog : MonoBehaviour {

    private Text nameText;
    private Text doesText;
    private Text energyText;
    private Text energytagText;
    //private Button enterBtn;
    private DOTweenAnimation tweenAnim;

    private void Awake()
    {
        nameText =transform.Find("NameText").GetComponent<Text> ();
        doesText = transform.Find("DoesText").GetComponent<Text>();
        energyText = transform.Find("TiLiText").GetComponent<Text>();
        energytagText = transform.Find("TilitagText").GetComponent<Text>();
        //enterBtn = transform.Find("Button").GetComponent<Button>();
        tweenAnim = this.GetComponent<DOTweenAnimation>();

    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowWarn()
    {
        this.gameObject.SetActive(true);
        energytagText.gameObject.SetActive(false);
        energytagText.enabled = false;
        energyText.gameObject.SetActive(false);
        //enterBtn.enabled = false;
        doesText.text = "当前等级无法进入该地下城";
        

    }

    public void ShowDialog(BtnTranscript transcript)
    {
        this.gameObject.SetActive(true);
        energytagText.gameObject.SetActive(true);
        energyText.gameObject.SetActive(true);
        //enterBtn.enabled = true;
        doesText.text = transcript.does;
        energyText.text = transcript.needEnergy.ToString();

    }

       public void OnEnterClick()
    {
        transform.parent.SendMessage("OnEnterClick");
    }
    

    public void OnCancelClose()
    {
        tweenAnim.DOPlayBackwards();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

   
 

}
