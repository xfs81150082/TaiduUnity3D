using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerVillageAnimation : MonoBehaviour {
    private Animator anim;
    private Rigidbody rigidbody;
    private NavMeshAgent agent;


    // Use this for initialization
    void Start () {
        anim = this.GetComponent<Animator>();
        rigidbody = this.GetComponent<Rigidbody>();
        agent = this.GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        if (rigidbody.velocity.magnitude > 0.5f || agent.velocity.magnitude > 0.5f)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }


	}
}
