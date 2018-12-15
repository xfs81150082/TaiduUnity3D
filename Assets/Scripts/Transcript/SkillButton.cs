using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class SkillButton : MonoBehaviour {

    public PosType posType = PosType.Basic;
    public float coldTime = 4;
    private float coldTimer = 0;
    private float timer = 5;
    private Button skillBtn;
    private Image maskImage;
   
    
    
    private PlayerAnimation playerAnimation;


    private void Awake()
    {        
        

    }

    // Use this for initialization
    void Start () {

        /*
       
        if (TranscriptManager._instance.player != null)
        {
            playerAnimation = TranscriptManager._instance.player.GetComponent<PlayerAnimation>();

        }
		 */

        skillBtn = GetComponent<Button>();
        if (transform.Find("Mask"))
        {
            maskImage = transform.Find("Mask").GetComponent<Image>();
        }
       

	}
	
	// Update is called once per frame
	void Update () {
        if (maskImage == null) return;        
        if (coldTimer > 0)
        {
            coldTimer -= Time.deltaTime;
            maskImage.fillAmount = coldTimer / coldTime;
            if (coldTimer <= 0)
            {
                Enable();
            }
        }
        else
        {
            maskImage.fillAmount = 0;
        }
		
	}

     public void OnPress (bool isPress) 
    {
        if (TranscriptManager.Instance.player != null)  //TranscriptManager._instance.player 改为 TranscriptManager._instance
        {
            playerAnimation = TranscriptManager.Instance.player.GetComponent<PlayerAnimation>();
        }
        playerAnimation.OnAttacButtonClick(isPress, posType);
        if (isPress && maskImage != null) 
        {
            coldTimer = coldTime;            
            Disable();
        }
        
    }

    void Disable()
    {
       skillBtn.interactable = false;
            
    }
    void Enable()
    {      
        skillBtn.interactable = true;
        
    }

}
