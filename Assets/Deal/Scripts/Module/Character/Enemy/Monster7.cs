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
    public class Monster7 : Enemy
    {
        public float _atackInterval = 0;

        public override void OnAwake()
        {
            base.OnAwake();

            //this.roleAtt.HP = 150;
            //this.roleAtt.MaxHP = 150;
            //this.roleAtt.Attack = 15;
            //this.roleAtt.MoveSpeed = 5;
        }

        void Update()
        {
            //if (this.EnemyState == EnemyState.Dead) return;

            //if (EnemyState == EnemyState.Idle)
            //{
            //    this.FindTarget();
            //}
            //else if (EnemyState == EnemyState.Move)
            //{
            //    this.FollowTarget();
            //    this.AttackTarget();
            //}
        }

        private void AttackTarget()
        {
            this._atackInterval += Time.deltaTime;
            if (this._atackInterval >= 3)
            {

                Hero hero = PlayManager.I.mHero;
                if (Vector3.Distance(hero.transform.position, this.transform.position) <= 2)
                {
                    this._atackInterval = 0;
                    //this.NewBullet();
                }

            }
        }


    }
}

