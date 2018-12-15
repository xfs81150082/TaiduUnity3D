using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItemUI : MonoBehaviour {

    public PosType posType;
    private Skill skill;
    public bool isSelect = true;

    private Image skillImage;

    public Image SkillImage
    {
        get
        {
            if(skillImage==null)
            {
                skillImage = this.GetComponent<Image>();
            }
            return skillImage;
        }
    }

    private void Start()
    {
        SkillManager._instance.OnSyncSkillComplete += this.OnSyncSkillCompelte;
    }

    public void OnSyncSkillCompelte()
    {
         UpdateShow();
        if (isSelect)
        {
            OnSkillClick();
        }
    }

    void UpdateShow()
    {
        skill = SkillManager._instance.GetSkillByPostion(posType);
        SkillImage.overrideSprite = Resources.Load(skill.Icon, typeof(Sprite)) as Sprite;

    }
    
    public  void OnSkillClick()
    {
        transform.parent.SendMessage("OnSkillClick",skill);
    }

    void OnDestory()
    {
        if (SkillManager._instance != null)
        {
            SkillManager._instance.OnSyncSkillComplete -= this.OnSyncSkillCompelte;
        }
    }
	
}
