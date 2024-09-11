using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using Deal.Env;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Druid;

namespace Deal
{


    /// <summary>
    /// 小怪8：朝目标移动，初始速度3，随时间移速逐渐变快，每秒增加0.3，血量600，攻击力50
    /// </summary>
    public class Monster8 : Enemy
    {

        public override void OnAwake()
        {
            base.OnAwake();

            //this.roleAtt.HP = 600;
            //this.roleAtt.MaxHP = 600;
            //this.roleAtt.Attack = 50;
            //this.roleAtt.MoveSpeed = 3;
        }

        //void Update()
        //{
        //    if (this.EnemyState == EnemyState.Dead) return;

        //    if (EnemyState == EnemyState.Idle)
        //    {
        //        this.FindTarget();
        //    }
        //    else if (EnemyState == EnemyState.Move)
        //    {
        //        this.roleAtt.MoveSpeed += 0.3f * Time.deltaTime;

        //        this.FollowTarget();
        //    }
        //}


    }
}

