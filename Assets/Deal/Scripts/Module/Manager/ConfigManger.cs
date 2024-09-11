using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using ExcelData;
using System;

namespace Deal
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public class ConfigManger : PersistentSingleton<ConfigManger>
    {
        // 配置表
        public ConfigMgrSObj configS;
        public SO_MapData mapData;


        // 工具配置
        public Workshop GetWorkshopCfg(string name)
        {
            return Array.Find(this.configS.workshops, v => v.name == name);
        }

        // 大厅能力配置
        public HallUpgrade GetHallAbilityCfg(HallAbilityEnum abilityEnum)
        {
            int abilityId = (int)abilityEnum;
            if (this.configS.hallUpgrades.Length <= abilityId)
            {
                return null;
            }

            HallUpgrade hallUpgrade = this.configS.hallUpgrades[(int)abilityEnum];

            return hallUpgrade;
        }

        public Task GetTaskCfg(int taskId)
        {
            if (this.configS.tasks.Length <= taskId)
            {
                return null;
            }

            Task task = this.configS.tasks[taskId];

            return task;
        }

        public Resource GetResourceCfg(string name)
        {
            return Array.Find(this.configS.resources, v => v.name == name);
        }

        public Market GetMarketCfg(string name)
        {
            return Array.Find(this.configS.markets, v => v.name == name);
        }

        public ResBuilding GetResBuildingCfg(string name)
        {
            return Array.Find(this.configS.resBuildings, v => v.name == name);
        }

        public ExcelData.Statue GetStatueCfg(string name)
        {
            return Array.Find(this.configS.statues, v => v.name == name);
        }

        public ExcelData.Dungeon GetDungeonCfg(int lv)
        {
            return this.configS.dungeons[lv];
        }

        public ExcelData.Monster GetMonsterCfg(int id)
        {
            if (id < this.configS.monsters.Length)
            {
                return this.configS.monsters[id];
            }

            return null;
        }

        public ExcelData.Research GetResearchCfg(int id)
        {
            return Array.Find(this.configS.researchs, v => v.id == id);
        }

        public ExcelData.Diamond GetDiamondsCfg(string name)
        {
            return Array.Find(this.configS.diamonds, v => v.name == name);
        }

        // 限时礼包
        public ExcelData.SpecialOffer GetSpecialOfferCfg(int id)
        {
            return Array.Find(this.configS.specialOffers, v => v.id == id);
        }

        public ExcelData.Shop GetShopCfg(int id)
        {
            return Array.Find(this.configS.shops, v => v.id == id);
        }



        /// <summary>
        /// 装备
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Equip GetEquipCfg(int id)
        {
            return Array.Find(this.configS.equips, v => v.id == id);
        }

    }
}

