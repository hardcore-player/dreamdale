using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deal
{

    /// <summary>
    /// 只会溜达，cd好了就攻击
    /// </summary>
    public class EnemyAI100_2 : EnemyAI100
    {
        /// <summary>
        /// idle行为
        /// </summary>
        protected override void UpdateIdle()
        {
            this.findAttakTarget();
        }

        /// <summary>
        /// 寻找攻击目标
        /// </summary>
        private void findAttakTarget()
        {
            //// 寻找玩家

            //Hero hero = PlayManager.I.mHero;
            //if (hero == null || hero.IsDie()) return;

            //float dis = Vector3.Distance(this.transform.position, hero.transform.position);

            //if (dis <= 100)
            //{
            //    this._attackTarget = hero;
            //    this._actor.EnemyState = EnemyState.Move;

            //    if (this._actor.AttackInCd == true)
            //    {
            //        //溜达
            //        this._curTactics = EnemyMoveTactics.STROLL;
            //    }
            //    else
            //    {
            //        // 溜达EnemyAI100_2
            //        this._curTactics = EnemyMoveTactics.STROLL;
            //    }
            //}
        }

    }

}