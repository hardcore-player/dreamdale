using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using Deal.Env;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Druid;
using System.Threading.Tasks;

namespace Deal
{


    /// <summary>
    /// 怪
    /// </summary>
    public class Monster5 : Enemy
    {
        public Animator animatorEffect;

        public override void OnAwake()
        {
            base.OnAwake();

            //this.roleAtt.HP = 250;
            //this.roleAtt.MaxHP = 250;
            //this.roleAtt.Attack = 20;
            //this.roleAtt.MoveSpeed = 8;
            //this.roleAtt.AttackSpeed = 1;
        }

        //public override void PlayAttack(BattleRoleBase role)
        //{
        //this.EnemyState = EnemyState.Attack;
        //this.AttackWeapon.attackTarget(role);
        //this.PlayAnimation(WorkshopToolEnum.AttackWeapon);

        //bodyAnimator.Play($"ani_Monster5Attack", 0, 0);
        //animatorEffect.Play($"ani_Monster5AttackEffect", 0, 0);

        //this.AttackInCd = true;
        //this._attackCdInterval = 0;
        //}


    }
}

