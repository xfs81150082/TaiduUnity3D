using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.PhotonEngine.Model;
using ExitGames.Client.Photon;
using TumoCommon;
using TumoCommon.Tools;
using UnityEngine;
using TumoCommon.Model;

public delegate void OnSyncTaskCompleteEvent();

public delegate void OnGetRoleEvent(List<Role> rolelList);

public delegate void OnAddRoleEvent(Role role);

public delegate void OnSelectRoleEvent();

public delegate void OnGetTaskDBListEvent(List<TaskDB> list);

public delegate void OnAddTaskDBEvent(TaskDB taskDb);

public delegate void OnUpdateTaskDBEvent();

public delegate void OnGetInventoryItemDBListEvent(List<InventoryItemDB> list);

public delegate void OnAddInventoryItemDBEvent(InventoryItemDB itemDb);

public delegate void OnUpdateInventoryItemDBEvent();

public delegate void OnUpdateInventoryItemDBListEvent();

public delegate void OnUpgradeEquipEvent();

public delegate void OnGetSkillDBEvent(List<SkillDB> list);

public delegate void OnAddSkillDBEvent(SkillDB skillDb);

public delegate void OnUpdateSkillDBEvent();

public delegate void OnUpgrateSkillDBEvent(SkillDB skillDb);

public delegate void OnSyncSkillCompleteEvent();

public delegate void OnPlayerHpChangeEvent(int hp);

public delegate void OnGetTeamEvent(List<Role> rolelList,int masterRoleId);  //组队成功

public delegate void OnWaitingTeamEvent(); //等待组队

public delegate void OnCancelTeamEvent();//取消组队响应

public delegate void OnSyncPositionAndRotationEvent(int roleid,Vector3 position,Vector3 eulerAngles);

public delegate void OnSyncMoveAnimationEvent(int roleId,PlayerMoveAnimationModel model);

public delegate void OnCreateEnemyEvent(CreateEnemyModel model);

public delegate void OnSyncEnemyPositionAndRotationEvent(EnemyPositionModel model);

public delegate void OnSyncEnemyAnimationEvent(EnemyAnimationModel model);

public delegate void OnSyncPlayerAnimationEvent(int roleId, PlayerAnimationModel model);

public delegate void OnGameStateChangeEvent(GameStateModel model);

public delegate void OnSyncBossAnimationEvent(BossAnimationModel model);