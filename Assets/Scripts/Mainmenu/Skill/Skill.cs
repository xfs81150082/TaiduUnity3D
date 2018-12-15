﻿using System.Collections;
using System.Collections.Generic;
using TumoCommon.Model;
using UnityEngine;

public enum SkillType
{
    Basic,Skill
}

public enum PosType
{
    Basic,One,Two,Three
}

public class Skill  {

    private int id;
    private string name;
    private string icon;
    private PlayerType playerType;
    private SkillType skillType;
    private PosType posType;
    private int coldTime;
    private int damage;
    private int level = 1;

    private SkillDB skillDb;

    public void Sync(SkillDB skillDb)
    {
        this.SkillDb = skillDb;
        Level = skillDb.Level;
    }

    public SkillDB SkillDb
    {
        get { return skillDb; }
        set { skillDb = value; }
    }



#region
    public int Id
    {
        get
        {
            return id;
        }

        set
        {
            id = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public string Icon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    public PlayerType PlayerType
    {
        get
        {
            return playerType;
        }

        set
        {
            playerType = value;
        }
    }

    public SkillType SkillType
    {
        get
        {
            return skillType;
        }

        set
        {
            skillType = value;
        }
    }

    public PosType PosType
    {
        get
        {
            return posType;
        }

        set
        {
            posType = value;
        }
    }

    public int ColdTime
    {
        get
        {
            return coldTime;
        }

        set
        {
            coldTime = value;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }

        set
        {
            damage = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            level = value;
        }
    }
#endregion

    public void Upgrade()
    {
        level++;
        if (skillDb == null)
        {
            skillDb=new SkillDB();
            skillDb.Level = Level;
            skillDb.SkilId = id;
        }
        else
        {
            skillDb.Level = Level;
        }
    }

}