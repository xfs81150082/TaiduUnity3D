using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGoTarget : MonoBehaviour {
    public static FollowGoTarget _instance;

    public Transform uifollowgoTarget;
    public Vector3 uifollowgoOffset = new Vector3(0f, 1f, 0f);

    public Transform gofollowgoTarget;
    public Vector3 gofollowgoOffset = new Vector3(0f, 1f, 0f);

    public Transform camerafollowgoTarget;
    public Vector3 camerafollowgoOffset = new Vector3(0f, 8.2f, -11.2f);
    public float smoothing = 3;

    private void Awake()
    {
        _instance = this;

    }


    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (FindNearestTargetGo() != null)
        //{
        //    camerafollowgoTarget = FindNearestTargetGo().transform;
        //}     
        if (TranscriptManager.Instance != null)
        {
            camerafollowgoTarget = TranscriptManager.Instance.player.transform; //空指针？
        }
        else
        {
            if (FindNearestTargetGo() != null)
            {
                camerafollowgoTarget = FindNearestTargetGo().transform;
            }
        }
        UIFollowGoTarget();
        GoFollowGoTarget();
        //CameraFollowGoTarget();
    }

    private void FixedUpdate()
    {
        CameraFollowGoTarget();
    }

    public void UIFollowGoTarget()
    {
        if (uifollowgoTarget == null)
        {
            return;
        }
        else
        { 
            transform.position = Camera.main.WorldToScreenPoint(uifollowgoTarget.position + uifollowgoOffset);
        }
    }

    public void GoFollowGoTarget()
    {
        if (gofollowgoTarget == null)
        {
            return;
        }
        else
        { 
            transform.position = gofollowgoTarget.position + gofollowgoOffset;
        }
    }

    public void CameraFollowGoTarget()
    {
        if (camerafollowgoTarget == null)
        {
            return;
        }
        else
        {
            //transform.position = camerafollowgoTarget.position + camerafollowgoOffset;
            Vector3 targetPos = camerafollowgoTarget.position + camerafollowgoOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
        }
    }




    GameObject FindNearestTargetGo()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject nearestGo = null;
        float distance = Mathf.Infinity;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - transform.position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                nearestGo = go;
                distance = curDistance;
            }
        }
        return nearestGo;
    } //最近的目标，通过数组查找 
}
