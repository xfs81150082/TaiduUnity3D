using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private Animator anim;
    private PlayerAttack playerAttack;

    private Player player;
    private BattleController battleController;
    private bool isSyncPlayerAnimation = false;//表示是否需要同步动画
    
    // Use this for initialization
    void Start () {
        player = this.GetComponent<Player>();
        if (GameController.Instance.battleType == BattleType.Team && player.roleId == PhotonEngine.Instance.role.Id) //当前角色是团队战斗，且属于当前客户端
        {
            battleController = GameController.Instance.GetComponent<BattleController>();
            isSyncPlayerAnimation = true;
        }
        anim = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }
	
    public void OnAttacButtonClick(bool isPress,PosType posType)
    {
        if (playerAttack.hp < 0) return;
        anim.SetTrigger("Skill"+posType);



        //发起动画同步的申请
        if(isSyncPlayerAnimation)
        {
            if (isPress)
            {
                switch ((int)posType)
                {
                    case 0:
                        PlayerAnimationModel model0 = new PlayerAnimationModel() { skillBasic = true };
                        battleController.SyncPlayerAnimation(model0);
                        break;
                    case 1:
                        PlayerAnimationModel model1 = new PlayerAnimationModel() { skillOne = true };
                        battleController.SyncPlayerAnimation(model1);
                        break;
                    case 2:
                        battleController.SyncPlayerAnimation(new PlayerAnimationModel() {skillTwo = true});
                        break;
                    case 3:
                        battleController.SyncPlayerAnimation(new PlayerAnimationModel() {skillThree = true});
                        break;
                }

            }
            else
            {
                PlayerAnimationModel model =new PlayerAnimationModel();
                battleController.SyncPlayerAnimation(model);
            }

        }
    }

    public void SyncAnimation(PlayerAnimationModel model)
    {
        if (model.skillBasic)
        {
            anim.SetTrigger("SkillBasic");
        }
        else if (model.skillOne)
        {
            anim.SetTrigger("SkillOne");
        }
        else if (model.skillTwo)
        {
            anim.SetTrigger("SkillTwo");
        }
        else if (model.skillThree)
        {
            anim.SetTrigger("SkillThree");
        }
        else if (model.takeDamage)
        {
            anim.SetTrigger("TakeDamage");
        }
        else if (model.die)
        {
            anim.SetTrigger("Die");
        }

    }

}
