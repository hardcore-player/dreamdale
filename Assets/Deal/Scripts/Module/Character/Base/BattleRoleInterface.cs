using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using Deal.Env;
using UnityEngine;

namespace Deal
{
    /// <summary>
    /// 角色的基础
    /// </summary>
    public interface BattleRoleInterface
    {


        void AddBattleWeapon(Data_Equip weaponId);

        /// <summary>
        /// 获得伤害
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        RoleDamageData GetAttackDamage(BattleRoleBase role);

        /// <summary>
        /// 获得伤害
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        RoleDamageData GetAttackDamage(BattleRoleAtt roleAtt);

        void OnFightEnter(BattleRoleBase role);
        void OnFightStart(BattleRoleBase role);
        void OnFightEnd(BattleRoleBase role);
        void OnFightExit(BattleRoleBase role);

        /// <summary>
        /// 被攻击
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <returns></returns>
        void OnDamage(BattleRoleBase role);

        void BefroeDamage(BattleRoleBase role);
        void AfterDamage(BattleRoleBase role);

        void BefroeAttack(BattleRoleBase role);
        void OnAttack(BattleRoleBase role);
        void AfterAttack(BattleRoleBase role);

        void Look2Role(BattleRoleBase role);

        /// <summary>
        /// 死亡
        /// </summary>
        void OnDie();

        bool IsDie();




    }
}
