using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    private Transform playerPos;
    public Vector3 offset = new Vector3(0, 8, -7);

	// Use this for initialization
	void Start () {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = playerPos.position + offset;
		
	}
}
