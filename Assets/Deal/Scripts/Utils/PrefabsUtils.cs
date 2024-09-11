using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deal.Data;
using UnityEngine.AddressableAssets;
using Deal.Env;
using System;
using UnityEngine.ResourceManagement.AsyncOperations;
using DG.Tweening;

namespace Deal
{

    public class PrefabsUtils
    {
        /// <summary>
        ///  建筑变化委托
        /// </summary>
        /// <param name="task"></param>
        public delegate void DelegateBuildingLoad(Data_BuildingBase data, BuildingBase building);

        public static DelegateBuildingLoad OnBuildingLoad;


        /// <summary>
        /// 需要加载的
        /// </summary>
        public static int inLoadCount = 0;
        /// <summary>
        /// 加载完成的
        /// </summary>
        public static int hasLoadCount = 0;

        public static AsyncOperationHandle<GameObject> InstantiateAsync(object key, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
        {
            inLoadCount++;
            return Addressables.InstantiateAsync(key, parent, instantiateInWorldSpace);
        }


        public static void InstantiateBuilding<T>(Data_BuildingBase data, string path, Transform parent, Vector3 pos, Action<T> Completed = null) where T : BuildingBase
        {

            Debug.Log("InstantiateBuilding" + data.BuildingEnum + "   " + path);
            MapRender mapRender = MapManager.I.mapRender;

            InstantiateAsync(path).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;

                T res = go.GetComponent<T>();
                res.SetData(data);

                if (Completed != null) Completed(res);

                // 加入列表
                mapRender.AddBuilding(res);
                if (OnBuildingLoad != null)
                    OnBuildingLoad(data, res);

            };
        }


        /// <summary>
        /// 标准
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        public static void NewSign(string signName, Transform parent, Vector3 pos)
        {
            MapRender mapRender = MapManager.I.mapRender;

            string path = AddressbalePathEnum.PREFAB_Sign1;
            if (signName == "Sign1")
            {
                path = AddressbalePathEnum.PREFAB_Sign1;
            }
            else if (signName == "Sign2")
            {
                path = AddressbalePathEnum.PREFAB_Sign2;
            }
            else if (signName == "Sign3")
            {
                path = AddressbalePathEnum.PREFAB_Sign3;
            }
            else if (signName == "Sign4")
            {
                path = AddressbalePathEnum.PREFAB_Sign4;
            }
            else if (signName == "Sign5")
            {
                path = AddressbalePathEnum.PREFAB_Sign5;
            }
            else if (signName == "Sign6")
            {
                path = AddressbalePathEnum.PREFAB_Sign6;
            }
            else if (signName == "Sign7")
            {
                path = AddressbalePathEnum.PREFAB_Sign7;
            }

            // 未开放
            InstantiateAsync(path).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
            };
        }

