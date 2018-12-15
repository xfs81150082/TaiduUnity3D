using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationModel  {
    public  List<EnemyAnimtionProperty> list = new List<EnemyAnimtionProperty>();
	
}

public class EnemyAnimtionProperty
{
    public string guid;
    public bool isIdle;
    public bool isWalk;
    public bool isAttack;
    public bool isTakeDamage;
    public bool isDie;
}