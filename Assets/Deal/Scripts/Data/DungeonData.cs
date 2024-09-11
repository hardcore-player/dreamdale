using System;
using System.Collections.Generic;
using Deal.Data;
using Druid;
using Druid.Utils;
using ExcelData;
using UnityEngine;

namespace Deal
{
    //武器装备位置
    public enum EquipPointEnum
    {
        weapon, // 武器
        head,  // 头
        chest, //胸甲
        cloak, // 披风
        shield,  //盾牌
        rune, //附文
    }

    // 属性字段
    public enum EquipAttrEnum
    {
        hp, // 
        attack,  // 头
        crit, //
        dodge, // 
        hit,  //
        decrit, //
        hreg, //
    }

    public delegate void DelegateEquipChange(EquipPointEnum point, Data_Equip equip);
    public delegate void DelegateEquipLvup(EquipPointEnum point, Data_Equip equip);
    public delegate void DelegateHeroLvChange(int lv, int exp, bool lvup);


    /// <summary>
    /// 地下城数据保存
    /// </summary>
    public class DungeonData : DataTable
    {
        public string SaveFile = "Mary_DungeonData";

        // 存档
        public DungeonDataSlot Data;


        public DelegateEquipChange OnEquipChange;
        public DelegateEquipChange OnEquipLvup;
        public DelegateHeroLvChange OnHeroLvChange;

        public DungeonData(string tableName) : base(tableName)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Load()
        {
            DungeonDataSlot userSlot = SaveLoadManager.Load<DungeonDataSlot>(SaveFile) as DungeonDataSlot;
            if (userSlot == null)
            {
                Debug.Log("DungeonDataSlot null");

                DungeonDataSlot dataSlot = new DungeonDataSlot();
                dataSlot.DefaultData();
                this.Data = dataSlot;
            }
            else
            {
                Debug.Log("DungeonDataSlot not null");
                this.Data = userSlot;
            }

        }

        public override void Save()
        {
            Debug.Log("Save DungeonDataSlot");
            this.Data.SaveSecond = TimeUtils.TimeNowSeconds();
            SaveLoadManager.Save(this.Data, SaveFile);
        }

        /// <summary>
        /// 获取装备
        /// </summary>
        /// <param name="point"></param>
        /// <param name="equip"></param>
        public void GetEquipToBag(Data_Equip equip)
        {
            if (this.Data.EquipList.Contains(equip)) return;

            this.Data.EquipList.Add(equip);

            if (GetEquip(equip.point) == null)
            {
                // 直接装备
                Equip(equip.point, equip);
            }

            this.Save();
        }

        /// <summary>
        /// 装备
        /// </summary>
        public void Equip(EquipPointEnum point, Data_Equip equip)
        {
            if (this.Data.EquipPoints.ContainsKey(point))
            {
                this.Data.EquipPoints[point] = equip;
            }
            else
            {
                this.Data.EquipPoints.Add(point, equip);
            }

            this.OnEquipChange(point, equip);

            this.Save();
        }

        /// <summary>
        /// 装备升级
        /// </summary>
        /// <param name="equip"></param>
        public void EquipLvup(Data_Equip equip)
        {
            equip.equipLv++;
            if (GetEquip(equip.point) == equip)
            {
                this.OnEquipLvup(equip.point, equip);
            }

            this.Save();
        }

        public void EquipLvRecover(Data_Equip equip)
        {
            equip.equipLv = 1;
            if (GetEquip(equip.point) == equip)
            {
                this.OnEquipLvup(equip.point, equip);
            }

            this.Save();
        }

        public Data_Equip GetEquip(EquipPointEnum point)
        {
            if (this.Data.EquipPoints.ContainsKey(point))
            {
                return this.Data.EquipPoints[point];
            }
            else
            {
                return null;
            }

        }


        /// <summary>
        /// 角色等级
        /// </summary>
        /// <param name="exp"></param>
        public void AddHeroExp(int exp)
        {
            this.Data.HeroExp += exp;
            bool lvup = false;
            if (this.Data.HeroExp >= this.Data.HeroExpMax)
            {
                this.Data.HeroLv++;
                this.Data.HeroExp -= this.Data.HeroExpMax;

                this.Data.HeroExpMax = 100 + this.Data.HeroLv * 20;

                lvup = true;
            }

            this.OnHeroLvChange(this.Data.HeroLv, this.Data.HeroExp, lvup);

            //保存
            this.Save();
        }

    }

    /// <summary>
    /// 关卡
    /// </summary>
    [Serializable]
    public class Data_DungeonStage
    {

        // 复活次数
        public int ResurrectionTimes = 0;
        // 关卡数据
        public double DungeonBuilding = 0;

        public DataDungeonLevel DataDungeonLevel = null;

    }

    /// <summary>
    /// 存档数据
    /// </summary>
    [Serializable]
    public class DungeonDataSlot
    {
        // 保存时间
        public double SaveSecond = 0;

        public int HeroLv = 1;
        public int HeroExp = 0;
        public int HeroExpMax = 0;

        // 所有的装备列表
        public List<Data_Equip> EquipList = new List<Data_Equip>();
        // 身上的装备
        public Dictionary<EquipPointEnum, Data_Equip> EquipPoints = new Dictionary<EquipPointEnum, Data_Equip>();

        // 地牢信息
        [SerializeField]
        public Data_DungeonStage DataStage;


        public void DefaultData()
        {
            this.DataStage = new Data_DungeonStage();

            this.HeroLv = 1;
            this.HeroExp = 0;
            this.HeroExpMax = 100 + this.HeroLv * 20;

            //this.DataStage.DungeonLevel = 1;
            //this.DataStage.ResurrectionTimes = 1;
            //for (int i = 0; i < 5; i++)
            //{
            //    this.Data_Stage.Rooms.Add(i, 0);
            //}

            ////TODO
            //Equip[] equip = ConfigManger.I.configS.equips;

            //for (int i = 0; i < equip.Length; i++)
            //{
            //    this.EquipList.Add(new Data_Equip(equip[i].id, 1, 1));
            //}
        }
    }
}

