using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using System.Threading.Tasks;
using DG.Tweening;
using Deal.Data;

namespace Deal
{
    /// <summary>
    /// 角色的战斗脚本
    /// </summary>
    public class CharacterBattle : MonoBehaviour
    {
        private Hero _hero;
        public Hero Hero { get => _hero; set => _hero = value; }


        // 每秒回血
        private float _recoverInterval = 0f;

        private void Awake()
        {
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            dungeonData.OnEquipChange += OnEquipChange;
            dungeonData.OnEquipLvup += OnEquipLvup;

            this.Hero.AddBattleWeapon(dungeonData.GetEquip(EquipPointEnum.weapon));
        }

        private void OnDestroy()
        {
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            if (dungeonData != null)
            {
                dungeonData.OnEquipChange -= OnEquipChange;
            }
        }

        public void OnEquipChange(EquipPointEnum point, Data_Equip equip)
        {
            if (point == EquipPointEnum.weapon)
            {
                this.Hero.AddBattleWeapon(equip);
            }

            this.Hero.RestBattleBaseAtt();
        }

        public void OnEquipLvup(EquipPointEnum point, Data_Equip equip)
        {
            this.Hero.RestBattleBaseAtt();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == TagDefine.Enemy)
            {
                // 遇到敌人
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                bool fight = BattleController.I.OnRoleEnter(this.Hero, enemy);

                if (fight) this.Hero.Controller.SetStatePause();
            }

        }

    }
}