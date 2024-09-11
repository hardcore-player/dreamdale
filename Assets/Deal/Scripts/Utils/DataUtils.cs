using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Data;
using Druid;
using Druid.Utils;
using ExcelData;
using Deal.Env;

namespace Deal
{

    public class DataUtils
    {
        public static Data_Hero NewHero()
        {
            Data_Hero hero = new Data_Hero();

            return hero;
        }

        public static Data_Task NewTask(int taskId)
        {
            //taskId = 153;
            taskId = taskId - 1;
            ExcelData.Task task = ConfigManger.I.GetTaskCfg(taskId);

            // 没开发就下一个
            while (task != null && task.enable == 0)
            {
                taskId++;
                task = ConfigManger.I.GetTaskCfg(taskId);
            }

            if (task == null)
            {
                return null;
            }

            Data_Task data_Task = new Data_Task();
            data_Task.TaskId = task.id;
            data_Task.CurProgress = 0;
            data_Task.IsDone = false;
            data_Task.HasReward = true;

            data_Task.Auto = task.auto;
            data_Task.Reward = task.reward;
            data_Task.TaskInfo = task.content;
            data_Task.TaskType = task.type;
            data_Task.TotalProgress = task.num;
            data_Task.TargetType = task.target;

            if (task.type == TaskTypeEnum.upgrade.ToString())
            {
                // 大厅升级
                HallAbilityEnum abilityEnum = (HallAbilityEnum)Enum.Parse(typeof(HallAbilityEnum), task.target);

                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                int aLv = userData.GetHallAbilityLv(abilityEnum);
                data_Task.CurProgress = aLv + 1;



            }
            else if (task.type == TaskTypeEnum.buy.ToString())
            {

                if (task.target == "Sheep")
                {
                    //买羊的任务
                    MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

                    List<Data_BuildingBase> buildings = mapData.Data.buildings;

                    for (int i = 0; i < buildings.Count; i++)
                    {
                        if (buildings[i].BuildingEnum == BuildingEnum.FarmerHouse)
                        {
                            Data_FarmerHouse _FarmerHouse = buildings[i] as Data_FarmerHouse;
                            data_Task.CurProgress = _FarmerHouse.SheepNum;

                            break;
                        }
                    }
                }
                else if (task.target == "Chicken")
                {
                    //买鸡的任务
                    MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

                    List<Data_BuildingBase> buildings = mapData.Data.buildings;

                    for (int i = 0; i < buildings.Count; i++)
                    {
                        if (buildings[i].BuildingEnum == BuildingEnum.ChickenFarm)
                        {
                            Data_ChickenFarm _FarmerHouse = buildings[i] as Data_ChickenFarm;
                            data_Task.CurProgress = _FarmerHouse.ChickenNum;

                            break;
                        }
                    }
                }

            }
            else if (task.type == TaskTypeEnum.library.ToString())
            {
                // 升级宝石塔
                Building_Library library = MapManager.I.GetSingleBuilding(BuildingEnum.Library) as Building_Library;
                if (library != null)
                {
                    Data_Library data_ = library.GetData<Data_Library>();

                    Data_LibraryAssetsLv _LibraryAssetsLv = data_.GetLibraryAssetsLv(AssetEnum.Sapphire);

                    if (_LibraryAssetsLv.capacityLv > 1 || _LibraryAssetsLv.speedLv > 1 || _LibraryAssetsLv.prductLv > 1)
                    {
                        data_Task.CurProgress = data_Task.TotalProgress;
                    }

                }
            }

            if (data_Task.CurProgress >= data_Task.TotalProgress)
            {
                data_Task.IsDone = true;
            }

            return data_Task;
        }


        /// <summary>
        /// 默认一个树
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_Worker NewDataWorker()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);


            Data_Worker data = new Data_Worker();

            data.AssetId = AssetEnum.Wood;
            data.AssetNum = 0;
            data.BagTotal = (int)userData.GetHallAbilityVal(HallAbilityEnum.WorkerBagLevel);
            data.AbilityVal = (int)userData.GetHallAbilityVal(HallAbilityEnum.WorkerAbilityLevel);

