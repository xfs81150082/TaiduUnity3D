using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBar : MonoBehaviour {
    private Button button01;
    private Button button02;
    private Button button03;
    private Button button04;
    private Button button05;
    private Button button06;

    private PlayerAutoMove playerAutoMove;
    private PlayerAutoMove PlayerAutoMove
    {
        get
        {
            if (playerAutoMove == null)
            {
                playerAutoMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAutoMove>();
            }
            return playerAutoMove;
        }

    }

    // Use this for initialization
    void Start () {
        button01 = transform.Find("Button01").GetComponent<Button>();
        button02 = transform.Find("Button02").GetComponent<Button>();
        button03 = transform.Find("Button03").GetComponent<Button>();
        button04 = transform.Find("Button04").GetComponent<Button>();
        button05 = transform.Find("Button05").GetComponent<Button>();
        button06 = transform.Find("Button06").GetComponent<Button>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void OnCombat()
    {
        PlayerAutoMove.SetDestin(NPCManager._instance.transcriptGO.GetComponent<Transform>().position);


    }


}
