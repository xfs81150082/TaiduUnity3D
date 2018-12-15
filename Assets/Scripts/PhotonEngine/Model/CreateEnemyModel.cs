using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用这个类来传递敌人的数据
public class CreateEnemyModel
{
    public List<CreateEnemyProperty> List = new List<CreateEnemyProperty>();
}

//存储创建一个敌人所需要的属性
public class CreateEnemyProperty
{
    public string guid;              //表示一个敌人的GUID
    public string prefabName;        //根据prefab的name找到prefab进行生成
    public Vector3Obj position;      //表示在哪里生产prefab
    public int targetRoleID;
}