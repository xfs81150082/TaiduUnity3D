using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsSetActiveSelf : MonoBehaviour {
    private static IsSetActiveSelf _instance;
    public bool IsSetActive = false;
    public float timer = 0;
    public float coldTimer = 3f;

    private void Awake()
    {
        _instance = this;
        

    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {        
/*
        if (IsSetActive)
        {
            timer += Time.deltaTime;
            if (timer >= coldTimer)
            {
                this.gameObject.SetActive(false);
                timer = coldTimer;
            }
        }
*/
	}

    
}
