using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour {
    

    private Image wopingImage;
    private Text nameText;
    private Text doesText;    
    private Text piliangText;
    private InventoryItem it;
    private InventoryItemUI itUI;

    private void Awake()
    {
        wopingImage = transform.Find("WoPingImage").GetComponent<Image>();
        nameText = transform.Find("NameText").GetComponent<Text>();
        doesText = transform.Find("DoesText").GetComponent<Text>();
        piliangText = transform.Find("PiLiangImage/Text").GetComponent<Text>();
        
    }

    // Use this for initialization
    void Start () {        
        gameObject.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Show(InventoryItem it,InventoryItemUI itUI, bool isLeft = true)
    {
        gameObject.SetActive(true);        
        this.it = it;
        this.itUI = itUI; 
        wopingImage.overrideSprite = Resources.Load(it.Inventory.Icon, typeof(Sprite)) as Sprite;
        nameText.text = it.Inventory.Name;
        doesText.text = it.Inventory.Des;
        piliangText.text = "批量使用(" + it.Count + ")";
    }   
    
    public void OnClose()
    {
        Close();
        GameObject.Find("Knapsack").SendMessage("DisableButton");
    }
    public void Close()
    {
        Clear();
        gameObject.SetActive(false);
    }

    public void OnUse()
    {
        itUI.ChangeCount(1);
        PlayerInfo.Intance.InventoryUse(it,1);
        OnClose();
    }
    public void OnUseBatching()
    {
        itUI.ChangeCount(it.Count);
        PlayerInfo.Intance.InventoryUse(it,it.Count);
        OnClose();
    }
    void Clear()
    {
        this.it = null;
        this.itUI = null;
    }

}
