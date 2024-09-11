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
    public class Monster6 : Enemy
    {
        private Vector3 _moveDir;

        public override void OnAwake()
        {
            base.OnAwake();

            //this.roleAtt.HP = 400;
            //this.roleAtt.MaxHP = 400;
            //this.roleAtt.Attack = 30;
            //this.roleAtt.MoveSpeed = 5;
            //this.roleAtt.AttackSpeed = 5;
        }

    }
}

