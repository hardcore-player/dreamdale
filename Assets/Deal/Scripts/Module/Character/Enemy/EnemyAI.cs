using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deal
{
    public enum EnemyState
    {
        Idle, // 空闲
        Move, // 移动
        Attack, // 攻击
        Dead, // 死亡
        Stop, // 停止
    }

    // 移动策略
    public enum EnemyMoveTactics
    {
        CHASE, // 追逐
        STROLL, // 溜达    
    }

    public class EnemyAI : MonoBehaviour
    {
        protected Enemy _actor;
        protected Hero _attackTarget;

        // 移动策略
        protected EnemyMoveTactics _curTactics = EnemyMoveTactics.CHASE;


        private void Awake()
        {
            this._actor = this.GetComponent<Enemy>();
        }

        /// <summary>
        /// 移动行为
        /// </summary>
        protected virtual void UpdateMove()
        {
        }

        /// <summary>
        /// idle行为
        /// </summary>
        protected virtual void UpdateIdle()
        {
        }

        /// <summary>
        /// 攻击行为
        /// </summary>
        protected virtual void UpdateAttack()
        {
        }

        //protected virtual void UpdateAttackCd()
        //{
        //}


        void Update()
        {
            if (this._actor == null) return;

            //switch (this._actor.EnemyState)
            //{
            //    case EnemyState.Dead:
            //        break;
            //    case EnemyState.Idle:
            //        this.UpdateIdle();
            //        break;
            //    case EnemyState.Move:
            //        this.UpdateMove();
            //        break;
            //    case EnemyState.Attack:
            //        this.UpdateAttack();
            //        break;
            //    default:
            //        break;
            //}
        }
    }

}