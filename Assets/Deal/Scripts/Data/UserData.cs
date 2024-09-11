using System;
using System.Collections.Generic;
using Deal.Data;
using Deal.Env;
using Deal.Msg;
using Druid;
using Druid.Utils;
using ExcelData;
using UnityEngine;

namespace Deal
{
    /// <summary>
    /// 大厅能力枚举
    /// </summary>
    public enum HallAbilityEnum
    {
        SawmillLevel,  // 锯木厂
        WorkerBagLevel, // 工人背包
        StorageLevel, // 储物仓
        WorkerAbilityLevel, // 工人能力
        BagLevel, // 玩家背包容量
        AxeLevel, // 斧子
        PickAxeLevel, // 搞头
        StoneMineLevel, // 采石场
        FishingRobLevel, // 鱼竿
        WoodFactoryLevel, // 木板厂
        StoneFactoryLevel, // 制砖厂
        BreadFactoryLevel, // 面包厂的生产力
        IronFactoryLevel, // 炼铁厂的生产力
        FishingHutLevel, // 钓鱼小屋的生产力
        IronMineLevel, // 铁矿场的生产力
        CactusFactoryLevel, // 仙人掌药水工厂的生产力
        ConeFactoryLevel, // 球果工厂的生产力
        SickleLevel, // 镰刀
        DeadWoodFactoryLevel, // 枯木工厂的生产力
        CookingPotLevel, // 烹饪锅的生产力
        WeaverFactoryLevel, //纺织工厂的生产力
    }

    public enum WorkshopToolEnum
    {
        None = 0, //
        Axe = 1, //斧头
        Pickaxe = 2, //镐头
        Shovel = 3, //铲子
        FishingRod = 4, //鱼竿
        Sickle = 5, //镰刀
        AttackWeapon = 6, // 所有攻击武器
        //Sword = 7, //剑
    }

    public enum BluePrintEnum
    {
        None = 0, //
        Museum = 1, //博物馆
        FarmerHouse = 2, //农屋（羊圈）
        Lighthouse = 3, //灯塔
        Ship = 4, //图书馆
        Library = 5, //船
        GemTower = 6, //宝石塔
        Academy = 7, //酒馆
        ChickenFarm = 8, //养鸡场
    }

    /// <summary>
    /// 雕塑
    /// </summary>
    public enum StatueEnum
    {
        None = 0,
        Colossus = 1,
        GoldApple = 2,
        Miner = 3,
        MusicalKey = 4,
        Excalibur = 5,
        Totem = 6,
        Dinosaur = 7,
        Elemental = 8,
        Time = 9,
        Sacred = 10,
        Lumber = 11,
        Heart = 12,
        Scarecrow = 13,
        Triumphal = 14
    }

    // 任务类型
    public enum TaskTypeEnum
    {
        collect, //采集
        build, //建造
        sell, //在市场上出售
        land, //购买新的土地
        upgrade, //升级
        tool, //解锁搞头
        action, //挖出宝藏
        equip, //
        complete, //完成地下城
        research, //购买蓝图
        buy,
        tower, //宝石塔
        library, //宝石矿场
        assign, //安排工人
        kill, //击杀25个敌人
    }

    // 任务类型
    public enum TaskActionEnum
    {
        Treasure, //宝藏
    }

    /// <summary>
    /// 资产变化委托
    /// </summary>
    public delegate void DelegateAssetChange(AssetEnum assetEnum, int assetNum);
    public delegate void DelegateAbilityChange(HallAbilityEnum abilityEnum, int num);
    public delegate void DelegateLandExpChange(int lv, int exp);
    public delegate void DelegateSpecialOfferChange();
    public delegate void DelegateVipChange();
    public delegate void DelegateShareChange();

    public class UserData : DataTable
    {
        public string SaveFile = "Mary_UserData";

        // 存档
        public UserDataSlot Data;

        // 竞技场排名数据
        public Msg_ArenaRankInfo rankInfo;

        // 
        public bool openShopServer = false;
        public bool openIdentity = false;


        public UserData(string tableName) : base(tableName)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Load()
        {
            UserDataSlot userSlot = SaveLoadManager.Load<UserDataSlot>(SaveFile) as UserDataSlot;
            Debug.Log("UserData" + userSlot);
            //Debug.Log(userSlot);
            if (userSlot == null)
            {
                Debug.Log("UserData null");

                UserDataSlot dataSlot = new UserDataSlot();
                dataSlot.DefaultData();
                this.Data = dataSlot;
            }
            else
            {
                Debug.Log("UserData not null");
                this.Data = userSlot;
            }

        }

        public override void Save()
        {
            Debug.Log("Save UserData");
            this.Data.SaveSecond = TimeUtils.TimeNowSeconds();
            SaveLoadManager.Save(this.Data, SaveFile);
            this.SetAssetUnDitry();
        }

        #region AutoAssetSave
        private bool _assetDitry = false;
        private float _assetDitryInterval = 0;

        public void UpdateAssetSave()
        {
            if (this._assetDitry == true)
            {
                this._assetDitryInterval += Time.deltaTime;
                this.AutoAssetSave();
            }
        }

