using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBuliet : MonoBehaviour {

    public float lifeTime = 10;

    public float moveSpeed = 2;
    public float Damage
    {
        set;
        private get;
    }
    private float repeatRate = 1f;
    private float force = 1000;

    public List<GameObject> playerList = new List<GameObject>();



    // Use this for initialization
    void Start () {
        Destroy(this.gameObject, lifeTime);
        InvokeRepeating("Attack",0, repeatRate);

    }
	
	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

	}
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            if (playerList.IndexOf(col.gameObject) < 0)
            {
                playerList.Add(col.gameObject);
            }

        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            if (playerList.IndexOf(col.gameObject) >= 0)
            {
                playerList.Remove(col.gameObject);
            }

        }

    }
    void Attack()
    {
        foreach (GameObject player in playerList)
        {
            player.SendMessage("TakeDamage", Damage * repeatRate, SendMessageOptions.DontRequireReceiver);
            player.GetComponent<Rigidbody>().AddForce(transform.forward * force);
        }
    }
}
