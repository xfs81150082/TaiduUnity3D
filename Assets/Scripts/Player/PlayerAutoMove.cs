using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class PlayerAutoMove : MonoBehaviour {

    private NavMeshAgent agent;
    public float minDistance = 3f;

    private void Awake()
    {
        agent = this.GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start () {
        

	}
	
	// Update is called once per frame
	void Update () {
        if (agent.enabled)
        {
            if (agent.remainingDistance < minDistance && agent.remainingDistance != 0) 
            {
                agent.Stop();
                agent.enabled = false;
                TaskManager._inatance.OnArriveDestion();
            }
        
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");        
        if (Mathf.Abs(h) > 0.5f || Mathf.Abs(v) > 0.5f)            
        {
            StopAuto();     //如果在寻路过程按上移动键，停止寻路
            
        }
     
	}
    public void SetDestin(Vector3 targetPos)
    {
        agent.enabled = true;
        agent.SetDestination(targetPos); 
    }

    public void StopAuto()
    {
        if (agent.enabled)
        {
            agent.Stop();
            agent.enabled = false;
        }

    }

}
