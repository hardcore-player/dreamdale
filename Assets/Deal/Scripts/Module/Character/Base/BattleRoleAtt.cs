using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deal
{
    public class RoleDamageData
    {
        public float Damage;
        public bool Crit;
        public bool Hit;

        public RoleDamageData(float damage, bool crit, bool hit)
        {
            Damage = damage;
            Crit = crit;
            Hit = hit;
        }
    }

    // 玩家的战斗数值
    public class BattleRoleAtt
    {

        public float MaxHP;
        public float HP;
        public float Attack;  // 攻击
        public float Crit; // 暴击
        public float Dodge; // 闪避
        public float Hit; // 命中
        public float DeCrit; // 抗暴
        public float HPReg; // 生命恢复
        public float AttackSpeed; // 攻击速度

        public BattleRoleAtt()
        {
            this._init();
        }


        public BattleRoleAtt Clone()
        {
            BattleRoleAtt c = new BattleRoleAtt();
            c.MaxHP = this.MaxHP;
            c.HP = this.HP;
            c.Attack = this.Attack;
            c.Crit = this.Crit;
            c.Dodge = this.Dodge;
            c.Hit = this.Hit;
            c.DeCrit = this.DeCrit;
            c.HPReg = this.HPReg;
            c.AttackSpeed = this.AttackSpeed;

            return c;
        }

        /// <summary>
        /// 获得伤害
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual RoleDamageData GetDamage(BattleRoleAtt roleAtt)
        {
            float damage = this.Attack;

            // 暴击
            bool isCrit = false;
            float critPro = 5 + this.Crit - roleAtt.DeCrit;
            int r = Druid.Utils.MathUtils.RandomInt(0, 100);
            if (r < critPro)
            {
                isCrit = true;
                damage *= 1.5f;
            }

            // 命中
            bool isHit = false;
            float hitPro = 100 + this.Hit - roleAtt.Dodge;
            int r1 = Druid.Utils.MathUtils.RandomInt(0, 100);
            if (r1 < hitPro)
            {
                isHit = true;
            }

            return new RoleDamageData((int)damage, isCrit, isHit);
        }

        private void _init()
        {
            MaxHP = 200;
            HP = 200;
            Attack = 50;

            Crit = 0;
            Dodge = 0;
            Hit = 0;
            DeCrit = 0;
            HPReg = 0;
            AttackSpeed = 1f;
        }

        public static BattleRoleAtt operator +(BattleRoleAtt c1, BattleRoleAtt c2)
        {
            if (c1 == null) return c2;
            if (c2 == null) return c1;
            if (c2 == null && c1 == null) return null;

            BattleRoleAtt c = new BattleRoleAtt();
            c.MaxHP = c1.MaxHP + c2.MaxHP;
            c.HP = c1.HP + c2.HP;
            c.Attack = c1.Attack + c2.Attack;
            c.Crit = c1.Crit + c2.Crit;
            c.Dodge = c1.Dodge + c2.Dodge;
            c.Hit = c1.Hit + c2.Hit;
            c.DeCrit = c1.DeCrit + c2.DeCrit;
            c.HPReg = c1.HPReg + c2.HPReg;
            //c.AttackSpeed = this.AttackSpeed;

            return c;
        }


    }
}