            return data;
        }

        /// <summary>
        /// 默认一个树
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataTree(int x, int y)
        {
            ExcelData.Resource cfg = ConfigManger.I.GetResourceCfg(NameDefine.Wood);
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.Wood;
            data.State = CollectableResState.DONE;
            data.CollectType = CollectableRes_CollectType.Idle;
            data.AssetTotal = cfg.collect;
            data.AssetLeft = cfg.collect;
            data.RefreshNeed = cfg.restore;

            return data;
        }

        /// <summary>
        /// 默认一个仙人掌
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataCactus(int x, int y)
        {
            ExcelData.Resource cfg = ConfigManger.I.GetResourceCfg(NameDefine.Cactus);
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.Cactus;
            data.State = CollectableResState.DONE;
            data.CollectType = CollectableRes_CollectType.Idle;
            data.AssetTotal = cfg.collect;
            data.AssetLeft = cfg.collect;
            data.RefreshNeed = cfg.restore;

            return data;
        }

        /// <summary>
        /// 默认一个松树
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataWinterWood(int x, int y)
        {
            ExcelData.Resource cfg = ConfigManger.I.GetResourceCfg(NameDefine.WinterWood);
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.WinterWood;
            data.State = CollectableResState.DONE;
            data.CollectType = CollectableRes_CollectType.Idle;
            data.AssetTotal = cfg.collect;
            data.AssetLeft = cfg.collect;
            data.RefreshNeed = cfg.restore;

            return data;
        }

        /// <summary>
        /// 默认一个ku树
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataDeadWood(int x, int y)
        {
            ExcelData.Resource cfg = ConfigManger.I.GetResourceCfg(NameDefine.DeadWood);
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.DeadWood;
            data.State = CollectableResState.DONE;
            data.CollectType = CollectableRes_CollectType.Idle;
            data.AssetTotal = cfg.collect;
            data.AssetLeft = cfg.collect;
            data.RefreshNeed = cfg.restore;

            return data;
        }

        /// <summary>
        /// 默认一个zhuzi
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataBamboo(int x, int y)
        {
            ExcelData.Resource cfg = ConfigManger.I.GetResourceCfg(NameDefine.Bamboo);
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.Bamboo;
            data.State = CollectableResState.DONE;
            data.CollectType = CollectableRes_CollectType.Idle;
            data.AssetTotal = cfg.collect;
            data.AssetLeft = cfg.collect;
            data.RefreshNeed = cfg.restore;

            return data;
        }

        /// <summary>
        /// 默认一个石头
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataStone(int x, int y)
        {
            ExcelData.Resource cfg = ConfigManger.I.GetResourceCfg(NameDefine.Stone);
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.Stone;
            data.State = CollectableResState.DONE;
            data.CollectType = CollectableRes_CollectType.Idle;
            data.AssetTotal = cfg.collect;
            data.AssetLeft = cfg.collect;
            data.RefreshNeed = cfg.restore;

            return data;
        }


        /// <summary>
        /// 默认一个五谷
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataGrain(int x, int y)
        {
            ExcelData.Resource cfg = ConfigManger.I.GetResourceCfg(NameDefine.Grain);
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.Grain;
            data.State = CollectableResState.DONE;
            data.CollectType = CollectableRes_CollectType.Idle;
            data.AssetTotal = cfg.collect;
            data.AssetLeft = cfg.collect;
            data.RefreshNeed = cfg.restore;

            return data;
        }

        /// <summary>
        /// 默认一个钢铁
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataIron(int x, int y)
        {
            ExcelData.Resource cfg = ConfigManger.I.GetResourceCfg(AssetEnum.Iron.ToString());
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.Iron;
            data.State = CollectableResState.DONE;
            data.CollectType = CollectableRes_CollectType.Idle;
            data.AssetTotal = cfg.collect;
            data.AssetLeft = cfg.collect;
            data.RefreshNeed = cfg.restore;

            return data;
        }

        /// <summary>
        /// 默认一个宝藏
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataTreasure(int x, int y)
        {
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.Treasure;
            data.State = CollectableResState.DONE;
            data.CollectType = CollectableRes_CollectType.Idle;
            data.AssetTotal = 1;
            data.AssetLeft = 1;
            data.RefreshNeed = 180;

            return data;
        }

        /// <summary>
        /// 默认一个苹果树
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataAppleTree(int x, int y)
        {
            ExcelData.ResBuilding cfg = ConfigManger.I.GetResBuildingCfg(NameDefine.AppleTree);
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.Apple;
            data.State = CollectableResState.CD;
            data.CollectType = CollectableRes_CollectType.Idle;
            data.AssetTotal = 3;
            data.AssetLeft = 0;
            data.RefreshNeed = cfg.speed;
            data.CDAt = TimeUtils.TimeNowMilliseconds() - (cfg.speed - 10) * 1000;

            return data;
        }

        /// <summary>
        /// 默认一个橘子树
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataOrangeTree(int x, int y)
        {
            ExcelData.ResBuilding cfg = ConfigManger.I.GetResBuildingCfg(NameDefine.OrangeTree);
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.Orange;
            data.State = CollectableResState.CD;
            data.CollectType = CollectableRes_CollectType.Idle;
            data.AssetTotal = 3;
            data.AssetLeft = 0;
            data.RefreshNeed = cfg.speed;
            data.CDAt = TimeUtils.TimeNowMilliseconds() - (cfg.speed - 10) * 1000;

            return data;
        }

        /// <summary>
        /// 默认一个南瓜
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataPumpkin(int x, int y)
        {
            ExcelData.ResBuilding cfg = ConfigManger.I.GetResBuildingCfg(NameDefine.Farm);
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.Pumpkin;
            data.State = CollectableResState.CD;
            data.CollectType = CollectableRes_CollectType.Touch;
            data.AssetTotal = 1;
            data.AssetLeft = 0;
            data.RefreshNeed = cfg.speed;
            data.CDAt = TimeUtils.TimeNowMilliseconds() - (cfg.speed - 10) * 1000;

            return data;
        }

        /// <summary>
        /// 默认一个胡萝卜
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataCarrot(int x, int y)
        {
            ExcelData.ResBuilding cfg = ConfigManger.I.GetResBuildingCfg(NameDefine.CarrotPatch);
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.Carrot;
            data.State = CollectableResState.CD;
            data.CollectType = CollectableRes_CollectType.Touch;
            data.AssetTotal = 1;
            data.AssetLeft = 0;
            data.RefreshNeed = cfg.speed;
            data.CDAt = TimeUtils.TimeNowMilliseconds() - (cfg.speed - 10) * 1000;


            return data;
        }

        /// <summary>
        /// 默认一个棉花
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Data_CollectableRes NewDataCotton(int x, int y)
        {
            ExcelData.ResBuilding cfg = ConfigManger.I.GetResBuildingCfg(NameDefine.CottonFarm);
            Data_CollectableRes data = new Data_CollectableRes();
            data.Pos = new Data_Point(x, y);
            data.AssetId = AssetEnum.Cotton;
            data.State = CollectableResState.CD;
            data.CollectType = CollectableRes_CollectType.Touch;
            data.AssetTotal = 1;
            data.AssetLeft = 0;
            data.RefreshNeed = cfg.speed;
            data.CDAt = TimeUtils.TimeNowMilliseconds() - (cfg.speed - 10) * 1000;


            return data;
        }

        /// <summary>
        /// 新建建筑数据
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="buildingEnum"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static Data_BuildingBase NewDataBuilding(int x, int y, int w, int h, BuildingEnum buildingEnum, List<Data_GameAsset> price, BluePrintEnum bluePrintEnum, StatueEnum statueEnum, int unlockTask)
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            if (buildingEnum == BuildingEnum.PumpkinLand || buildingEnum == BuildingEnum.Farm)
            {
                // 南瓜地
                Data_PumpkinLand data = new Data_PumpkinLand();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;

                return data;
            }
            if (buildingEnum == BuildingEnum.CarrotPatch)
            {
                // 胡萝卜地
                Data_CarrotPatch data = new Data_CarrotPatch();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;

                return data;
            }
            if (buildingEnum == BuildingEnum.CottonFarm)
            {
                // 棉花地
                Data_CottonFarm data = new Data_CottonFarm();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;

                return data;
            }
            if (buildingEnum == BuildingEnum.AppleTree)
            {
                //
                Data_AppleTree data = new Data_AppleTree();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;

                return data;
            }
            if (buildingEnum == BuildingEnum.OrangeTree)
            {
                //
                Data_OrangeTree data = new Data_OrangeTree();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;

                return data;
            }
            else if (buildingEnum == BuildingEnum.Bridge)
            {
                // 桥
                Data_Bridge data = new Data_Bridge();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;

                return data;
            }
            else if (buildingEnum == BuildingEnum.BridgeTD)
            {
                // 桥
                Data_BridgeTD data = new Data_BridgeTD();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;

                return data;
            }
            else if (buildingEnum == BuildingEnum.Storage)
            {
                // 储物仓库
                Data_Storage data = new Data_Storage();
                //data.Assets.Add(AssetEnum.Gold, 100);

                data.AssetTotal = (int)userData.GetHallAbilityVal(HallAbilityEnum.StorageLevel);

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.CDAt = TimeUtils.TimeNowMilliseconds();
                return data;
            }
            else if (buildingEnum == BuildingEnum.Sawmill)
            {
                // 锯木厂
                Data_Sawmill data = new Data_Sawmill();
                data.AssetId = AssetEnum.Wood;
                data.Assets.Add(AssetEnum.Wood, 0);
                data.AssetTotal = (int)userData.GetHallAbilityVal(HallAbilityEnum.SawmillLevel);
                data.RefreshNeed = 5;
                data.ProductNum = 1;
                data.CDAt = TimeUtils.TimeNowMilliseconds();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                return data;
            }
            else if (buildingEnum == BuildingEnum.Stonemine)
            {
                // 采石厂
                Data_StoneMine data = new Data_StoneMine();
                data.Assets.Add(AssetEnum.Stone, 0);
                data.AssetId = AssetEnum.Stone;
                data.AssetTotal = (int)userData.GetHallAbilityVal(HallAbilityEnum.StoneMineLevel);
                data.RefreshNeed = 5;
                data.ProductNum = 1;
                data.CDAt = TimeUtils.TimeNowMilliseconds();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                return data;
            }
            else if (buildingEnum == BuildingEnum.IronMine)
            {
                // 采矿厂
                Data_IronMine data = new Data_IronMine();
                data.Assets.Add(AssetEnum.Iron, 0);
                data.AssetId = AssetEnum.Iron;
                data.AssetTotal = (int)userData.GetHallAbilityVal(HallAbilityEnum.IronMineLevel);
                data.RefreshNeed = 5;
                data.ProductNum = 1;
                data.CDAt = TimeUtils.TimeNowMilliseconds();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                return data;
            }
            else if (buildingEnum == BuildingEnum.Hall)
            {
                // 大厅
                Data_Hall data = new Data_Hall();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                return data;
            }
            else if (buildingEnum == BuildingEnum.Market)
            {
                // 市场
                Data_Market data = new Data_Market();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                return data;
            }
            else if (buildingEnum == BuildingEnum.Workshop)
            {
                // 工坊
                Data_Workshop data = new Data_Workshop();

                data.ToolEnum = WorkshopToolEnum.None;

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                return data;
            }
            else if (buildingEnum == BuildingEnum.Armory)
            {
                // 武器库
                Data_Armory data = new Data_Armory();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                return data;
            }
            else if (buildingEnum == BuildingEnum.House)
            {
                // 房屋
                Data_House data = new Data_House();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                return data;
            }

            else if (buildingEnum == BuildingEnum.Wagon)
            {
                // 马车
                Data_Wagon data = new Data_Wagon();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.Open;
                data.UnlockTask = unlockTask;

                Data_GameAsset _GameAsset = price[0];
                data.AssetId = _GameAsset.assetType;
                data.AssetTotal = _GameAsset.assetNum;
                data.RefreshNeed = 8 * 60 * 60;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                return data;
            }
            else if (buildingEnum == BuildingEnum.Portal)
            {
                // 传送门
                Data_Portal data = new Data_Portal();

                data.isTmp = false;
                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                return data;
            }
            else if (buildingEnum == BuildingEnum.TmpPortal)
            {
                // 传送门
                Data_TmpPortal data = new Data_TmpPortal();

                data.isTmp = true;
                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;

                return data;
            }
            else if (buildingEnum == BuildingEnum.Fountain)
            {
                // 喷泉
                Data_Fountain data = new Data_Fountain();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;

                return data;
            }
            else if (buildingEnum == BuildingEnum.WoodFactory)
            {
                // 木板厂
                Data_WoodFactory data = new Data_WoodFactory();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.WoodFactory.ToString());

                data.FromAsset = AssetEnum.Wood;
                data.ToAsset = AssetEnum.Plank;

                data.FromNum = 0;
                data.FromTotal = 10;

                data.ToNum = 0;
                data.ToTotal = 10;
                data.ChangeNeed = resBuilding.num;
                data.RefreshNeed = resBuilding.speed;
                data.NumBuff = 0;
                data.RefreshNeedBuff = 0;

                return data;
            }
            else if (buildingEnum == BuildingEnum.Pier)
            {
                // 码头
                Data_Pier data = new Data_Pier();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                return data;
            }
            else if (buildingEnum == BuildingEnum.ResearchLab)
            {
                // 实验室
                Data_ResearchLab data = new Data_ResearchLab();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                return data;
            }
            else if (buildingEnum == BuildingEnum.Museum)
            {
                // 博物馆
                Data_Museum data = new Data_Museum();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                return data;
            }
            else if (buildingEnum == BuildingEnum.StoneFactory)
            {
                // 制砖厂
                Data_StoneFactory data = new Data_StoneFactory();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.StoneFactory.ToString());

                data.FromAsset = AssetEnum.Stone;
                data.ToAsset = AssetEnum.Brick;

                data.FromNum = 0;
                data.FromTotal = 10;

                data.ToNum = 0;
                data.ToTotal = 10;
                data.ChangeNeed = resBuilding.num;
                data.RefreshNeed = resBuilding.speed;
                data.NumBuff = 0;
                data.RefreshNeedBuff = 0;

                return data;
            }
            else if (buildingEnum == BuildingEnum.SheepFactory)
            {
                // 羊毛厂
                Data_SheepFactory data = new Data_SheepFactory();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.SheepFactory.ToString());

                data.FromAsset = AssetEnum.Sheep;
                data.ToAsset = AssetEnum.Wool;

                data.FromNum = 0;
                data.FromTotal = 30;

                data.ToNum = 0;
                data.ToTotal = 30;
                data.ChangeNeed = resBuilding.num;
                data.RefreshNeed = resBuilding.speed;
                data.NumBuff = 0;
                data.RefreshNeedBuff = 0;

                return data;
            }
            else if (buildingEnum == BuildingEnum.FarmerHouse)
            {
                // 羊圈
                Data_FarmerHouse data = new Data_FarmerHouse();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                data.SheepNum = 0;
                data.SheepTotal = 12;

                return data;
            }
            else if (buildingEnum == BuildingEnum.BreadFactory)
            {
                // 面包厂
                Data_BreadFactory data = new Data_BreadFactory();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.BreadFactory.ToString());

                data.FromAsset = AssetEnum.Grain;
                data.ToAsset = AssetEnum.Bread;

                data.FromNum = 0;
                data.FromTotal = 10;

                data.ToNum = 0;
                data.ToTotal = 10;
                data.ChangeNeed = resBuilding.num;
                data.RefreshNeed = resBuilding.speed;
                data.NumBuff = 0;
                data.RefreshNeedBuff = 0;

                return data;
            }
            else if (buildingEnum == BuildingEnum.CactusFactory)
            {
                // 药水工厂
                Data_CactusFactory data = new Data_CactusFactory();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.CactusFactory.ToString());

                data.FromAsset = AssetEnum.Cactus;
                data.ToAsset = AssetEnum.Potion;

                data.FromNum = 0;
                data.FromTotal = 10;

                data.ToNum = 0;
                data.ToTotal = 10;
                data.ChangeNeed = resBuilding.num;
                data.RefreshNeed = resBuilding.speed;
                data.NumBuff = 0;
                data.RefreshNeedBuff = 0;

                return data;
            }
            else if (buildingEnum == BuildingEnum.ConeFactory)
            {
                // 松果工厂
                Data_ConeFactory data = new Data_ConeFactory();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.ConeFactory.ToString());

                data.FromAsset = AssetEnum.WinterWood;
                data.ToAsset = AssetEnum.Cone;

                data.FromNum = 0;
                data.FromTotal = 10;

                data.ToNum = 0;
                data.ToTotal = 10;
                data.ChangeNeed = resBuilding.num;
                data.RefreshNeed = resBuilding.speed;
                data.NumBuff = 0;
                data.RefreshNeedBuff = 0;

                return data;
            }
            else if (buildingEnum == BuildingEnum.CookingPot)
            {
                // 松果工厂
                Data_CookingPot data = new Data_CookingPot();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.CookingPot.ToString());

                data.FromAsset = AssetEnum.Fish;
                data.ToAsset = AssetEnum.FishSoup;

                data.FromNum = 0;
                data.FromTotal = 10;

                data.ToNum = 0;
                data.ToTotal = 10;
                data.ChangeNeed = resBuilding.num;
                data.RefreshNeed = resBuilding.speed;
                data.NumBuff = 0;
                data.RefreshNeedBuff = 0;

                return data;
            }
            else if (buildingEnum == BuildingEnum.DeadWoodFactory)
            {
                // 枯木工厂
                Data_DeadWoodFactory data = new Data_DeadWoodFactory();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.DeadWoodFactory.ToString());

                data.FromAsset = AssetEnum.DeadWood;
                data.ToAsset = AssetEnum.DeadWoodPlank;

                data.FromNum = 0;
                data.FromTotal = 10;

                data.ToNum = 0;
                data.ToTotal = 10;
                data.ChangeNeed = resBuilding.num;
                data.RefreshNeed = resBuilding.speed;
                data.NumBuff = 0;
                data.RefreshNeedBuff = 0;

                return data;
            }
            else if (buildingEnum == BuildingEnum.WeaverFactory)
            {
                // 松果工厂
                Data_WeaverFactory data = new Data_WeaverFactory();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.WeaverFactory.ToString());

                data.FromAsset = AssetEnum.Bamboo;
                data.ToAsset = AssetEnum.BambooTissue;

                data.FromNum = 0;
                data.FromTotal = 10;

                data.ToNum = 0;
                data.ToTotal = 10;
                data.ChangeNeed = resBuilding.num;
                data.RefreshNeed = resBuilding.speed;
                data.NumBuff = 0;
                data.RefreshNeedBuff = 0;

                return data;
            }
            else if (buildingEnum == BuildingEnum.FishingHut)
            {
                // 钓鱼小屋
                Data_FishingHut data = new Data_FishingHut();

                ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.FishingHut.ToString());


                data.AssetId = AssetEnum.Fish;
                data.Assets.Add(AssetEnum.Fish, 0);
                data.AssetTotal = 10;
                data.RefreshNeed = resBuilding.speed;
                data.ProductNum = 1;
                data.CDAt = TimeUtils.TimeNowMilliseconds();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                return data;
            }

            else if (buildingEnum == BuildingEnum.IronFactory)
            {
                // 炼铁厂
                Data_IronFactory data = new Data_IronFactory();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.IronFactory.ToString());

                data.FromAsset = AssetEnum.Iron;
                data.ToAsset = AssetEnum.Nail;

                data.FromNum = 0;
                data.FromTotal = 10;

                data.ToNum = 0;
                data.ToTotal = 10;
                data.ChangeNeed = resBuilding.num;
                data.RefreshNeed = resBuilding.speed;
                data.NumBuff = 0;
                data.RefreshNeedBuff = 0;

                return data;
            }
            else if (buildingEnum == BuildingEnum.Teleport)
            {
                // 传送点
                Data_Teleport data = new Data_Teleport();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                return data;
            }
            else if (buildingEnum == BuildingEnum.Lighthouse)
            {
                // 灯塔
                Data_Lighthouse data = new Data_Lighthouse();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                return data;
            }
            else if (buildingEnum == BuildingEnum.Academy)
            {
                // 灯塔
                Data_Academy data = new Data_Academy();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                return data;
            }
            else if (buildingEnum == BuildingEnum.SpaceShip)
            {
                // 太空飞船
                Data_SpaceShip data = new Data_SpaceShip();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                return data;
            }

            else if (buildingEnum == BuildingEnum.DiamondMine || buildingEnum == BuildingEnum.RubyMine
               || buildingEnum == BuildingEnum.EmeraldMine || buildingEnum == BuildingEnum.AmethystMine)
            {

                AssetEnum asset = AssetEnum.Sapphire;
                if (buildingEnum == BuildingEnum.DiamondMine)
                {
                    asset = AssetEnum.Sapphire;
                }
                else if (buildingEnum == BuildingEnum.RubyMine)
                {
                    asset = AssetEnum.Ruby;
                }
                else if (buildingEnum == BuildingEnum.EmeraldMine)
                {
                    asset = AssetEnum.Emerald;
                }
                else if (buildingEnum == BuildingEnum.AmethystMine)
                {
                    asset = AssetEnum.Amethyst;
                }

                Data_LibraryAssetsLv data_Library = getDimaondMineLv(asset);
                ExcelData.Diamond resBuilding = ConfigManger.I.GetDiamondsCfg(asset.ToString());

                // 蓝宝石矿场
                Data_DiamondMine data = new Data_DiamondMine();
                data.Assets.Add(asset, 0);
                data.AssetId = asset;
                data.UpdateNumWithLibraryLv(resBuilding, data_Library);

                data.CDAt = TimeUtils.TimeNowMilliseconds() - (data.RefreshNeed - 10) * 1000;
                //data.CDAt = TimeUtils.TimeNowMilliseconds();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                return data;
            }
            else if (buildingEnum == BuildingEnum.Library)
            {
                // 图书馆
                Data_Library data = new Data_Library();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.Price = price;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;

                data.LvList.Add(new Data_LibraryAssetsLv(AssetEnum.Sapphire));
                data.LvList.Add(new Data_LibraryAssetsLv(AssetEnum.Ruby));
                data.LvList.Add(new Data_LibraryAssetsLv(AssetEnum.Emerald));
                data.LvList.Add(new Data_LibraryAssetsLv(AssetEnum.Amethyst));

                return data;
            }
            else if (buildingEnum == BuildingEnum.GemTower)
            {
                // 宝石塔
                Data_GemTower data = new Data_GemTower();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                return data;
            }
            else if (buildingEnum == BuildingEnum.Statue)
            {
                // 雕塑
                Data_Statue data = new Data_Statue();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                return data;
            }
            else if (buildingEnum == BuildingEnum.Ship)
            {
                // 船
                Data_Ship data = new Data_Ship();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                return data;
            }
            else if (buildingEnum == BuildingEnum.JewelryShop)
            {
                // 珠宝店
                Data_JewelryShop data = new Data_JewelryShop();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                return data;
            }
            else if (buildingEnum == BuildingEnum.ChickenFarm)
            {
                // 养鸡场
                Data_ChickenFarm data = new Data_ChickenFarm();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                data.ChickenNum = 0;
                data.ChickenTotal = 12;

                return data;
            }
            else if (buildingEnum == BuildingEnum.ChickenRoost || buildingEnum == BuildingEnum.ChickenRoostTD)
            {
                // 鸡舍
                Data_ChickenRoost data = new Data_ChickenRoost();


                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;

                ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.ChickenRoost.ToString());
                data.RefreshNeed = resBuilding.speed;

                // 四个鸡蛋
                data.RoostItem = new List<Data_ChickenRoostItem>();
                for (int i = 0; i < 4; i++)
                {
                    data.RoostItem.Add(new Data_ChickenRoostItem());
                }

                return data;
            }
            else
            {
                Data_Default data = new Data_Default();

                data.Pos = new Data_Point(x, y);
                data.Size = new Data_Point(w, h);
                data.BuildingEnum = buildingEnum;
                data.StateEnum = BuildingStateEnum.None;
                data.UnlockTask = unlockTask;
                data.BluePrint = bluePrintEnum;
                data.StatueEnum = statueEnum;
                data.Price = price;
                return data;
            }

        }



        public static Data_BuildingBase DataBuildingFromJson(int _buildingEnum, string json)
        {
            BuildingEnum buildingEnum = (BuildingEnum)_buildingEnum;

            if (buildingEnum == BuildingEnum.PumpkinLand || buildingEnum == BuildingEnum.Farm)
            {
                // 南瓜地
                return LitJsonEx.JsonMapper.ToObject<Data_PumpkinLand>(json);

            }
            if (buildingEnum == BuildingEnum.CarrotPatch)
            {
                // 胡萝卜地
                return LitJsonEx.JsonMapper.ToObject<Data_CarrotPatch>(json);

            }
            if (buildingEnum == BuildingEnum.CottonFarm)
            {
                // 棉花地
                return LitJsonEx.JsonMapper.ToObject<Data_CottonFarm>(json);

            }
            if (buildingEnum == BuildingEnum.AppleTree)
            {
                //
                return LitJsonEx.JsonMapper.ToObject<Data_AppleTree>(json);

            }
            if (buildingEnum == BuildingEnum.OrangeTree)
            {
                //
                return LitJsonEx.JsonMapper.ToObject<Data_OrangeTree>(json);

            }
            else if (buildingEnum == BuildingEnum.Bridge)
            {
                // 桥
                return LitJsonEx.JsonMapper.ToObject<Data_Bridge>(json);

            }
            else if (buildingEnum == BuildingEnum.BridgeTD)
            {
                // 桥
                return LitJsonEx.JsonMapper.ToObject<Data_BridgeTD>(json);

            }
            else if (buildingEnum == BuildingEnum.Storage)
            {
                // 储物仓库
                return LitJsonEx.JsonMapper.ToObject<Data_Storage>(json);

            }
            else if (buildingEnum == BuildingEnum.Sawmill)
            {
                // 锯木厂
                return LitJsonEx.JsonMapper.ToObject<Data_Sawmill>(json);

            }
            else if (buildingEnum == BuildingEnum.Stonemine)
            {
                // 采石厂
                return LitJsonEx.JsonMapper.ToObject<Data_StoneMine>(json);

            }
            else if (buildingEnum == BuildingEnum.Hall)
            {
                // 大厅
                return LitJsonEx.JsonMapper.ToObject<Data_Hall>(json);

            }
            else if (buildingEnum == BuildingEnum.Market)
            {
                // 市场
                return LitJsonEx.JsonMapper.ToObject<Data_Market>(json);
            }
            else if (buildingEnum == BuildingEnum.Workshop)
            {
                // 工坊
                return LitJsonEx.JsonMapper.ToObject<Data_Workshop>(json);
            }
            else if (buildingEnum == BuildingEnum.Armory)
            {
                // 武器库

                return LitJsonEx.JsonMapper.ToObject<Data_Armory>(json);

            }
            else if (buildingEnum == BuildingEnum.House)
            {
                // 房屋
                return LitJsonEx.JsonMapper.ToObject<Data_House>(json);
            }

            else if (buildingEnum == BuildingEnum.Wagon)
            {
                // 马车
                return LitJsonEx.JsonMapper.ToObject<Data_Wagon>(json);
            }
            else if (buildingEnum == BuildingEnum.Portal)
            {
                // 传送门
                return LitJsonEx.JsonMapper.ToObject<Data_Portal>(json);
            }
            else if (buildingEnum == BuildingEnum.TmpPortal)
            {
                // 传送门
                return LitJsonEx.JsonMapper.ToObject<Data_TmpPortal>(json);
            }
            else if (buildingEnum == BuildingEnum.Fountain)
            {
                // 喷泉
                return LitJsonEx.JsonMapper.ToObject<Data_Fountain>(json);
            }
            else if (buildingEnum == BuildingEnum.WoodFactory)
            {
                // 木板床
                return LitJsonEx.JsonMapper.ToObject<Data_WoodFactory>(json);
            }
            else if (buildingEnum == BuildingEnum.Pier)
            {
                // 码头
                return LitJsonEx.JsonMapper.ToObject<Data_Pier>(json);
            }
            else if (buildingEnum == BuildingEnum.ResearchLab)
            {
                // 实验室
                return LitJsonEx.JsonMapper.ToObject<Data_ResearchLab>(json);
            }
            else if (buildingEnum == BuildingEnum.Museum)
            {
                // 博物馆
                return LitJsonEx.JsonMapper.ToObject<Data_Museum>(json);
            }
            else if (buildingEnum == BuildingEnum.StoneFactory)
            {
                // 制砖厂
                return LitJsonEx.JsonMapper.ToObject<Data_StoneFactory>(json);
            }
            else if (buildingEnum == BuildingEnum.SheepFactory)
            {
                // 羊毛厂
                return LitJsonEx.JsonMapper.ToObject<Data_SheepFactory>(json);
            }
            else if (buildingEnum == BuildingEnum.FarmerHouse)
            {
                // 羊圈
                return LitJsonEx.JsonMapper.ToObject<Data_FarmerHouse>(json);
            }
            else if (buildingEnum == BuildingEnum.BreadFactory)
            {
                // 面包厂
                return LitJsonEx.JsonMapper.ToObject<Data_BreadFactory>(json);
            }
            else if (buildingEnum == BuildingEnum.CactusFactory)
            {
                // 药水工厂
                return LitJsonEx.JsonMapper.ToObject<Data_CactusFactory>(json);
            }
            else if (buildingEnum == BuildingEnum.ConeFactory)
            {
                // 松果厂
                return LitJsonEx.JsonMapper.ToObject<Data_ConeFactory>(json);
            }
            else if (buildingEnum == BuildingEnum.CookingPot)
            {
                // 纺织厂
                return LitJsonEx.JsonMapper.ToObject<Data_CookingPot>(json);
            }
            else if (buildingEnum == BuildingEnum.WeaverFactory)
            {
                // 纺织厂
                return LitJsonEx.JsonMapper.ToObject<Data_WeaverFactory>(json);
            }
            else if (buildingEnum == BuildingEnum.DeadWoodFactory)
            {
                // 枯树工厂
                return LitJsonEx.JsonMapper.ToObject<Data_DeadWoodFactory>(json);
            }
            else if (buildingEnum == BuildingEnum.FishingHut)
            {
                // 钓鱼小屋
                return LitJsonEx.JsonMapper.ToObject<Data_FishingHut>(json);
            }
            else if (buildingEnum == BuildingEnum.IronMine)
            {
                // 采矿厂
                return LitJsonEx.JsonMapper.ToObject<Data_IronMine>(json);
            }
            else if (buildingEnum == BuildingEnum.IronFactory)
            {
                // 炼铁厂
                return LitJsonEx.JsonMapper.ToObject<Data_IronFactory>(json);
            }
            else if (buildingEnum == BuildingEnum.Teleport)
            {
                // 传送点
                return LitJsonEx.JsonMapper.ToObject<Data_Teleport>(json);
            }
            else if (buildingEnum == BuildingEnum.Lighthouse)
            {
                // 灯塔
                return LitJsonEx.JsonMapper.ToObject<Data_Lighthouse>(json);
            }
            else if (buildingEnum == BuildingEnum.Academy)
            {
                // 灯塔
                return LitJsonEx.JsonMapper.ToObject<Data_Academy>(json);
            }
            else if (buildingEnum == BuildingEnum.DiamondMine || buildingEnum == BuildingEnum.RubyMine
                || buildingEnum == BuildingEnum.EmeraldMine || buildingEnum == BuildingEnum.AmethystMine)
            {
                // 蓝宝石矿场
                return LitJsonEx.JsonMapper.ToObject<Data_DiamondMine>(json);
            }
            else if (buildingEnum == BuildingEnum.Library)
            {
                // 图书馆
                return LitJsonEx.JsonMapper.ToObject<Data_Library>(json);
            }
            else if (buildingEnum == BuildingEnum.GemTower)
            {
                // 宝石塔
                return LitJsonEx.JsonMapper.ToObject<Data_GemTower>(json);
            }
            else if (buildingEnum == BuildingEnum.Statue)
            {
                // 雕塑
                return LitJsonEx.JsonMapper.ToObject<Data_Statue>(json);
            }
            else if (buildingEnum == BuildingEnum.Ship)
            {
                // 船
                return LitJsonEx.JsonMapper.ToObject<Data_Ship>(json);
            }
            else if (buildingEnum == BuildingEnum.JewelryShop)
            {
                // 珠宝店
                return LitJsonEx.JsonMapper.ToObject<Data_JewelryShop>(json);
            }
            else if (buildingEnum == BuildingEnum.ChickenFarm)
            {
                // 养鸡场
                return LitJsonEx.JsonMapper.ToObject<Data_ChickenFarm>(json);
            }
            else if (buildingEnum == BuildingEnum.ChickenRoost || buildingEnum == BuildingEnum.ChickenRoostTD)
            {
                // 鸡蛋舍
                return LitJsonEx.JsonMapper.ToObject<Data_ChickenRoost>(json);
            }
            else if (buildingEnum == BuildingEnum.SpaceShip)
            {
                // 太空飞船
                return LitJsonEx.JsonMapper.ToObject<Data_SpaceShip>(json);
            }

            else
            {
                return LitJsonEx.JsonMapper.ToObject<Data_Default>(json);
            }

        }


        #region private

        /// <summary>
        /// 图书馆等级
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        private static Data_LibraryAssetsLv getDimaondMineLv(AssetEnum asset)
        {
            ExcelData.Diamond resBuilding = ConfigManger.I.GetDiamondsCfg(asset.ToString());

            MapRender mapRender = MapManager.I.mapRender;
            Building_Library building_Library = MapManager.I.GetSingleBuilding(BuildingEnum.Library) as Building_Library;
            if (building_Library != null)
            {
                Data_Library data_ = building_Library.GetData<Data_Library>();
                return data_.GetLibraryAssetsLv(asset);
            }
            else
            {
                return new Data_LibraryAssetsLv(asset);
            }
        }

        #endregion private

    }
}
