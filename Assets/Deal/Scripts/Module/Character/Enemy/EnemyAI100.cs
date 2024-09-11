using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deal
{


    public class EnemyAI100 : EnemyAI
    {

        protected float _attackInterval = 0;


        // 溜达
        private float _strollInterval = 0f;
        private float _strollTime = 1f;
        private Vector3 _strollDir = new Vector3(0, 0, 0);


        /// <summary>
        /// idle行为
        /// </summary>
        protected override void UpdateIdle()
        {
            this.findAttakTarget();
        }

        /// <summary>
        /// 移动行为
        /// </summary>
        protected override void UpdateMove()
        {

            switch (this._curTactics)
            {
                case EnemyMoveTactics.CHASE:
                    this.moveChase();
                    break;
                case EnemyMoveTactics.STROLL:
                    this.moveStroll();
                    break;
                default:
                    break;
            }

            this.checkAttackHero();
        }

        /// <summary>
        /// 攻击行为
        /// </summary>
        protected override void UpdateAttack()
        {
            if (this._actor == null || this._attackTarget == null) return;


            //this._attackInterval += Time.deltaTime;
            //if (this._attackInterval >= 1f)
            //{
            //    this._actor.EnemyState = EnemyState.Idle;
            //}

            //if (this._actor.AttackWeapon != null)
            //{
            //    this._actor.LookAtHero(this._attackTarget);
            //}

            //if (this._actor.AttackWeapon != null && this.AttackWeapon is WeaponRemote)
            //{
            //    Hero hero = PlayManager.I.mHero;
            //    this.LookAtHero(hero);
            //}
        }

        /// <summary>
        /// 检查攻击
        /// </summary>
        /// <param name="enemy"></param>
        private void checkAttackHero()
        {
            //if (this._actor == null || this._attackTarget == null) return;
            //if (this._actor.EnemyState != EnemyState.Move) return;

            //if (this._actor.AttackInCd == true) return;

            //Hero hero = this._attackTarget;
            //float dis = Vector3.Distance(this.transform.position, hero.transform.position);

            //if (dis <= this._actor.AttackWeapon.attckLength())
            //{
            //    this._actor.PlayAttack(hero);
            //    this._attackInterval = 0;
            //}

        }

        /// <summary>
        /// 寻找攻击目标
        /// </summary>
        private void findAttakTarget()
        {
            // 寻找玩家

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
            //        // 追击
            //        this._curTactics = EnemyMoveTactics.CHASE;
            //    }
            //}
        }


        /// <summary>
        /// 跟随攻击目标
        /// </summary>
        protected void moveChase()
        {
            //if (this._actor == null || this._attackTarget == null) return;


            //// 追踪
            //Hero hero = this._attackTarget;
            //float dis = Vector3.Distance(hero.transform.position, this.transform.position);

            //Vector3 dir = hero.transform.position - this.transform.position;

            //if (dis > this._actor.AttackWeapon.attckLength())
            //{
            //    Vector3 dst = dir.normalized * Time.deltaTime * this._actor.roleAtt.MoveSpeed * 0.2f;
            //    this.transform.Translate(dst);
            //}

            //this._actor.LookDir(dir);
            //this._actor.LookAtHero(this._attackTarget);
        }


        // 溜达
        protected void moveStroll()
        {
            //if (this._strollTime <= 0)
            //{
            //    this._strollInterval = 0;
            //    this._strollTime = Druid.Utils.MathUtils.RandomInt(500, 1500) / 1000f;

            //    float x = (float)Druid.Utils.MathUtils.RandomDouble(-90, 90);
            //    float y = (float)Druid.Utils.MathUtils.RandomDouble(-90, 90);

            //    this._strollDir = new Vector3(x, y, 0).normalized;
            //}

            //this._strollInterval += Time.deltaTime;

            //Hero hero = PlayManager.I.mHero;
            //Vector3 dir = this._strollDir;

            //Vector3 dst = dir.normalized * Time.deltaTime * this._actor.roleAtt.MoveSpeed * 0.2f;
            //this.transform.Translate(dst);

            //if (this._strollInterval >= this._strollTime)
            //{
            //    this._strollTime = -1;
            //    if (this._actor.AttackInCd == false)
            //    {
            //        this._curTactics = EnemyMoveTactics.CHASE;
            //    }
            //}

            ////this._actor.LookDir(dir);
            //this._actor.LookAtHero(this._attackTarget);
        }

    }

}