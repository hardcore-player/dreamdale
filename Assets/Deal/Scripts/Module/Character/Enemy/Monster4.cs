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
    public class Monster4 : Enemy
    {
        private float _atackInterval = 0;

        public override void OnAwake()
        {
            base.OnAwake();

            //this.roleAtt.HP = 100;
            //this.roleAtt.MaxHP = 100;
            //this.roleAtt.Attack = 10;
            //this.roleAtt.MoveSpeed = 8;
            //this.roleAtt.AttackSpeed = 5;
        }

    }
}

