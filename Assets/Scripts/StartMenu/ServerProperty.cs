using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerProperty : MonoBehaviour {
    public string ip = "127.0.0.1:4530";
    private string _name;
    private int _count = 100;
    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value;
            transform.Find("ServerNameText").GetComponent<Text>().text = _name;
        }
    }
    public int Count
    {
        get
        {
            return _count;
        }

        set
        {
            _count = value;
        }
    }

    public void OnPress(bool isPress)
    {
        if (isPress == false)
        {
            //选择了当前的服务器            
            GameObject.Find("StartMenuCanvas").SendMessage("OnServerselect", this.gameObject);
            
        }
    }
}
