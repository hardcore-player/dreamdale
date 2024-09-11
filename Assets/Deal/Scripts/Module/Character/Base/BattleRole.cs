using System.Collections;
using System.Collections.Generic;
using Druid;
using UnityEngine;

namespace Deal
{


    /// <summary>
    /// 战斗角色
    /// </summary>
    public class BattleRole : BattleRoleBase
    {
        // 攻击目标
        private BattleRoleBase _target;
        public BattleRoleBase Target { get => _target; set => _target = value; }

        // 攻击间隔
        private float _attackInterval = 0;


        public void SetBattleGrid(Vector3 grid)
        {
            this.BattleGrid = grid;
        }

        public override void OnFightEnter(BattleRoleBase role)
        {
            if (this.CurAtt == null)
            {
                this.CurAtt = this.OriAtt.Clone();
            }


            this.Target = role;
            this._attackInterval = 0;

            this.Look2Role(role);

        }

        public override void OnFightStart(BattleRoleBase role)
        {
            this._attackInterval = this.CurAtt.AttackSpeed;
        }

        public override void OnFightEnd(BattleRoleBase role)
        {
            //this.Target = null;
            this._attackInterval = 0;
        }

        public override void OnFightExit(BattleRoleBase role)
        {
            this.Target = null;
        }

        public bool UpdateAttack()
        {
            this._attackInterval += Time.deltaTime;

            if (this._attackInterval >= this.CurAtt.AttackSpeed)
            {
                this.AttackTatget();
                this._attackInterval = 0;
            }

            return this.Target.IsDie();
        }


        /// <summary>
        /// 攻击
        /// </summary>
        public void AttackTatget()
        {
            this.BefroeAttack(this.Target);
            this.OnAttack(this.Target);
            this.AfterAttack(this.Target);
        }
    }
}
