using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerEffect : MonoBehaviour {
    public static PlayerEffect _instance;
   
    private ParticleSystem[] basicParticleArray;    
    public float timer = 0;
    public float coldTimer = 2;
    public bool isSetAct = false;
    

    private void Awake()
    {
        _instance = this;        
        basicParticleArray = this.GetComponentsInChildren<ParticleSystem>();     
        
    }

    // Use this for initialization
    void Start() {       
        foreach (ParticleSystem Particle in basicParticleArray)
        {
            Particle.gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update() {
        ShowArray();

    }

    public void ShowTrue()
    {
        isSetAct = true;
    }
   
    public void ShowArray()
    {
        if (isSetAct)
        {
            timer += Time.deltaTime;
            foreach (ParticleSystem basicParticle in basicParticleArray)
            {
                basicParticle.gameObject.SetActive(true);                
            }
            if (timer >= coldTimer)
            {
                foreach (ParticleSystem basicParticle in basicParticleArray)
                {
                    basicParticle.gameObject.SetActive(false);
                    timer = 0;
                    isSetAct = false;
                }
            }
        }
        
        

    }


}
