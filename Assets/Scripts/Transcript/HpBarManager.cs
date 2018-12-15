using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBarManager : MonoBehaviour {
    public static HpBarManager _instance;

    public GameObject hpbarPrefab;
    public GameObject hudtextPrefab;

    //private Transform parentGo;


    private void Awake()
    {
        _instance = this;
        //parentGo = this.GetComponent<Transform>();
    }
 
    
    public GameObject GetHpBar(GameObject target)
    {        
        GameObject go = Instantiate(hpbarPrefab,transform.position, Quaternion.identity);
        go.transform.parent = this.transform;
        go.GetComponent<FollowGoTarget>().uifollowgoTarget = target.transform;        
        return go;
    }
    public GameObject GetHudText(GameObject target)
    {
        GameObject go = Instantiate(hudtextPrefab, transform.position, Quaternion.identity);
        go.transform.parent = this.transform;
        go.GetComponent<bl_HUDText>().CanvasParent = this.transform;
        go.GetComponent<FollowGoTarget>().uifollowgoTarget = target.transform;
        return go;       
    }


}
