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
    /// 怪
    /// </summary>
    public class Monster2 : Enemy
    {
        private List<Vector3> movePos = new List<Vector3>();
        private int movePidx = 0;

        public override void OnAwake()
        {
            base.OnAwake();

            //this.roleAtt.HP = 150;
            //this.roleAtt.MaxHP = 150;
            //this.roleAtt.Attack = 15;
            //this.roleAtt.MoveSpeed = 8;
            //this.roleAtt.AttackSpeed = 5;

            // 散步坐标
            movePos.Add(transform.position + new Vector3(1.5f, 0, 0));
            movePos.Add(transform.position + new Vector3(-1.5f, 0, 0));

            this.movePidx = 0;
        }

        private void MoveInPos()
        {
            //// 追踪
            //if (this.movePos.Count == 0) return;

            //Vector3 toPos = this.movePos[this.movePidx];

            //Vector3 dir = toPos - this.transform.position;
            //Vector3 dst = dir.normalized * Time.deltaTime * this.roleAtt.MoveSpeed * 0.2f;

            //this.transform.Translate(dst);

            //this.LookDir(dir);

            //if (Vector3.Distance(toPos, this.transform.position) < 0.1f)
            //{
            //    this.movePidx++;
            //    if (this.movePidx >= this.movePos.Count)
            //    {
            //        this.movePidx = 0;
            //    }
            //}
        }


        /// <summary>
        /// 移动行为
        /// </summary>
        //protected override void UpdateMove()
        //{
        //if (this.AttackWeapon == null) return;

        //Hero hero = PlayManager.I.mHero;
        //this.MoveInPos();

        //this.LookAtHero(hero);
        //this.CheckAttackHero();
        //}

    }
}

