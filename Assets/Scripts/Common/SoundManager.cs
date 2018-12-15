using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager _instance;

    public AudioClip[] audioClipArray;
    private Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();
    public AudioSource audioSource;
    public bool isQuiet = false;

    private void Awake()
    {
        _instance = this;

    }

    // Use this for initialization
    void Start () {
		foreach (AudioClip ac in audioClipArray)
        {
            audioDict.Add(ac.name, ac);
        }


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Play(string audioName)
    {
        if (isQuiet) return;
        AudioClip ac;
        if(audioDict.TryGetValue(audioName,out ac))
        {
            //AudioSource.PlayClipAtPoint(ac, Vector3.zero);
            audioSource.PlayOneShot(ac);
        }
    }

    public void Play(string audioName,AudioSource audioSource)
    {
        if (isQuiet) return;
        AudioClip ac;
        if (audioDict.TryGetValue(audioName, out ac))
        {
            //AudioSource.PlayClipAtPoint(ac, Vector3.zero);
            audioSource.PlayOneShot(ac);
        }
    }


}
