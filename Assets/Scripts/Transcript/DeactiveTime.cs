using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveTime : MonoBehaviour {
    public float deactiveTime = 1;
    private float timer = 0;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > deactiveTime)
        {
            this.gameObject.SetActive(false);
            timer = 0;
        }
	}
}