        private void SetAssetDitry()
        {

            _assetDitry = true;
            _assetDitryInterval = 0;
        }

        private void SetAssetUnDitry()
        {

            _assetDitry = false;
            _assetDitryInterval = 0;
        }

        private void AutoAssetSave()
        {

            this.Save();
            _assetDitry = false;
            _assetDitryInterval = 0;
        }

        #endregion AutoAssetSave

        // 资产变化
        public DelegateAssetChange OnAssetChange;
        public DelegateAbilityChange OnAbilityChange;
        public DelegateLandExpChange OnLandExpChange;
        public DelegateSpecialOfferChange OnSpecialOfferChange;
        public DelegateVipChange OnVipChange;
        public DelegateVipChange OnShareChange;
        public DelegateVipChange OnUserInfoChange;
        public DelegateVipChange OnMapRiderChange;
        public DelegateVipChange OnGoldAxeChange;
        public DelegateVipChange OnGoldPickaxeChange;


        public int BagToatl()
        {
            int total = 0;
            foreach (var item in this.Data.Assets)
            {
                if (DealUtils.isAssetInBagTotal(item.Key))
                {
                    total += item.Value;
                }
            }

            return total;
        }

        public int BagSpaceRemain()
        {
            return this.Data.Data_Hero.BagTotal - this.BagToatl();
        }

        public bool IsBagFull()
        {
            int total = 0;
            foreach (var item in this.Data.Assets)
            {
                total += item.Value;
            }

            if (total >= this.Data.Data_Hero.BagTotal)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 增加资产，单一
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="change"></param>
        public bool AddAsset(AssetEnum assetEnum, int changeNum)
        {
            if (changeNum == 0) return true;
            if (assetEnum == AssetEnum.Exp) return true;
            if (assetEnum == AssetEnum.DungeonExp)
            {
                // 玩家经验
                DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
                dungeonData.AddHeroExp(changeNum);
                return true;
            }

            if (assetEnum == AssetEnum.Bag)
            {
                // 玩家经验
                this.AddBagBase(changeNum);
                return true;
            }

            // 背包增加key
            if (!this.Data.Assets.ContainsKey(assetEnum))
            {
                this.Data.Assets.Add(assetEnum, 0);
            }
            // 背包数量
            int cBagNum = this.Data.Assets[assetEnum];


            if (changeNum > 0)
            {
                // 判断背包容量是不是够
                if (DealUtils.isAssetInBagTotal(assetEnum))
                {
                    if (this.BagToatl() + changeNum > this.Data.Data_Hero.BagTotal)
                    {
                        return false;
                    }
                }

                //更新任务
                TaskManager.I.OnTaskCollect(assetEnum, changeNum);

                if (DealUtils.isAssetInBagTotal(assetEnum))
                {
                    ActivityUtils.DoDailyTask(DailyTaskTypeEnum.collect, changeNum);
                }


                if (assetEnum == AssetEnum.Plank || assetEnum == AssetEnum.Bread
                    || assetEnum == AssetEnum.Wool || assetEnum == AssetEnum.Potion
                    || assetEnum == AssetEnum.BambooTissue || assetEnum == AssetEnum.SakuraPlank
                    || assetEnum == AssetEnum.DeadWoodPlank || assetEnum == AssetEnum.FishSoup)
                {
                    ActivityUtils.DoDailyTask(DailyTaskTypeEnum.collectFactory, changeNum);
                }
            }
            else if (changeNum < 0)
            {
                if (cBagNum + changeNum < 0)
                {
                    //不够
                    return false;
                }
            }

            cBagNum += changeNum;
            this.Data.Assets[assetEnum] = cBagNum;

            //调用委托事件
            if (this.OnAssetChange != null)
            {
                this.OnAssetChange(assetEnum, cBagNum);
            }
            this.SetAssetDitry();

            return true;
        }

        public bool AddAssetForce(AssetEnum assetEnum, int changeNum)
        {
            if (assetEnum == AssetEnum.Exp) return true;
            if (assetEnum == AssetEnum.GoldPick) return true;
            if (assetEnum == AssetEnum.GoldAxe) return true;
            if (assetEnum == AssetEnum.DungeonExp)
            {
                // 玩家经验
                DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
                dungeonData.AddHeroExp(changeNum);
                return true;
            }

            if (!this.Data.Assets.ContainsKey(assetEnum))
            {
                this.Data.Assets.Add(assetEnum, 0);
            }


            int assetNum = this.Data.Assets[assetEnum];
            if (changeNum < 0 && assetNum + changeNum < 0)
            {
                return false;
            }
            assetNum += changeNum;
            if (assetNum < 0) assetNum = 0;

            this.Data.Assets[assetEnum] = assetNum;

            if (changeNum > 0)
            {
                //更新任务
                TaskManager.I.OnTaskCollect(assetEnum, changeNum);

                if (DealUtils.isAssetInBagTotal(assetEnum))
                {
                    ActivityUtils.DoDailyTask(DailyTaskTypeEnum.collect, changeNum);
                }
                if (assetEnum == AssetEnum.Plank || assetEnum == AssetEnum.Bread
                    || assetEnum == AssetEnum.Wool || assetEnum == AssetEnum.Potion
                    || assetEnum == AssetEnum.BambooTissue || assetEnum == AssetEnum.SakuraPlank
                    || assetEnum == AssetEnum.DeadWoodPlank || assetEnum == AssetEnum.FishSoup)
                {
                    ActivityUtils.DoDailyTask(DailyTaskTypeEnum.collectFactory, changeNum);
                }
            }

            //调用委托事件
            if (this.OnAssetChange != null)
            {
                this.OnAssetChange(assetEnum, assetNum);
            }
            this.SetAssetDitry();
            return true;
        }

        /// <summary>
        /// 花钱
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <param name="changeNum"></param>
        public bool CostAsset(AssetEnum assetEnum, int changeNum)
        {
            //if (!this.Data.Assets.ContainsKey(assetEnum))
            //{
            //    return false;
            //}

            int assetNum = this.GetAssetNum(assetEnum);

            if (assetNum < changeNum)
            {
                if (assetEnum == AssetEnum.Gem)
                {
                    ShopUtils.pushGemGuide();
                }
                return false;
            }


            assetNum -= changeNum;
            this.Data.Assets[assetEnum] = assetNum;

            //调用委托事件
            if (this.OnAssetChange != null)
            {
                this.OnAssetChange(assetEnum, assetNum);
            }

            this.SetAssetDitry();
            return true;
        }

        public int GetAssetNum(AssetEnum assetEnum)
        {
            if (!this.Data.Assets.ContainsKey(assetEnum))
            {
                this.Data.Assets.Add(assetEnum, 0);
            }

            return this.Data.Assets[assetEnum];
        }


        // 大厅能力等级
        public int GetHallAbilityLv(HallAbilityEnum abilityEnum)
        {
            return this.Data.GetHallAbilityLv(abilityEnum);
        }

        // 升级能力等级
        public void UpHallAbilityLv(HallAbilityEnum abilityEnum)
        {
            this.Data.UpHallAbilityLv(abilityEnum);

            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);
            List<Data_BuildingBase> buildings = mapData.Data.buildings;

            float val = GetHallAbilityVal(abilityEnum);
            // 更新相关数据的能力
            if (abilityEnum == HallAbilityEnum.SawmillLevel)
            {
                // 锯木厂
                Building_Sawmill _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.Sawmill) as Building_Sawmill;

                Data_Sawmill _Data = _Storage.GetData<Data_Sawmill>();
                _Data.AssetTotal = (int)val;
                _Storage.UpdateView();
            }
            else if (abilityEnum == HallAbilityEnum.StoneMineLevel)
            {
                // 采石场
                Building_StoneMine _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.Stonemine) as Building_StoneMine;

