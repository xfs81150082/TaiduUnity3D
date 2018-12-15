using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    private static BossHpBar _instance;
    public static BossHpBar Instance
    {
        get { return _instance; }
    }

    private Slider hpSlider;
    private Text hpText;
    private int maxHp;

    void Awake()
    {
        _instance = this;
        hpSlider = this.GetComponent<Slider>();
        hpText = transform.Find("HpText").GetComponent<Text>();
        this.gameObject.SetActive(false);

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Show(int maxHp)
    {
        this.maxHp = maxHp;
        this.gameObject.SetActive(true);
        UpdateShow(maxHp);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void UpdateShow(int currentHp)
    {
        if (currentHp < 0) currentHp = 0;
        hpSlider.value = currentHp / (float)maxHp;
        hpText.text = currentHp + "/" + maxHp;

    }




}
