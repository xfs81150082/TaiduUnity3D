using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnTranscript : MonoBehaviour {

    public int id;
    public int needLevel;
    public int needEnergy = 3;
    public string sceneName;
    public string does = "这里是一个阴森恐怖的地方，你怕吗？";

    


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        transform.parent.SendMessage("OnBtnTranscriptClick",this);

    }


}
