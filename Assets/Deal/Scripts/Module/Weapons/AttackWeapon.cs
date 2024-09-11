using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deal
{
    /// <summary>
    /// 战斗武器
    /// </summary>
    public class AttackWeapon : Weapon
    {
        public float AniSpeed = 1;

        public float AttackLenth = 1f;


        public virtual float attckLength()
        {
            return this.AttackLenth;
        }

        /// <summary>
        /// 攻击范围
        /// </summary>
        /// <param name="tagert"></param>
        public virtual void attackTarget(BattleRoleBase tagert)
        {
        }
    }
}