                Data_StoneMine _Data = _Storage.GetData<Data_StoneMine>();
                _Data.AssetTotal = (int)val;
                _Storage.UpdateView();
            }
            else if (abilityEnum == HallAbilityEnum.StorageLevel)
            {
                // 仓库

                Building_Storage _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.Storage) as Building_Storage;

                Data_Storage _Data = _Storage.GetData<Data_Storage>();
                _Data.AssetTotal = (int)val;
                _Storage.UpdateView();

            }
            else if (abilityEnum == HallAbilityEnum.WoodFactoryLevel)
            {
                // 木板厂

                Building_WoodFactory _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.WoodFactory) as Building_WoodFactory;
                if (_Storage != null)
                {
                    Data_WoodFactory _Data = _Storage.GetData<Data_WoodFactory>();
                    _Data.FromTotal = (int)val;
                    _Data.ToTotal = (int)val;
                    _Storage.UpdateView();
                }
            }
            else if (abilityEnum == HallAbilityEnum.StoneFactoryLevel)
            {
                // 砖厂
                Building_StoneFactory _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.StoneFactory) as Building_StoneFactory;

                Data_StoneFactory _Data = _Storage.GetData<Data_StoneFactory>();
                _Data.FromTotal = (int)val;
                _Data.ToTotal = (int)val;
                _Storage.UpdateView();

            }
            else if (abilityEnum == HallAbilityEnum.AxeLevel)
            {
                Data_Hero hero = this.Data.Data_Hero;
                hero.AxeVal = (int)val;
            }
            else if (abilityEnum == HallAbilityEnum.PickAxeLevel)
            {
                Data_Hero hero = this.Data.Data_Hero;
                hero.PickAxeVal = (int)val;
            }
            else if (abilityEnum == HallAbilityEnum.FishingRobLevel)
            {
                Data_Hero hero = this.Data.Data_Hero;
                hero.FishRodVal = (int)val;
            }
            else if (abilityEnum == HallAbilityEnum.WorkerAbilityLevel)
            {
                Data_Hero hero = this.Data.Data_Hero;
                hero.WorkerAbilityVal = (int)val;
            }
            else if (abilityEnum == HallAbilityEnum.SickleLevel)
            {
                // 镰刀
                Data_Hero hero = this.Data.Data_Hero;
                hero.SickleVal = (int)val;
            }
            else if (abilityEnum == HallAbilityEnum.BagLevel)
            {
                Data_Hero hero = this.Data.Data_Hero;
                hero.SetBagHall((int)val);
            }
            else if (abilityEnum == HallAbilityEnum.WorkerBagLevel)
            {
                for (int i = 0; i < mapData.Data.workers.Count; i++)
                {
                    Data_Worker _Worker = mapData.Data.workers[i];
                    _Worker.BagTotal = (int)val;
                }
            }
            else if (abilityEnum == HallAbilityEnum.BreadFactoryLevel)
            {

                Building_BreadFactory _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.BreadFactory) as Building_BreadFactory;

                Data_BreadFactory _Data = _Storage.GetData<Data_BreadFactory>();
                _Data.FromTotal = (int)val;
                _Data.ToTotal = (int)val;
                _Storage.UpdateView();

            }
            else if (abilityEnum == HallAbilityEnum.IronFactoryLevel)
            {
                Building_IronFactory _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.IronFactory) as Building_IronFactory;

                Data_IronFactory _Data = _Storage.GetData<Data_IronFactory>();
                _Data.FromTotal = (int)val;
                _Data.ToTotal = (int)val;
                _Storage.UpdateView();

                //for (int i = 0; i < buildings.Count; i++)
                //{
                //    if (buildings[i].BuildingEnum == BuildingEnum.IronFactory)
                //    {
                //        Data_IronFactory _Data = buildings[i] as Data_IronFactory;
                //        _Data.FromTotal = (int)val;
                //        _Data.ToTotal = (int)val;
                //    }
                //}
            }
            else if (abilityEnum == HallAbilityEnum.FishingHutLevel)
            {
                for (int i = 0; i < buildings.Count; i++)
                {
                    if (buildings[i].BuildingEnum == BuildingEnum.FishingHut)
                    {
                        Data_FishingHut _StoneMine = buildings[i] as Data_FishingHut;
                        _StoneMine.AssetTotal = (int)val;
                    }
                }
            }
            else if (abilityEnum == HallAbilityEnum.CactusFactoryLevel)
            {
                Building_CactusFactory _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.CactusFactory) as Building_CactusFactory;

                Data_CactusFactory _Data = _Storage.GetData<Data_CactusFactory>();
                _Data.FromTotal = (int)val;
                _Data.ToTotal = (int)val;
                _Storage.UpdateView();
            }
            else if (abilityEnum == HallAbilityEnum.ConeFactoryLevel)
            {
                Building_ConeFactory _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.ConeFactory) as Building_ConeFactory;

                Data_ConeFactory _Data = _Storage.GetData<Data_ConeFactory>();
                _Data.FromTotal = (int)val;
                _Data.ToTotal = (int)val;
                _Storage.UpdateView();
            }
            else if (abilityEnum == HallAbilityEnum.DeadWoodFactoryLevel)
            {
                Building_DeadWoodFactory _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.DeadWoodFactory) as Building_DeadWoodFactory;

                Data_DeadWoodFactory _Data = _Storage.GetData<Data_DeadWoodFactory>();
                _Data.FromTotal = (int)val;
                _Data.ToTotal = (int)val;
                _Storage.UpdateView();
            }
            else if (abilityEnum == HallAbilityEnum.WeaverFactoryLevel)
            {
                Building_WeaverFactory _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.WeaverFactory) as Building_WeaverFactory;

                Data_WeaverFactory _Data = _Storage.GetData<Data_WeaverFactory>();
                _Data.FromTotal = (int)val;
                _Data.ToTotal = (int)val;
                _Storage.UpdateView();
            }
            else if (abilityEnum == HallAbilityEnum.CookingPotLevel)
            {
                Building_CookingPot _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.CookingPot) as Building_CookingPot;

                Data_CookingPot _Data = _Storage.GetData<Data_CookingPot>();
                _Data.FromTotal = (int)val;
                _Data.ToTotal = (int)val;
                _Storage.UpdateView();
            }
            else if (abilityEnum == HallAbilityEnum.IronMineLevel)
            {
                for (int i = 0; i < buildings.Count; i++)
                {
                    if (buildings[i].BuildingEnum == BuildingEnum.IronMine)
                    {
                        Data_IronMine _StoneMine = buildings[i] as Data_IronMine;
                        _StoneMine.AssetTotal = (int)val;
                    }
                }
            }

            if (OnAbilityChange != null)
            {
                OnAbilityChange(abilityEnum, (int)val);
            }
        }

        public float GetHallAbilityVal(HallAbilityEnum abilityEnum)
        {
            int cLv = this.GetHallAbilityLv(abilityEnum);
            HallUpgrade hallUpgrade = ConfigManger.I.GetHallAbilityCfg(abilityEnum);

            //float cVal = hallUpgrade.baseVal + hallUpgrade.upgrade * cLv;

            float cVal = MathUtils.GetHallabilityValue(abilityEnum, cLv, hallUpgrade.baseVal, hallUpgrade.upgrade);

            return cVal;
        }

        // 获得工具
        public bool HasWorkshopTool(WorkshopToolEnum toolEnum)
        {
            return this.Data.HasWorkshopTool(toolEnum);
        }

        public void UnlockWorkshopTool(WorkshopToolEnum toolEnum)
        {
            this.Data.UnlockWorkshopTool(toolEnum);
        }

        /// <summary>
        /// 是否可以收集资源
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <returns></returns>
        public bool CanCollectRes(AssetEnum assetEnum)
        {
            WorkshopToolEnum workshopTool = this.GetCollectResTool(assetEnum);

            if (workshopTool != WorkshopToolEnum.None)
            {
                return this.HasWorkshopTool(workshopTool);
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// 是否可以收集资源
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <returns></returns>
        public int GetCollectRes(AssetEnum assetEnum)
        {
            bool can = this.CanCollectRes(assetEnum);
            if (can == false)
            {
                return 0;
            }
            WorkshopToolEnum tool = GetCollectResTool(assetEnum);

            if (tool == WorkshopToolEnum.Axe)
            {
                return (int)this.Data.Data_Hero.AxeVal;
            }
            else if (tool == WorkshopToolEnum.Pickaxe)
            {
                return (int)this.Data.Data_Hero.PickAxeVal;
            }
            else if (tool == WorkshopToolEnum.FishingRod)
            {
                return (int)this.Data.Data_Hero.FishRodVal;
            }
            else if (tool == WorkshopToolEnum.Shovel)
            {
                return 1;
            }
            else if (tool == WorkshopToolEnum.None)
            {
                return (int)this.Data.Data_Hero.WorkerAbilityVal;
            }
            else if (tool == WorkshopToolEnum.Sickle)
            {
                return (int)this.Data.Data_Hero.SickleVal;
            }

            return 0;
        }

        /// <summary>
        /// 是否可以收集资源
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <returns></returns>
        public WorkshopToolEnum GetCollectResTool(AssetEnum assetEnum)
        {
            if (assetEnum == AssetEnum.Wood)
            {
                return WorkshopToolEnum.Axe;
            }
            else if (assetEnum == AssetEnum.Stone)
            {
                return WorkshopToolEnum.Pickaxe;
            }
            else if (assetEnum == AssetEnum.Treasure)
            {
                return WorkshopToolEnum.Shovel;
            }
            else if (assetEnum == AssetEnum.Grain)
            {
                return WorkshopToolEnum.Sickle;
            }
            else if (assetEnum == AssetEnum.Iron)
            {
                return WorkshopToolEnum.Pickaxe;
            }
            else if (assetEnum == AssetEnum.Cactus)
            {
                return WorkshopToolEnum.Axe;
            }
            else if (assetEnum == AssetEnum.DeadWood)
            {
                return WorkshopToolEnum.Axe;
            }
            else if (assetEnum == AssetEnum.WinterWood)
            {
                return WorkshopToolEnum.Axe;
            }
            else if (assetEnum == AssetEnum.Bamboo)
            {
                return WorkshopToolEnum.Axe;
            }
            else if (assetEnum == AssetEnum.Fish)
            {
                return WorkshopToolEnum.FishingRod;
            }

            return WorkshopToolEnum.None;
        }

        /// <summary>
        /// 背包基础容量增加
        /// </summary>
        /// <param name="bagAdd"></param>
        public void AddBagBase(int bagAdd)
        {
            Data_Hero hero = this.Data.Data_Hero;
            hero.AddBagBase(bagAdd);
            if (OnAbilityChange != null)
            {
                OnAbilityChange(HallAbilityEnum.BagLevel, hero.BagTotal);
            }
            //保存
            this.Save();
        }

        /// <summary>
        /// 小岛等级
        /// </summary>
        /// <param name="exp"></param>
        public void AddLandExp(int exp)
        {
            this.Data.LandExp += exp;
            if (this.Data.LandExp >= 100)
            {
                this.Data.LandLv++;
                this.Data.LandExp -= 100;
            }

            if (this.OnLandExpChange != null)
            {
                this.OnLandExpChange(this.Data.LandLv, this.Data.LandExp);
            }

            Hero hero = PlayManager.I.mHero;
            //for (int i = 0; i < exp; i++)
            //{
            DealUtils.newDropItem(AssetEnum.Exp, exp, hero.transform.position);
            //}

            //保存
            this.Save();
        }

        /// <summary>
        /// 是否有蓝图
        /// </summary>
        /// <param name="bluePrint"></param>
        /// <returns></returns>
        public bool HasBulePrint(BluePrintEnum bluePrint)
        {
            if (this.Data.BluePrint == null)
            {
                this.Data.BluePrint = new Dictionary<BluePrintEnum, int>();
            }

            if (this.Data.BluePrint.ContainsKey(bluePrint))
            {
                return this.Data.BluePrint[bluePrint] > 0;
            }

            return false;
        }

        public void UnlockBulePrint(BluePrintEnum bluePrint)
        {
            if (!this.Data.BluePrint.ContainsKey(bluePrint))
            {
                this.Data.BluePrint.Add(bluePrint, 1);
            }
            else
            {
                this.Data.BluePrint[bluePrint] = 1;
            }

        }

        /// <summary>
        /// 雕塑图纸
        /// </summary>
        /// <param name="bluePrint"></param>
        /// <returns></returns>
        public bool HasStatueBulePrint(StatueEnum bluePrint)
        {
            if (this.Data.StatueBluePrint == null)
            {
                this.Data.StatueBluePrint = new Dictionary<StatueEnum, int>();
            }

            if (this.Data.StatueBluePrint.ContainsKey(bluePrint))
            {
                return this.Data.StatueBluePrint[bluePrint] > 0;
            }

            return false;
        }

        public void UnlockStatueBulePrint(StatueEnum bluePrint)
        {
            if (!this.Data.StatueBluePrint.ContainsKey(bluePrint))
            {
                this.Data.StatueBluePrint.Add(bluePrint, 1);
            }
            else
            {
                this.Data.StatueBluePrint[bluePrint] = 1;
            }
        }

        /// <summary>
        /// 雕塑等级
        /// </summary>
        /// <param name="bluePrint"></param>
        /// <returns></returns>
        public int GetStatueLv(StatueEnum bluePrint)
        {
            if (this.Data.StatueLv == null)
            {
                this.Data.StatueLv = new Dictionary<StatueEnum, int>();
            }

            if (!this.Data.StatueLv.ContainsKey(bluePrint))
            {
                this.Data.StatueLv.Add(bluePrint, 100);
            }

            int lv = this.Data.StatueLv[bluePrint];

            return lv;
        }

        public void UpgradeStatueLv(StatueEnum bluePrint, int lv, int sep)
        {
            if (this.Data.StatueLv == null)
            {
                this.Data.StatueLv = new Dictionary<StatueEnum, int>();
            }

            if (!this.Data.StatueLv.ContainsKey(bluePrint))
            {
                this.Data.StatueLv.Add(bluePrint, 100);
            }

            this.Data.StatueLv[bluePrint] = lv * 100 + sep;

            if (bluePrint == StatueEnum.Miner)
            {
                //储物仓容量
                Building_Storage _Storage = MapManager.I.GetSingleBuilding(BuildingEnum.Storage) as Building_Storage;

                Data_Storage data_Storage = _Storage.GetData<Data_Storage>();
                data_Storage.DataBuff();
                _Storage.UpdateView();

            }
            else if (bluePrint == StatueEnum.Lumber)
            {
                //背包容量
                this.Data.Data_Hero.AddBagBase(0);
                if (OnAbilityChange != null)
                {
                    OnAbilityChange(HallAbilityEnum.BagLevel, this.Data.Data_Hero.BagTotal);
                }
            }

            else if (bluePrint == StatueEnum.Elemental || bluePrint == StatueEnum.Time)
            {
                //工厂容量增加

                MapRender mapRender = MapManager.I.mapRender;

                foreach (var building in mapRender.buildings)
                {
                    BuildingChangeFactory changeFactory = building.Value.gameObject.GetComponent<BuildingChangeFactory>();

                    if (changeFactory != null)
                    {
                        DataChangeFactory data_ChangeFactory = changeFactory.GetData<DataChangeFactory>();
                        data_ChangeFactory.DataBuff();
                        changeFactory.UpdateView();
                    }
                }
            }
            else
            {
                Hero hero = PlayManager.I.mHero;
                hero.RestBattleBaseAtt();
            }

            this.Save();
        }

        /// <summary>
        /// 解锁vip
        /// </summary>
        /// <param name="day"></param>
        public void UnlockVip(int day)
        {
            long nowSecond = TimeUtils.TimeNowSeconds();
            long endSecond = nowSecond + day * 24 * 60 * 60;

            this.Data.IsVip = true;
            this.Data.VipEndSecond = endSecond;

            // 更新一下背包
            this.AddBagBase(0);

            if (this.OnVipChange != null)
            {
                this.OnVipChange();
            }

            Hero hero = PlayManager.I.mHero;
            hero.SetRider(this.Data.IsVip || this.Data.IsInMapRider);

            this.Save();

            ShopUtils.vipFreeTicket();
        }

        /// <summary>
        /// vip结束
        /// </summary>
        public void FinishVip()
        {

            this.Data.IsVip = false;
            this.Data.VipEndSecond = 0;

            if (this.OnVipChange != null)
            {
                this.OnVipChange();
            }

            Hero hero = PlayManager.I.mHero;
            hero.SetRider(this.Data.IsVip || this.Data.IsInMapRider);

            this.Save();
        }

        /// <summary>
        /// 限时礼包
        /// </summary>
        /// <param name="day"></param>
        public void NewSpecialOffer(Data_SpecialOffer offer)
        {
            if (this.Data.SpecialOffers == null)
            {
                this.Data.SpecialOffers = new List<Data_SpecialOffer>();
            }

            this.Data.SpecialOffers.Add(offer);

            if (this.OnSpecialOfferChange != null)
            {
                this.OnSpecialOfferChange();
            }

            this.Save();
        }

        /// <summary>
        /// 删除一个礼包
        /// </summary>
        /// <param name="offer"></param>
        public void DelSpecialOffer(Data_SpecialOffer offer)
        {
            if (this.Data.SpecialOffers == null)
            {
                return;
            }
            this.Data.SpecialOffers.Remove(offer);

            if (this.OnSpecialOfferChange != null)
            {
                this.OnSpecialOfferChange();
            }

            this.Save();
        }

        /// <summary>
        /// 删除一个礼包
        /// </summary>
        /// <param name="offer"></param>
        public void DelSpecialOffer(int id)
        {
            if (this.Data.SpecialOffers == null)
            {
                return;
            }

            for (int i = 0; i < this.Data.SpecialOffers.Count; i++)
            {
                if (this.Data.SpecialOffers[i].Id == id)
                {
                    this.Data.SpecialOffers.RemoveAt(i);
                    break;
                }
            }

            if (this.OnSpecialOfferChange != null)
            {
                this.OnSpecialOfferChange();
            }

            this.Save();
        }

        public void HasSpecialOffer(int offer)
        {
            if (this.Data.HasSpecialOffers == null)
            {
                this.Data.HasSpecialOffers = new List<int>();
            }

            this.Data.HasSpecialOffers.Add(offer);

        }


        /// <summary>
        /// 保存新订单
        /// </summary>
        /// <param name="offer"></param>
        public void NewOrderList(Msg_Data_OrderList offer)
        {
            if (this.Data.ClientOrderList == null)
            {
                this.Data.ClientOrderList = new List<Msg_Data_OrderList>();
            }

            // 删除订单号一样的订单
            for (int i = this.Data.ClientOrderList.Count - 1; i >= 0; i--)
            {
                var item = this.Data.ClientOrderList[i];
                if (item.orderno == offer.orderno)
                {
                    this.Data.ClientOrderList.Remove(item);
                }
            }

            this.Data.ClientOrderList.Add(offer);
            this.Save();
        }

        public void AddShareTimes()
        {
            this.Data.TodayShareTimes++;

            if (this.OnShareChange != null)
            {
                this.OnShareChange();
            }

            this.Save();
        }

        /// <summary>
        /// 解锁坐骑
        /// </summary>
        /// <param name="day"></param>
        public void UnlockMapRider(int seconds)
        {
            if (seconds == -1)
            {
                this.Data.IsInMapRider = true;
                this.Data.MapRiderEndSecond = -1;
            }
            else
            {
                if (this.Data.MapRiderEndSecond == -1)
                {
                    // 无限制了
                }
                else
                {
                    if (this.Data.IsInMapRider == true)
                    {
                        this.Data.MapRiderEndSecond += seconds;
                    }
                    else
                    {
                        this.Data.IsInMapRider = true;

                        long nowSecond = TimeUtils.TimeNowSeconds();
                        long endSecond = nowSecond + seconds;
                        this.Data.MapRiderEndSecond = endSecond;
                    }
                }
            }


            Hero hero = PlayManager.I.mHero;
            hero.SetRider(this.Data.IsVip || this.Data.IsInMapRider);

            if (this.OnMapRiderChange != null)
            {
                this.OnMapRiderChange();
            }
            this.Save();
        }

        /// <summary>
        /// 坐骑时间到
        /// </summary>
        /// <param name="day"></param>
        public void RemoveMapRider()
        {
            if (this.Data.IsInMapRider == true)
            {
                this.Data.IsInMapRider = false;

                if (this.OnMapRiderChange != null)
                {
                    this.OnMapRiderChange();
                }
                this.Save();
            }

            Hero hero = PlayManager.I.mHero;
            hero.SetRider(this.Data.IsVip || this.Data.IsInMapRider);
        }

        /// <summary>
        /// 解锁金斧头
        /// </summary>
        /// <param name="day"></param>
        public void UnlockGlodAxe(int seconds)
        {
            if (this.Data.IsGoldAxe == true)
            {
                this.Data.GoldAxeEndSecond += seconds;
            }
            else
            {
                this.Data.IsGoldAxe = true;

                long nowSecond = TimeUtils.TimeNowSeconds();
                long endSecond = nowSecond + seconds;
                this.Data.GoldAxeEndSecond = endSecond;
            }

            Hero hero = PlayManager.I.mHero;
            hero.SetGoldAxe(this.Data.IsGoldAxe);

            if (this.OnGoldAxeChange != null)
            {
                this.OnGoldAxeChange();
            }
            this.Save();
        }

        /// <summary>
        /// 金斧头时间到
        /// </summary>
        /// <param name="day"></param>
        public void RemoveGlodAxe()
        {
            if (this.Data.IsGoldAxe == true)
            {
                this.Data.IsGoldAxe = false;

                if (this.OnGoldAxeChange != null)
                {
                    this.OnGoldAxeChange();
                }
                this.Save();
            }

            Hero hero = PlayManager.I.mHero;
            hero.SetGoldAxe(this.Data.IsGoldAxe);
        }

        /// <summary>
        /// 解锁金搞头
        /// </summary>
        /// <param name="day"></param>
        public void UnlockGlodPickAxe(int seconds)
        {
            if (this.Data.IsGoldPickaxe == true)
            {
                this.Data.GoldPickaxeEndSecond += seconds;
            }
            else
            {
                this.Data.IsGoldPickaxe = true;

                long nowSecond = TimeUtils.TimeNowSeconds();
                long endSecond = nowSecond + seconds;
                this.Data.GoldPickaxeEndSecond = endSecond;
            }

            Hero hero = PlayManager.I.mHero;
            hero.SetGoldPickaxe(this.Data.IsGoldPickaxe);

            if (this.OnGoldPickaxeChange != null)
            {
                this.OnGoldPickaxeChange();
            }
            this.Save();
        }

        /// <summary>
        /// 金斧头时间到
        /// </summary>
        /// <param name="day"></param>
        public void RemoveGlodPickaxe()
        {
            if (this.Data.IsGoldPickaxe == true)
            {
                this.Data.IsGoldPickaxe = false;

                if (this.OnGoldPickaxeChange != null)
                {
                    this.OnGoldPickaxeChange();
                }
                this.Save();
            }

            Hero hero = PlayManager.I.mHero;
            hero.SetGoldPickaxe(this.Data.IsGoldPickaxe);
        }
    }

    /// <summary>
    /// 存档数据
    /// </summary>
    [Serializable]
    public class UserDataSlot
    {
        // 保存时间
        public double SaveSecond = 0;

        //资产列表
        [SerializeField]
        public Dictionary<AssetEnum, int> Assets = new Dictionary<AssetEnum, int>();

        [SerializeField]
        // 大厅能力等级
        public Dictionary<HallAbilityEnum, int> HallAbilityLv = new Dictionary<HallAbilityEnum, int>();

        [SerializeField]
        // 工具
        public Dictionary<WorkshopToolEnum, int> WorkshopTool = new Dictionary<WorkshopToolEnum, int>();

        [SerializeField]
        // 蓝图
        public Dictionary<BluePrintEnum, int> BluePrint = new Dictionary<BluePrintEnum, int>();

        [SerializeField]
        // 雕塑图纸
        public Dictionary<StatueEnum, int> StatueBluePrint = new Dictionary<StatueEnum, int>();

        [SerializeField]
        // 雕塑等级
        public Dictionary<StatueEnum, int> StatueLv = new Dictionary<StatueEnum, int>();

        [SerializeField]
        // 已经购买的限时礼包
        public List<int> HasSpecialOffers = new List<int>();

        [SerializeField]
        // 限时礼包
        public List<Data_SpecialOffer> SpecialOffers = new List<Data_SpecialOffer>();

        // 客户端的订单立列表
        public List<Msg_Data_OrderList> ClientOrderList = new List<Msg_Data_OrderList>();
        // 七日签到
        public List<Data_SevenDay> SevenSign = new List<Data_SevenDay>();

        public List<Data_DailyTask> DailyTasks = new List<Data_DailyTask>();


        // 当前任务
        public Data_Task Data_Task = null;

        public Data_Hero Data_Hero = null;

        public int LandLv = 1;
        public int LandExp = 0;
        public int LandExpMax = 100;

        // 用户信息
        public Data_Userinfo Userinfo = null;

        // 登录时间戳
        public double LoginSecond = 0;

        // vip信息
        public bool IsVip = false;
        public double VipEndSecond = 0;

        // 坐骑信息
        public bool IsInMapRider = false;
        public double MapRiderEndSecond = 0;

        // 金斧子
        public bool IsGoldAxe = false;
        public double GoldAxeEndSecond = 0;

        // 金搞头
        public bool IsGoldPickaxe = false;
        public double GoldPickaxeEndSecond = 0;


        // 今日古代碎片的数量
        public int TodayAncientShard = 0;

        // 每日次数购买
        public int TodayArenaTicketBuy = 0;

        public int TodayShareTimes = 0;

        // 改名次数
        public int ChangeNameTimes = 1;

        /// <summary>
        /// 默认账号数据
        /// </summary>
        public void DefaultData()
        {
            this.WorkshopTool.Add(WorkshopToolEnum.Axe, 1);
            this.Data_Task = DataUtils.NewTask(1);
            //this.Data_Hero = DataUtils.NewHero();

            this.Assets.Add(AssetEnum.ArenaTicket, 10);
            this.ChangeNameTimes = 1;
        }


        // 大厅能力等级
        public int GetHallAbilityLv(HallAbilityEnum abilityEnum)
        {
            if (!HallAbilityLv.ContainsKey(abilityEnum))
            {
                HallAbilityLv.Add(abilityEnum, 0);
            }

            return HallAbilityLv[abilityEnum];
        }

        // 升级能力等级
        public void UpHallAbilityLv(HallAbilityEnum abilityEnum)
        {
            if (!HallAbilityLv.ContainsKey(abilityEnum))
            {
                HallAbilityLv.Add(abilityEnum, 0);
            }

            HallAbilityLv[abilityEnum] = HallAbilityLv[abilityEnum] + 1;
        }


        public bool HasWorkshopTool(WorkshopToolEnum toolEnum)
        {
            if (WorkshopTool.ContainsKey(toolEnum))
            {
                return WorkshopTool[toolEnum] > 0;
            }

            return false;
        }

        // 获得工具
        public void UnlockWorkshopTool(WorkshopToolEnum toolEnum)
        {
            if (!WorkshopTool.ContainsKey(toolEnum))
            {
                WorkshopTool.Add(toolEnum, 1);
                TaskManager.I.OnTaskTool(toolEnum);
            }
        }

    }
}

