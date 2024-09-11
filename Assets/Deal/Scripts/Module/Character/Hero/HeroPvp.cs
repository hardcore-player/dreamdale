using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using DG.Tweening;
using Deal.UI;
using Deal.Dungeon;
using Deal.Data;
using System;
using Deal.Msg;

namespace Deal
{

    /// <summary>
    /// 主角英雄
    /// </summary>
    public class HeroPvp : BattleRole
    {
        public SpriteRenderer srWeapon;
        public SpriteRenderer srHead;

        public DelegateHeroFightEnter OnHeroFightEnter;
        public DelegateHeroFightStart OnHeroFightStart;
        public DelegateHeroFightEnd OnHeroFightEnd;
        public DelegateHeroFightExit OnHeroFightExit;
        public DelegateHeroDie OnHeroFightDie;

        public override void OnAwake()
        {

        }

        /// <summary>
        /// 重制战斗属性
        /// </summary>
        public void RestBattleBaseAtt(Msg_Data_Arena_Playerinfo data)
        {

            this.OriAtt = new BattleRoleAtt();

            this.OriAtt.MaxHP = data.max_hp;
            this.OriAtt.HP = data.hp;
            this.OriAtt.Attack = data.attack;
            this.OriAtt.Crit = data.crit;
            this.OriAtt.Dodge = data.dodge;
            this.OriAtt.Hit = data.hit;
            this.OriAtt.DeCrit = data.decrit;
            this.OriAtt.HPReg = data.hpreg;
            this.OriAtt.AttackSpeed = data.attack_speed;

            this.CurAtt = this.OriAtt.Clone();

            this.AddEquip(data.weapon, EquipPointEnum.weapon);
            this.AddEquip(data.hat, EquipPointEnum.head);

            this.UpdateHP();
        }

        public void UpdateHP()
        {
            this.roleDisplay.UpdateHp((int)this.CurAtt.HP, (int)this.CurAtt.MaxHP);
        }



        public override void OnDie()
        {
            base.OnDie();

            if (this.OnHeroFightDie != null)
            {
                this.OnHeroFightDie(this);
            }
        }

        public override void PlayBorn()
        {
            this.birth.SetActive(true);
            Animator animator = this.birth.GetComponent<Animator>();
            animator.speed = 1;
            animator.Play($"ani_birth0", 0, 0);
        }

        /// <summary>
        /// 战斗开始
        /// </summary>
        /// <param name="role"></param>
        public override void OnFightEnter(BattleRoleBase role)
        {
            base.OnFightEnter(role);

            if (this.OnHeroFightEnter != null)
            {
                this.OnHeroFightEnter(role);
            }

        }

        public override void OnFightStart(BattleRoleBase role)
        {
            base.OnFightStart(role);

            if (this.OnHeroFightStart != null)
            {
                this.OnHeroFightStart(role);
            }
        }

        /// <summary>
        /// 战斗结束
        /// </summary>
        /// <param name="role"></param>
        public override void OnFightEnd(BattleRoleBase role)
        {
            base.OnFightEnd(role);

            if (this.OnHeroFightEnd != null)
            {
                this.OnHeroFightEnd(role);
            }
        }

        public override void OnFightExit(BattleRoleBase role)
        {
            base.OnFightExit(role);

            if (this.OnHeroFightExit != null)
            {
                this.OnHeroFightExit(role);
            }

        }


        /// <summary>
        /// 添加攻击武器
        /// </summary>
        /// <param name="weaponId"></param>
        public void AddEquip(int equipId, EquipPointEnum point)
        {
            if (equipId > 0)
            {
                ExcelData.Equip equip = ConfigManger.I.GetEquipCfg(equipId);

                if (point == EquipPointEnum.weapon)
                {
                    SpriteUtils.SetRoleEquipIcon(this.srWeapon, equipId);
                }
                else if (point == EquipPointEnum.head)
                {
                    SpriteUtils.SetRoleEquipIcon(this.srHead, equipId);
                }
            }
            else
            {
                if (point == EquipPointEnum.weapon)
                {
                    SpriteUtils.SetRoleEquipIcon(this.srWeapon, 0);
                }
                else if (point == EquipPointEnum.head)
                {
                    SpriteUtils.SetRoleEquipIcon(this.srHead, 0);
                }
            }
        }
    }
}

