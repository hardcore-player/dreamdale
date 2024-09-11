using System.Collections;
using System.Collections.Generic;
using Druid;
using UnityEngine;
using Deal.Tools;
using Deal.Data;

namespace Deal
{
    public class HeroAnimation : MonoBehaviour
    {
        public SpriteRenderer srWeapon;
        public SpriteRenderer srHead;


        [Header("皮肤ID")]
        public int skinId = 5;

        [Header("身体动画")]
        public Animator bodyAnimator;

        public bool InUI = false;

        void Start()
        {
            //this.weaponParent.gameObject.SetActive(true);

            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            dungeonData.OnEquipChange += OnEquipChange;
            this.AddEquip(dungeonData.GetEquip(EquipPointEnum.weapon), EquipPointEnum.weapon);
            this.AddEquip(dungeonData.GetEquip(EquipPointEnum.head), EquipPointEnum.head);

            if (this.InUI)
            {
                Druid.Utils.UnityUtils.ReSortRendererInUI(this.gameObject);

                bodyAnimator.speed = 1;
                bodyAnimator.Play($"ani_idle{this.skinId}", 0);
            }
        }

        private void OnDestroy()
        {
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            if (dungeonData != null)
            {
                dungeonData.OnEquipChange -= OnEquipChange;
            }
        }

        /// <summary>
        /// 添加攻击武器
        /// </summary>
        /// <param name="weaponId"></param>
        public void AddEquip(Data_Equip weapon, EquipPointEnum point)
        {
            if (weapon != null)
            {
                ExcelData.Equip equip = ConfigManger.I.GetEquipCfg(weapon.equipId);

                if (point == EquipPointEnum.weapon)
                {
                    SpriteUtils.SetRoleEquipIcon(this.srWeapon, weapon.equipId);
                }
                else if (point == EquipPointEnum.head)
                {
                    SpriteUtils.SetRoleEquipIcon(this.srHead, weapon.equipId);
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

        public void OnEquipChange(EquipPointEnum point, Data_Equip equip)
        {
            if (point == EquipPointEnum.weapon || point == EquipPointEnum.head)
            {
                this.AddEquip(equip, point);

                //Druid.Utils.UnityUtils.ReSortRendererInUI(this.gameObject);
            }
        }
    }

}