        /// <summary>
        /// 默认一个树
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewWorker(Data_Worker data, Transform parent, Vector3 pos, Action<Worker> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            // 未开放
            InstantiateAsync(AddressbalePathEnum.PREFAB_Worker).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Worker res = go.GetComponent<Worker>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddWoker(res);
            };
        }


        /// <summary>
        /// 默认一个树
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewTree(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_Tree> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            // 未开放
            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_Tree).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_Tree res = go.GetComponent<Res_Tree>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }

        /// <summary>
        /// 默认一个仙人掌
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewCactus(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_Cactus> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            // 未开放
            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_Cactus).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_Cactus res = go.GetComponent<Res_Cactus>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }

        /// <summary>
        /// 默认一个松树
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewWinterWood(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_WinterWood> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            // 未开放
            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_WinterWood).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_WinterWood res = go.GetComponent<Res_WinterWood>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }

        /// <summary>
        /// 默认一个枯树
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewDeadWood(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_DeadWood> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            // 未开放
            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_DeadWood).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_DeadWood res = go.GetComponent<Res_DeadWood>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }

        /// <summary>
        /// 默认一个竹子
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewBamboo(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_Bamboo> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            // 未开放
            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_Bamboo).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_Bamboo res = go.GetComponent<Res_Bamboo>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }

        /// <summary>
        /// 默认一个石头
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewStone(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_Stone> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_Stone).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_Stone res = go.GetComponent<Res_Stone>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }


        /// <summary>
        /// 默认一个南瓜
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewPumkin(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_Pumpkin> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_Pumpkin).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_Pumpkin res = go.GetComponent<Res_Pumpkin>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }

        /// <summary>
        /// 默认一个胡萝卜
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewCarrot(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_Carrot> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_Carrot).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_Carrot res = go.GetComponent<Res_Carrot>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }

        /// <summary>
        /// 默认一个棉花
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewCotton(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_Cotton> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_Cotton).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_Cotton res = go.GetComponent<Res_Cotton>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }

        /// <summary>
        /// 默认一个苹果树
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewApplTree(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_AppleTree> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_AppleTree).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_AppleTree res = go.GetComponent<Res_AppleTree>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }

        /// <summary>
        /// 默认一个橘子树
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewOrangeTree(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_OrangeTree> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_OrangeTree).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_OrangeTree res = go.GetComponent<Res_OrangeTree>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }

        /// <summary>
        /// 默认一个宝藏
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewTreasure(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_Treasure> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_Treasure).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_Treasure res = go.GetComponent<Res_Treasure>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }

        /// <summary>
        /// 默认一个
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewGrain(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_Grain> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_Grain).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_Grain res = go.GetComponent<Res_Grain>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }

        public static void NewIron(Data_CollectableRes data, Transform parent, Vector3 pos, Action<Res_Iron> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            InstantiateAsync(AddressbalePathEnum.PREFAB_Res_Iron).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;
                Res_Iron res = go.GetComponent<Res_Iron>();
                res.SetData(data);

                if (Completed != null) Completed(res);
                // 加入列表
                mapRender.AddCollectableRes(res);
            };
        }

        public static void NewDefault(Data_Default data, Transform parent, Vector3 pos, Action<Building_Default> Completed = null)
        {
            InstantiateBuilding<Building_Default>(data, AddressbalePathEnum.PREFAB_BuildingNull, parent, pos, Completed);
        }

        public static void NewDefaultUnlocked(Data_BuildingBase data, Transform parent, Vector3 pos, Action<Building_DefaultUnlocked> Completed = null)
        {
            InstantiateBuilding<Building_DefaultUnlocked>(data, AddressbalePathEnum.PREFAB_BuildingNull_Unlocked, parent, pos, Completed);
        }

        public static void NewDefaultNone(Data_BuildingBase data, Transform parent, Vector3 pos, Action<Building_DefaultNone> Completed = null)
        {
            InstantiateBuilding<Building_DefaultNone>(data, AddressbalePathEnum.PREFAB_BuildingNull_None, parent, pos, Completed);
        }

        /// <summary>
        /// 默认一个桥纵向
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewBridge(Data_Bridge data, Transform parent, Vector3 pos, Action<Building_Bridge> Completed = null)
        {
            InstantiateBuilding<Building_Bridge>(data, AddressbalePathEnum.PREFAB_Bridge, parent, pos, Completed);
        }

        /// <summary>
        /// 默认一个桥横向
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewBridgeTD(Data_BridgeTD data, Transform parent, Vector3 pos, Action<Building_BridgeTD> Completed = null)
        {
            InstantiateBuilding<Building_BridgeTD>(data, AddressbalePathEnum.PREFAB_BridgeTD, parent, pos, Completed);
        }


        /// <summary>
        /// 默认一个
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewSpaceLand(Data_SpaceCost data, Transform parent, Vector3 pos, Action<Building_SpaceLand> Completed = null)
        {

            //InstantiateBuilding<Building_SpaceLand>(data, AddressbalePathEnum.PREFAB_SpaceLand, parent, pos, Completed);

            MapRender mapRender = MapManager.I.mapRender;

            InstantiateAsync(AddressbalePathEnum.PREFAB_SpaceLand).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;

                Building_SpaceLand res = go.GetComponent<Building_SpaceLand>();
                res.SetData(data);

                if (Completed != null) Completed(res);

                // 加入列表
                mapRender.AddSpaceLand(res);
                OnBuildingLoad(data, res);

            };

        }


        /// <summary>
        /// 默认一个南瓜地
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewPumpkinLand(Data_PumpkinLand data, Transform parent, Vector3 pos, Action<Building_PumpkinLand> Completed = null)
        {

            InstantiateBuilding<Building_PumpkinLand>(data, AddressbalePathEnum.PREFAB_PumpkinLand, parent, pos, Completed);
        }


        /// <summary>
        /// 默认一个南瓜地
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewPumpkinLandUnlock(Data_PumpkinLand data, Transform parent, Vector3 pos, Action<Building_PumpkinLandUnlocked> Completed = null)
        {
            InstantiateBuilding<Building_PumpkinLandUnlocked>(data, AddressbalePathEnum.PREFAB_PumpkinLand_Unlocked, parent, pos, Completed);
        }

        /// <summary>
        /// 默认一个胡萝卜田
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewCarrotPatch(Data_CarrotPatch data, Transform parent, Vector3 pos, Action<Building_CarrotPatch> Completed = null)
        {

            InstantiateBuilding<Building_CarrotPatch>(data, AddressbalePathEnum.PREFAB_CarrotPatch, parent, pos, Completed);
        }

        /// <summary>
        /// 默认一个棉花田
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewCottonFarm(Data_CottonFarm data, Transform parent, Vector3 pos, Action<Building_CottonFarm> Completed = null)
        {

            InstantiateBuilding<Building_CottonFarm>(data, AddressbalePathEnum.PREFAB_CottonLand, parent, pos, Completed);
        }


        /// <summary>
        /// 默认一个胡萝卜田
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewCarrotPatchUnlock(Data_CarrotPatch data, Transform parent, Vector3 pos, Action<Building_CarrotPatchUnlocked> Completed = null)
        {
            InstantiateBuilding<Building_CarrotPatchUnlocked>(data, AddressbalePathEnum.PREFAB_CarrotPatch_Unlocked, parent, pos, Completed);
        }


        /// <summary>
        /// 默认一个苹果树
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewAppleTreeUnlock(Data_AppleTree data, Transform parent, Vector3 pos, Action<Building_AppleTreeUnlocked> Completed = null)
        {
            InstantiateBuilding<Building_AppleTreeUnlocked>(data, AddressbalePathEnum.PREFAB_AppleTree_Unlocked, parent, pos, Completed);

        }

        public static void NewAppleTreeLand(Data_AppleTree data, Transform parent, Vector3 pos, Action<Building_AppleTree> Completed = null)
        {

            InstantiateBuilding<Building_AppleTree>(data, AddressbalePathEnum.PREFAB_AppleTree, parent, pos, Completed);
        }

        /// <summary>
        /// 默认一个苹果树
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewOrangeTreeUnlock(Data_OrangeTree data, Transform parent, Vector3 pos, Action<Building_OrangeTreeUnlocked> Completed = null)
        {
            InstantiateBuilding<Building_OrangeTreeUnlocked>(data, AddressbalePathEnum.PREFAB_OrangeTree_Unlocked, parent, pos, Completed);

        }

        public static void NewOrangeTreeLand(Data_OrangeTree data, Transform parent, Vector3 pos, Action<Building_OrangeTree> Completed = null)
        {

            InstantiateBuilding<Building_OrangeTree>(data, AddressbalePathEnum.PREFAB_OrangeTree, parent, pos, Completed);
        }

        /// <summary>
        /// 仓库
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewStorage(Data_Storage data, Transform parent, Vector3 pos, Action<Building_Storage> Completed = null)
        {
            InstantiateBuilding<Building_Storage>(data, AddressbalePathEnum.PREFAB_BuildingStorage, parent, pos, Completed);
        }


        /// <summary>
        /// 锯木厂
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewSawmill(Data_Sawmill data, Transform parent, Vector3 pos, Action<Building_Sawmill> Completed = null)
        {
            InstantiateBuilding<Building_Sawmill>(data, AddressbalePathEnum.PREFAB_BuildingSawmill, parent, pos, Completed);
        }

        /// <summary>
        /// 采石长
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewStoneMine(Data_StoneMine data, Transform parent, Vector3 pos, Action<Building_StoneMine> Completed = null)
        {
            InstantiateBuilding<Building_StoneMine>(data, AddressbalePathEnum.PREFAB_BuildingStoneMine, parent, pos, Completed);
        }

        /// <summary>
        /// 采矿长
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewIronMine(Data_IronMine data, Transform parent, Vector3 pos, Action<Building_IronMine> Completed = null)
        {
            InstantiateBuilding<Building_IronMine>(data, AddressbalePathEnum.PREFAB_BuildingIronMine, parent, pos, Completed);

        }


        /// <summary>
        /// 大厅
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewHall(Data_Hall data, Transform parent, Vector3 pos, Action<Building_Hall> Completed = null)
        {
            InstantiateBuilding<Building_Hall>(data, AddressbalePathEnum.PREFAB_BuildingHall, parent, pos, Completed);
        }

        /// <summary>
        /// 市场
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewMarket(Data_Market data, Transform parent, Vector3 pos, Action<Building_Market> Completed = null)
        {
            InstantiateBuilding<Building_Market>(data, AddressbalePathEnum.PREFAB_BuildingMarket, parent, pos, Completed);
        }

        /// <summary>
        /// 工坊
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewWorkshop(Data_Workshop data, Transform parent, Vector3 pos, Action<Building_Workshop> Completed = null)
        {
            InstantiateBuilding<Building_Workshop>(data, AddressbalePathEnum.PREFAB_BuildingWorkshop, parent, pos, Completed);
        }

        /// <summary>
        /// 飞船
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewSpaceShip(Data_SpaceShip data, Transform parent, Vector3 pos, Action<Building_SpaceShip> Completed = null)
        {
            InstantiateBuilding<Building_SpaceShip>(data, AddressbalePathEnum.PREFAB_BuildingSpaceShip, parent, pos, Completed);
        }

        /// <summary>
        /// 武器库
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewArmory(Data_Armory data, Transform parent, Vector3 pos, Action<Building_Armory> Completed = null)
        {
            InstantiateBuilding<Building_Armory>(data, AddressbalePathEnum.PREFAB_BuildingArmory, parent, pos, Completed);
        }


        /// <summary>
        /// 工坊
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewHouse(Data_House data, Transform parent, Vector3 pos, Action<Building_House> Completed = null)
        {
            InstantiateBuilding<Building_House>(data, AddressbalePathEnum.PREFAB_BuildingHouse, parent, pos, Completed);
        }


        /// <summary>
        /// 工坊
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewWagon(Data_Wagon data, Transform parent, Vector3 pos, Action<Building_Wagon> Completed = null)
        {
            InstantiateBuilding<Building_Wagon>(data, AddressbalePathEnum.PREFAB_BuildingWagon, parent, pos, Completed);
        }


        public static void NewPortal(Data_Portal data, Transform parent, Vector3 pos, Action<Building_Portal> Completed = null)
        {
            InstantiateBuilding<Building_Portal>(data, AddressbalePathEnum.PREFAB_BuildingPortal, parent, pos, Completed);
        }



        public static void NewTmpPortal(Data_TmpPortal data, Transform parent, Vector3 pos, Action<Building_TmpPortal> Completed = null)
        {
            InstantiateBuilding<Building_TmpPortal>(data, AddressbalePathEnum.PREFAB_BuildingTmpPortal, parent, pos, Completed);
        }


        /// <summary>
        /// 图书馆
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewLibrary(Data_Library data, Transform parent, Vector3 pos, Action<Building_Library> Completed = null)
        {
            InstantiateBuilding<Building_Library>(data, AddressbalePathEnum.PREFAB_BuildingLibrary, parent, pos, Completed);
        }

        /// <summary>
        /// 喷泉
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewFountain(Data_Fountain data, Transform parent, Vector3 pos, Action<Building_Fountain> Completed = null)
        {
            InstantiateBuilding<Building_Fountain>(data, AddressbalePathEnum.PREFAB_BuildingFountain, parent, pos, Completed);
        }


        /// <summary>
        /// 木板厂
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewWoodFactory(Data_WoodFactory data, Transform parent, Vector3 pos, Action<Building_WoodFactory> Completed = null)
        {
            InstantiateBuilding<Building_WoodFactory>(data, AddressbalePathEnum.PREFAB_BuildingWoodFactory, parent, pos, Completed);
        }

        /// <summary>
        /// 制砖厂
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewStoneFactory(Data_StoneFactory data, Transform parent, Vector3 pos, Action<Building_StoneFactory> Completed = null)
        {
            InstantiateBuilding<Building_StoneFactory>(data, AddressbalePathEnum.PREFAB_BuildingStoneFactory, parent, pos, Completed);
        }

        /// <summary>
        /// 面包厂
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewBreadFactory(Data_BreadFactory data, Transform parent, Vector3 pos, Action<Building_BreadFactory> Completed = null)
        {
            InstantiateBuilding<Building_BreadFactory>(data, AddressbalePathEnum.PREFAB_BuildingBreadFactory, parent, pos, Completed);
        }

        /// <summary>
        /// 药水工厂
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewCactusFactory(Data_CactusFactory data, Transform parent, Vector3 pos, Action<Building_CactusFactory> Completed = null)
        {
            InstantiateBuilding<Building_CactusFactory>(data, AddressbalePathEnum.PREFAB_BuildingPotion, parent, pos, Completed);
        }

        /// <summary>
        /// 松果工厂
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewConeFactory(Data_ConeFactory data, Transform parent, Vector3 pos, Action<Building_ConeFactory> Completed = null)
        {
            InstantiateBuilding<Building_ConeFactory>(data, AddressbalePathEnum.PREFAB_BuildingConeFactory, parent, pos, Completed);
        }

        /// <summary>
        /// 烹饪锅
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewCookingPot(Data_CookingPot data, Transform parent, Vector3 pos, Action<Building_CookingPot> Completed = null)
        {
            InstantiateBuilding<Building_CookingPot>(data, AddressbalePathEnum.PREFAB_BuildingFishingSoup, parent, pos, Completed);
        }

        /// <summary>
        /// 枯树工厂
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewDeadWoodFactory(Data_DeadWoodFactory data, Transform parent, Vector3 pos, Action<Building_DeadWoodFactory> Completed = null)
        {
            InstantiateBuilding<Building_DeadWoodFactory>(data, AddressbalePathEnum.PREFAB_BuildingDeadWoodFactory, parent, pos, Completed);
        }

        /// <summary>
        /// 纺织工厂
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewWeaverFactory(Data_WeaverFactory data, Transform parent, Vector3 pos, Action<Building_WeaverFactory> Completed = null)
        {
            InstantiateBuilding<Building_WeaverFactory>(data, AddressbalePathEnum.PREFAB_BuildingBambooFactory, parent, pos, Completed);
        }

        /// <summary>
        /// 炼铁厂
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewIronFactory(Data_IronFactory data, Transform parent, Vector3 pos, Action<Building_IronFactory> Completed = null)
        {
            InstantiateBuilding<Building_IronFactory>(data, AddressbalePathEnum.PREFAB_BuildingIronFactory, parent, pos, Completed);
        }

        /// <summary>
        /// 钓鱼小屋
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewFishingHut(Data_FishingHut data, Transform parent, Vector3 pos, Action<Building_FishingHut> Completed = null)
        {
            InstantiateBuilding<Building_FishingHut>(data, AddressbalePathEnum.PREFAB_BuildingFishingHut, parent, pos, Completed);
        }

        /// <summary>
        /// 码头
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewPier(Data_Pier data, Transform parent, Vector3 pos, Action<Building_Pier> Completed = null)
        {
            InstantiateBuilding<Building_Pier>(data, AddressbalePathEnum.PREFAB_BuildingPier, parent, pos, Completed);
        }

        /// <summary>
        /// 实验室
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewResearchLab(Data_ResearchLab data, Transform parent, Vector3 pos, Action<Building_ResearchLab> Completed = null)
        {
            InstantiateBuilding<Building_ResearchLab>(data, AddressbalePathEnum.PREFAB_BuildingResearchLab, parent, pos, Completed);
        }

        /// <summary>
        /// 博物馆
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewMuseum(Data_Museum data, Transform parent, Vector3 pos, Action<Building_Museum> Completed = null)
        {
            InstantiateBuilding<Building_Museum>(data, AddressbalePathEnum.PREFAB_BuildingMuseum, parent, pos, Completed);
        }

        /// <summary>
        /// 羊圈
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewFarmerHouse(Data_FarmerHouse data, Transform parent, Vector3 pos, Action<Building_FarmerHouse> Completed = null)
        {
            InstantiateBuilding<Building_FarmerHouse>(data, AddressbalePathEnum.PREFAB_BuildingFarmerHouse, parent, pos, Completed);
        }

        /// <summary>
        /// 养鸡场
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewChickenFarm(Data_ChickenFarm data, Transform parent, Vector3 pos, Action<Building_ChickenFarm> Completed = null)
        {
            InstantiateBuilding<Building_ChickenFarm>(data, AddressbalePathEnum.PREFAB_BuildingHenhouse, parent, pos, Completed);
        }

        /// <summary>
        /// 羊毛厂
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewSheepFactory(Data_SheepFactory data, Transform parent, Vector3 pos, Action<Building_SheepFactory> Completed = null)
        {
            InstantiateBuilding<Building_SheepFactory>(data, AddressbalePathEnum.PREFAB_BuildingSheepFactory, parent, pos, Completed);
        }

        public static void NewChickenFactory(Data_ChickenRoost data, Transform parent, Vector3 pos, Action<Building_ChickenRoost> Completed = null)
        {
            InstantiateBuilding<Building_ChickenRoost>(data, AddressbalePathEnum.PREFAB_BuildingChickenRoost, parent, pos, Completed);
        }

        public static void NewChickenFactoryTD(Data_ChickenRoost data, Transform parent, Vector3 pos, Action<Building_ChickenRoost> Completed = null)
        {
            InstantiateBuilding<Building_ChickenRoost>(data, AddressbalePathEnum.PREFAB_BuildingChickenRoostTD, parent, pos, Completed);
        }

        /// <summary>
        /// 传送点
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewTeleport(Data_Teleport data, Transform parent, Vector3 pos, Action<Building_Teleport> Completed = null)
        {
            InstantiateBuilding<Building_Teleport>(data, AddressbalePathEnum.PREFAB_BuildingTeleport, parent, pos, Completed);
        }

        /// <summary>
        /// 灯塔
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewLighthouse(Data_Lighthouse data, Transform parent, Vector3 pos, Action<Building_Lighthouse> Completed = null)
        {
            InstantiateBuilding<Building_Lighthouse>(data, AddressbalePathEnum.PREFAB_BuildingLighthouse, parent, pos, Completed);
        }

        /// <summary>
        /// 学院
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewAcademy(Data_Academy data, Transform parent, Vector3 pos, Action<Building_Academy> Completed = null)
        {
            InstantiateBuilding<Building_Academy>(data, AddressbalePathEnum.PREFAB_BuildingAcademy, parent, pos, Completed);
        }

        /// <summary>
        /// 蓝宝石矿场
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewDiamondMine(Data_DiamondMine data, Transform parent, Vector3 pos, Action<Building_DiamondMine> Completed = null)
        {
            InstantiateBuilding<Building_DiamondMine>(data, AddressbalePathEnum.PREFAB_BuildingDiamondMine, parent, pos, Completed);
        }

        /// <summary>
        /// 雕塑
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewStatue(Data_Statue data, Transform parent, Vector3 pos, Action<Building_Statue> Completed = null)
        {
            InstantiateBuilding<Building_Statue>(data, AddressbalePathEnum.PREFAB_BuildingStatue, parent, pos, Completed);
        }

        /// <summary>
        /// 宝石塔
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewGemTower(Data_GemTower data, Transform parent, Vector3 pos, Action<Building_GemTower> Completed = null)
        {
            InstantiateBuilding<Building_GemTower>(data, AddressbalePathEnum.PREFAB_BuildingGemTower, parent, pos, Completed);
        }

        /// <summary>
        /// 船
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewShip(Data_Ship data, Transform parent, Vector3 pos, Action<Building_Ship> Completed = null)
        {
            InstantiateBuilding<Building_Ship>(data, AddressbalePathEnum.PREFAB_BuildingShip, parent, pos, Completed);
        }

        /// <summary>
        /// 珠宝店
        /// </summary>
        /// <param name="data"></param>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewJewelryShop(Data_JewelryShop data, Transform parent, Vector3 pos, Action<Building_JewelryShop> Completed = null)
        {
            InstantiateBuilding<Building_JewelryShop>(data, AddressbalePathEnum.PREFAB_BuildingJewelryShop, parent, pos, Completed);
        }



        /// <summary>
        /// 默认一个任务宝箱
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static void NewTaskChest(Transform parent, Vector3 pos, Action<GameObject> Completed = null)
        {
            MapRender mapRender = MapManager.I.mapRender;

            // 未开放
            InstantiateAsync(AddressbalePathEnum.PREFAB_TaskChest).Completed += (obj) =>
                {
                    hasLoadCount++;
                    // 加载完成回调
                    GameObject go = obj.Result;
                    go.transform.parent = parent;
                    go.transform.position = pos;

                    if (Completed != null) Completed(go);
                };
        }

        public static void NewUnlockSmoke(Transform parent, Vector3 pos, Action action = null)
        {

            InstantiateAsync(AddressbalePathEnum.PREFAB_SmokeCompleted).Completed += (obj) =>
            {
                hasLoadCount++;
                // 加载完成回调
                GameObject go = obj.Result;
                go.transform.parent = parent;
                go.transform.position = pos;

                Sequence s = DOTween.Sequence();
                s.AppendInterval(0.2f);
                s.AppendCallback(() =>
                {
                    if (action != null)
                    {
                        action();
                    }
                });
                s.AppendInterval(10.4f);
                s.AppendCallback(() =>
                {
                    GameObject.Destroy(go);
                });

            };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="pos"></param>
        /// <param name="Completed"></param>
        public static void NewDungeonTaskChest(Transform parent, Vector3 pos, Action<GameObject> Completed = null)
        {
            //MapRender mapRender = MapManager.I.mapRender;

            //// 未开放
            //InstantiateAsync(AddressbalePathEnum.PREFAB_DungeonChest).Completed += (obj) =>
            //{
            //    // 加载完成回调
            //    GameObject go = obj.Result;
            //    go.transform.parent = parent;
            //    go.transform.position = pos;

            //    if (Completed != null) Completed(go);
            //};
        }

    }
}
