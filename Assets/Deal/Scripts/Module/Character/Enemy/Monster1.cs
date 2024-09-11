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
    public class Monster1 : Enemy
    {
        public override void OnAwake()
        {
            base.OnAwake();

            //this.CurAtt.HP = 100;
            //this.roleAtt.MaxHP = 100;
            //this.roleAtt.Attack = 10;
            //this.roleAtt.MoveSpeed = 10;
            //this.roleAtt.AttackSpeed = 2;
        }

    }
}

