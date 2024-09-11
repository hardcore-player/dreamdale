using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using Druid;
using UnityEngine;

namespace Deal
{

    public delegate void DelegateRoleHpUpdate(int hp, int maxHp);

    /// <summary>
    /// 战斗角色
    /// </summary>
    public class BattleRoleBase : RoleBase, BattleRoleInterface
    {
        [Header("血条UI")]
        public RoleDisplay roleDisplay;


        [Header("武器父节点")]
        public Transform weaponParent;

        [Header("头盔父节点")]
        public Transform headParent;

        // 出生动画节点
        public GameObject birth;

        /// <summary>
        ///  面板战斗数值
        /// </summary>
        private BattleRoleAtt _oriAtt;

        /// <summary>
        /// 实时属性
        /// </summary>
        private BattleRoleAtt _curAtt;

        // 武器
        private AttackWeapon attackWeapon;

        public AttackWeapon AttackWeapon { get => attackWeapon; set => attackWeapon = value; }
        public BattleRoleAtt OriAtt { get => _oriAtt; set => _oriAtt = value; }
        public BattleRoleAtt CurAtt { get => _curAtt; set => _curAtt = value; }

        private Vector3 _battleGrid;
        public Vector3 BattleGrid { get => _battleGrid; set => _battleGrid = value; }

        public DelegateRoleHpUpdate OnRoleHpUpdate;

        //private void Start()
        //{
        //    this.roleDisplay.UpdateHp((int)this._curAtt.HP, (int)this._curAtt.MaxHP);
        //}


        /// <summary>
        /// 添加攻击武器
        /// </summary>
        /// <param name="weaponId"></param>
        public virtual void AddBattleWeapon(Data_Equip weapon)
        {
            if (weapon == null) return;

            if (this.AttackWeapon != null)
            {
                this.RemoveWeapon(this.AttackWeapon);
                Destroy(this.AttackWeapon.gameObject);
                this.AttackWeapon = null;
            }

            ExcelData.Equip equip = ConfigManger.I.GetEquipCfg(weapon.equipId);
            if (equip != null && equip.prefab != null)
            {
                GameObject go = Instantiate(equip.prefab, this.weaponParent);
                this.AttackWeapon = go.GetComponent<AttackWeapon>();
                this.AttackWeapon.Actor = this;
                go.transform.localRotation = Quaternion.Euler(0, 0, 0);

                this.AddWeapon(this.AttackWeapon);
            }
        }

        public RoleDamageData GetAttackDamage(BattleRoleBase role)
        {
            return this.GetAttackDamage(role.CurAtt);
        }

        public RoleDamageData GetAttackDamage(BattleRoleAtt roleAtt)
        {
            return this.CurAtt.GetDamage(roleAtt);
        }

        public virtual void OnDamage(BattleRoleBase attacker)
        {
            RoleDamageData damageData = attacker.GetAttackDamage(this);

            if (damageData.Hit)
            {
                this.CurAtt.HP -= damageData.Damage;

                if (this.CurAtt.HP <= 0)
                {
                    this.CurAtt.HP = 0;
                    this.OnDie();
                }

                this.roleDisplay.UpdateHp((int)this.CurAtt.HP, (int)this.CurAtt.MaxHP);

                if (this.OnRoleHpUpdate != null)
                {
                    this.OnRoleHpUpdate((int)this.CurAtt.HP, (int)this.CurAtt.MaxHP);
                }
            }
            DealUtils.newDamageNum(damageData, this);
            DealUtils.newDamageEffect(this);
        }

        public virtual void OnAttack(BattleRoleBase role)
        {
            this.PlayWeapon(WorkshopToolEnum.AttackWeapon);
            role.BefroeDamage(this);
            role.OnDamage(this);
            role.AfterDamage(this);
        }

        public virtual bool IsDie()
        {
            //return this.BattleState == BattleRoleStateEnum.DEAD;

            return this.CurAtt != null && this.CurAtt.HP <= 0;
        }

        public virtual void OnDie()
        {
            //this.BattleState = BattleRoleStateEnum.DEAD;
        }

        public virtual void OnBorn()
        {
        }

        public virtual void Look2Role(BattleRoleBase role)
        {
            bool lookRight = role.transform.position.x > this.transform.position.x;
            this.roleBody.localScale = new Vector3(lookRight ? 1 : -1, 1, 0);
        }

        public virtual void OnFightEnter(BattleRoleBase role)
        {
        }

        public virtual void OnFightStart(BattleRoleBase role)
        {
        }

        public virtual void OnFightEnd(BattleRoleBase role)
        {
        }

        public virtual void OnFightExit(BattleRoleBase role)
        {
        }

        public virtual void BefroeDamage(BattleRoleBase role)
        {
        }

        public virtual void AfterDamage(BattleRoleBase role)
        {
        }

        public virtual void BefroeAttack(BattleRoleBase role)
        {
        }

        public virtual void AfterAttack(BattleRoleBase role)
        {
        }
    }
}
