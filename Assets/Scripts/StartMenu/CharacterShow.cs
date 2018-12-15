using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShow : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMouseUpAsButton()
    {        
        StartMenuController._instance.OnCharacterClick(transform.gameObject);        
    }

}
