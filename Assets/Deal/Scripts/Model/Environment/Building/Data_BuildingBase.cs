using System;
using System.Collections.Generic;
using Druid;
using Druid.Utils;
using UnityEngine;

namespace Deal.Data
{
    /// <summary>
	/// 建筑枚举
	/// </summary>
    public enum BuildingEnum
    {
        None,
        Hall, //大厅
        Market, //市场
        Sawmill, //锯木厂
        Storage, //储物仓
        Workshop,  //工坊
        Stonemine, //采石场
        Armory, //武器库
        IronMine, //铁矿场
        FishingHut, //钓鱼小屋
        WoodFactory, //木板厂
        StoneFactory, //制砖厂
        IronFactory, //炼铁厂
        BreadFactory, //面包工厂
        House, //房屋（增加1个工人）
        Fountain, // 喷泉
        Pier, //码头
        Farm, //农场 
        AppleTree, // 苹果树
        ResearchLab, //实验室
        Portal, //传送门
        Museum, //博物馆
        SheepFactory, // 羊毛厂
        FarmerHouse, // 羊圈
        PumpkinLand, // 南瓜地(南瓜地，先不用了 用Farm)
        Bridge, // 桥
        Treasure, // 宝藏
        Teleport, // 传送点
        DiamondMine, // 钻石矿
        Ship, // 船
        Lighthouse, //灯塔
        Home, // 自家
        HomeStore, //自家商店
        Flagstaff, //旗杆
        Library, //图书馆
        GemTower, //宝石塔
        Wagon, //马车
        CactusFactory, //仙人掌 药水工厂
        Bag, //地上的背包升级
        Armillary, //浑天仪
        CarrotPatch, //胡萝卜田
        ConeFactory, // 球果工厂
        RubyMine, //红宝石矿场
        JewelryShop, //珠宝店
        Academy, //学院
        SeashellStore, //贝壳商店
        DeadWoodFactory, // 枯木工厂
        CookingPot, //烹饪锅
        TmpPortal, //临时传送门
        Statue, // 雕塑
        ChickenFarm, //养鸡场
        ChickenRoost, // 鸡舍（竖）
        EmeraldMine, // 绿宝石矿场
        AmethystMine, // 紫宝石矿场
        OrangeTree, // 橘子树
        WeaverFactory, //纺织工厂
        Harbour, //港口
        CottonFarm, //莲花农场
        BridgeTD, // 横向的桥
        ChickenRoostTD, // 鸡舍（横）
        SakuraFactory, //樱树工厂
        SpaceShip, //飞船

    }

    /// <summary>
	/// 建筑状态枚举
	/// </summary>
    public enum BuildingStateEnum
    {
        None,
        Building,  // 建造中
        Open       // 开放
    }

    /// <summary>
    /// 建筑的基础类
    /// </summary>
    [Serializable]
    public class Data_BuildingBase : Data_SaveBase
    {
        // 坐标
        public Data_Point Pos;
        // 大小
        public Data_Point Size;
        // 资源类型
        public BuildingEnum BuildingEnum = BuildingEnum.Hall;

        // 解锁任务ID
        public int UnlockTask = -1;
        // 状态
        public BuildingStateEnum StateEnum = BuildingStateEnum.None;
        // 价格
        public List<Data_GameAsset> Price = new List<Data_GameAsset>();
        // 蓝图图纸
        public BluePrintEnum BluePrint = BluePrintEnum.None;
        // 雕塑
        public StatueEnum StatueEnum = StatueEnum.None;

        // 刷新时间
        public long CommonCDAt = 0;
        // 秒
        public int CommonRefreshNeed = 0;

        public bool InCd = false;

        //图纸
        public int BluePrintPrice = 1;
        //图纸
        public int StatuePrice = 1;

        public virtual long UniqueId()
        {
            Data_Point center = this.CenterGrid();
            return center.y * 10000 + center.x;
        }

        public virtual void Open()
        {
            this.StateEnum = BuildingStateEnum.Open;
        }

        public virtual List<Data_GameAsset> GetPrice()
        {
            return this.Price;
        }

        public void OpenSmoke()
        {
            MapRender mapRender = MapManager.I.mapRender;
            PrefabsUtils.NewUnlockSmoke(mapRender.Builds.transform, this.WorldPos);

            //PrefabsUtils.NewUnlockSmoke();
        }

        public virtual void Building()
        {
            if (this.StateEnum != BuildingStateEnum.Building)
            {
                this.StateEnum = BuildingStateEnum.Building;

                // 雕塑减资源
                float buffVal = MathUtils.GetStatueBuff(StatueEnum.Totem);

                foreach (Data_GameAsset item in Price)
                {
                    int newVal = (int)(item.assetNum * (1 - buffVal));
                    item.assetNum = newVal;
                }
            }

        }


        /// <summary>
        /// 数据加成
        /// </summary>
        public virtual void DataBuff()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Data_Point CenterGrid()
        {
            int x = 0;
            int y = 0;
            if (this.Pos.x % 2 == 0)
            {
                x = this.Pos.x / 2;
            }
            else
            {
                x = (this.Pos.x - 1) / 2;
            }

            if (this.Pos.y % 2 == 0)
            {
                y = this.Pos.y / 2;
            }
            else
            {
                y = (this.Pos.y - 1) / 2;
            }


            return new Data_Point(x, y);
        }

        public Data_Point StartGrid()
        {
            Data_Point center = this.CenterGrid();

            int x = center.x - this.Size.x / 2;
            int y = center.y - this.Size.y / 2;

            return new Data_Point(x, y);
        }


        public Vector3 WorldPos { get => new Vector3(Pos.x / 2f, Pos.y / 2f, 0); }

        /// <summary>
        /// 
        /// </summary>
        public bool RefreshCommonState()
        {
            if (this.InCd)
            {
                if (TimeUtils.TimeNowMilliseconds() - this.CommonCDAt > this.CommonRefreshNeed * 1000)
                {
                    this.InCd = false;
                    return true;
                }
            }

            return false;

        }

        public virtual bool InCommonCd()
        {
            return this.InCd;
        }

        public float GetCdProgress()
        {
            float passed = TimeUtils.TimeNowMilliseconds() - this.CommonCDAt;
            float need = this.CommonRefreshNeed * 1000;

            return passed / need;
        }
    }
}

