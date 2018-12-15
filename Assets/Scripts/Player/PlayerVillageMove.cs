using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerVillageMove : MonoBehaviour {

    public float velocity = 5;
    private Rigidbody rigidbody;
    private NavMeshAgent agent;
    
   

    private void Awake()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        agent = this.GetComponent<NavMeshAgent>();
        
    }

    // Use this for initialization
    void Start () {
       

	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 vel = rigidbody.velocity;
        if(Mathf.Abs(h)>0.5f|| Mathf.Abs(v) > 0.5f)
        {
            
            rigidbody.velocity = new Vector3(h*velocity, vel.y, v*velocity);
            transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));
        }
        else
        {
            if (agent.enabled == false)
            {
                rigidbody.velocity = Vector3.zero;
            }
        }
        if (agent.enabled)
        {
            //transform.rotation = Quaternion.LookRotation(agent.velocity);
        }
		
	}


}
