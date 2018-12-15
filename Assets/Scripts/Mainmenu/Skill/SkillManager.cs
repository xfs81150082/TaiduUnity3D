using System.Collections;
using System.Collections.Generic;
using TumoCommon.Model;
using UnityEngine;

public class SkillManager : MonoBehaviour {
    public static SkillManager _instance;

    public TextAsset skillinfoText;
    private ArrayList skillList = new ArrayList();
    private Dictionary<int, Skill> skillDict = new Dictionary<int, Skill>();

    private SkillDBController skillDbController;

    public event OnSyncSkillCompleteEvent OnSyncSkillComplete;

    private void Awake()
    {
        _instance = this;
        skillDbController = this.GetComponent<SkillDBController>();
        skillDbController.OnGetSkillDBList += this.OnGetSkillDBList;
        skillDbController.OnUpgradeSkillDB += this.OnUpgradeSkillDB;

        InitSkill();
        skillDbController.Get();

    }

    void InitSkill()
    {
        string[] skillArray = skillinfoText.ToString().Split('\n');
        foreach (string str in skillArray)
        {
            string[] proArray = str.Split(',');
            Skill skill = new Skill();
            skill.Id = int.Parse(proArray[0]);
            skill.Name = proArray[1];
            skill.Icon = proArray[2];
            switch (proArray[3])
            {
                case "Warrior":
                    skill.PlayerType = PlayerType.Warrior;
                    break;
                case "FemaleAssassin":
                    skill.PlayerType = PlayerType.FemaleAssassin;
                    break;
            }
            switch (proArray[4])
            {
                case "Basic":
                    skill.SkillType = SkillType.Basic;
                    break;
                case "Skill":
                    skill.SkillType = SkillType.Skill;
                    break;
            }
            switch (proArray[5])
            {
                case "Basic":
                    skill.PosType = PosType.Basic;
                    break;
                case "One":
                    skill.PosType = PosType.One;
                    break;
                case "Two":
                    skill.PosType = PosType.Two;
                    break;
                case "Three":
                    skill.PosType = PosType.Three;
                    break;
            }
            skill.ColdTime = int.Parse(proArray[6]);
            skill.Damage = int.Parse(proArray[7]);
            skill.Level = 1;
            skillList.Add(skill);
            skillDict.Add(skill.Id,skill);
        }
    }

    public void OnGetSkillDBList(List<SkillDB> list)
    {
        foreach (var skillDB in list)
        {
            Skill skill = null;
            if (skillDict.TryGetValue(skillDB.SkilId, out skill))
            {
                skill.Sync(skillDB);
            }
        }
        if (OnSyncSkillComplete != null)
        {
            OnSyncSkillComplete();
        }
    }

    public Skill GetSkillByPostion(PosType posType)
    {
        PlayerInfo info = PlayerInfo.Intance;
        foreach (Skill skill in skillList)
        {
            if (skill.PlayerType == info.PlayerType && skill.PosType == posType) {
                return skill;
            }
        }
        return null;
    }

    public void Upgrade(Skill skill)
    {
        skillDbController.Upgrade(skill.SkillDb);
    }

    public void OnUpgradeSkillDB(SkillDB skillDb)
    {
        Skill skill;
        if (skillDict.TryGetValue(skillDb.SkilId, out skill))
        {
            skill.Sync(skillDb);
        }
    }


}
