using System.Collections;
using System.Collections.Generic;
using Druid;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;
using Deal.Data;
using Deal.Env;
using Cysharp.Threading.Tasks;
using System;
using Druid.Utils;

namespace Deal
{
    public class MapRender : MonoBehaviour
    {
        #region Inspector
        public SO_MapData SO_MapData;

        public Tilemap Ground;
        public Tilemap Decoration;
        public Tilemap Terrain;
        public Tilemap Resource;
        public Tilemap Builds;
        public Tilemap Fence;
        public Tilemap SeaShadow;

        public RuleTile GroundTile0;
        public RuleTile GroundTile1;
        public RuleTile WaterTile;
        public RuleTile FlowerTile;
        public RuleTile GrassTile;
        public RuleTile Grass2Tile;
        public RuleTile BucketTile;
        public RuleTile TrimStoneTile;
        public RuleTile TrimFlowersTile;
        public RuleTile StoneCircleTile;
        public RuleTile RoadTile;
        public RuleTile BridgeTile;

        public Tile Frence5Tile;
        public Tile Frence5_1Tile;
        public Tile Frence6Tile;
        public Tile Frence6_1Tile;
        public Tile Frence7Tile;
        public Tile Frence8Tile;
        public Tile Frence9Tile;
        public Tile Frence10Tile;
        public RuleTile WaterGrass;
        public RuleTile TrimTree0;
        public RuleTile TrimStone;
        public Tile TrimFence0;
        public Tile TrimFence1;
        public Tile TrimFence2;
        public Tile TrimFence3;
        public Tile TrimFence4;
        public Tile TrimFence5;

        public RuleTile SeaShadow1;
        public RuleTile SeaShadow2;
        public RuleTile SeaShadow3;
        public RuleTile SeaShadow4;


        public Tile Land1;
        public Tile Land2;
        public Tile Land3;

        public GameObject guideArrow;

        #endregion Inspector

        #region 地图上的预制体

        /// <summary>
        /// 树和石头采集资源
        /// </summary>
        public Dictionary<Data_Point, CollectableRes> collectableRes = new Dictionary<Data_Point, CollectableRes>();
        /// <summary>
        /// 空地
        /// </summary>
        public Dictionary<Data_Point, Building_SpaceLand> buildingSpaceLand = new Dictionary<Data_Point, Building_SpaceLand>();
        /// <summary>
        /// 建筑
        /// </summary>
        public Dictionary<Data_Point, BuildingBase> buildings = new Dictionary<Data_Point, BuildingBase>();

        /// <summary>
        /// 所有的资源建筑空地
        /// </summary>
        public Dictionary<long, BindingSaveData> allOnMap = new Dictionary<long, BindingSaveData>();

        /// <summary>
        /// 工人
        /// </summary>
        public List<Worker> workers = new List<Worker>();


        public BindingSaveData GetBuyUniqueId(long uniqueId)
        {
            if (allOnMap.ContainsKey(uniqueId))
            {
                return allOnMap[uniqueId];
            }

            return null;
        }

        public void AddWoker(Worker res)
        {
            workers.Add(res);
        }

        public void AddCollectableRes(CollectableRes res)
        {
            Data_CollectableRes data = res.GetData<Data_CollectableRes>();
            Data_Point point = data.CenterGrid();

            if (collectableRes.ContainsKey(point))
            {
                Destroy(collectableRes[point].gameObject);
                collectableRes[point] = res;
                allOnMap[data.UniqueId()] = res;
            }
            else
            {
                collectableRes.Add(point, res);
                allOnMap.Add(data.UniqueId(), res);
            }
        }

        public CollectableRes GetCollectableRes(Data_CollectableRes res)
        {
            Data_Point point = res.CenterGrid();

            if (collectableRes.ContainsKey(point))
            {
                return collectableRes[point];
            }

            return null;
        }

        public void AddBuilding(BuildingBase building)
        {
            Data_BuildingBase _Data = building.GetData<Data_BuildingBase>();

            Data_Point point = _Data.CenterGrid();

            if (buildings.ContainsKey(point))
            {
                Destroy(buildings[point].gameObject);
                buildings[point] = building;
                allOnMap[_Data.UniqueId()] = building;
            }
            else
            {
                buildings.Add(point, building);
                allOnMap.Add(_Data.UniqueId(), building);
            }

            MapManager.I.SetSingleBuilding(_Data.BuildingEnum, building);

            //if (_Data.BuildingEnum == BuildingEnum.Hall || _Data.BuildingEnum == BuildingEnum.Storage
            //    || _Data.BuildingEnum == BuildingEnum.Market || _Data.BuildingEnum == BuildingEnum.Sawmill
            //    || _Data.BuildingEnum == BuildingEnum.Stonemine || _Data.BuildingEnum == BuildingEnum.Workshop
            //     || _Data.BuildingEnum == BuildingEnum.Armory || _Data.BuildingEnum == BuildingEnum.GemTower
            //     || _Data.BuildingEnum == BuildingEnum.Library)
            //{
            //    MapManager.I.SetSingleBuilding(_Data.BuildingEnum, building);
            //}

        }

        public BuildingBase GetBuilding(Data_BuildingBase building)
        {
            Data_Point point = building.CenterGrid();

            if (buildings.ContainsKey(point))
            {
                return buildings[point];
            }

            return null;
        }

        public void AddSpaceLand(Building_SpaceLand building)
        {
            Data_BuildingBase _Data = building.GetData<Data_BuildingBase>();

            Data_Point point = _Data.CenterGrid();

            if (buildingSpaceLand.ContainsKey(point))
            {
                Destroy(buildingSpaceLand[point].gameObject);
                buildingSpaceLand[point] = building;
                allOnMap[_Data.UniqueId()] = building;

            }
            else
            {
                buildingSpaceLand.Add(point, building);
                allOnMap.Add(_Data.UniqueId(), building);
            }
        }


        public Building_SpaceLand GetSpaceLand(Data_SpaceCost building)
        {
            Data_Point point = building.CenterGrid();

            if (buildingSpaceLand.ContainsKey(point))
            {
                return buildingSpaceLand[point];
            }
            return null;
        }


        /// <summary>
        /// 删除买地
        /// </summary>
        /// <param name="building"></param>
        public void DeleteSpaceLand(Data_SpaceCost building)
        {
            Data_Point point = building.CenterGrid();

            if (buildingSpaceLand.ContainsKey(point))
            {
                Building_SpaceLand spaceLand = buildingSpaceLand[point];
                Destroy(spaceLand.gameObject);

                buildingSpaceLand.Remove(point); ;
            }

        }

        #endregion 地图上的预制体


        #region

        public bool LoadInited = false;

        public async void MapInited()
        {
            long t2 = TimeUtils.TimeNowMilliseconds();
            await InitCollectableRes();

            long t3 = TimeUtils.TimeNowMilliseconds();
            Debug.Log("LoadFromSave InitCollectableRes" + (t3 - t2));

            await InitBuildings();

        }

        public async void MapInited1()
        {
            long t4 = TimeUtils.TimeNowMilliseconds();
            await LoadSpaceLand();

            await UniTask.DelayFrame(1);
            long t5 = TimeUtils.TimeNowMilliseconds();
            Debug.Log("LoadFromSave LoadSpaceLand" + (t5 - t4));

            this.GeneratePathTiles();

            await UniTask.DelayFrame(1);
            long t6 = TimeUtils.TimeNowMilliseconds();
            Debug.Log("LoadFromSave GeneratePathTiles" + (t6 - t5));


            await InitWorker();
        }


        public void Load()
        {
            this.SO_MapData = ConfigManger.I.mapData;
            this.SO_MapData.Init();

            this.collectableRes.Clear();
            this.buildingSpaceLand.Clear();
            this.buildings.Clear();
            this.workers.Clear();
        }

        /// <summary>
        /// 存档加载
        /// </summary>
        public async void LoadFromSave()
        {
            long t1 = TimeUtils.TimeNowMilliseconds();

            this.InitTilemap();

            await UniTask.DelayFrame(1);
            long t2 = TimeUtils.TimeNowMilliseconds();

            Debug.Log("LoadFromSave InitTilemap" + (t2 - t1));

            //await InitCollectableRes();

            await UniTask.DelayFrame(1);
            long t3 = TimeUtils.TimeNowMilliseconds();
            Debug.Log("LoadFromSave InitCollectableRes" + (t3 - t2));


            //await InitBuildings();

            await UniTask.DelayFrame(1);
            long t4 = TimeUtils.TimeNowMilliseconds();
            Debug.Log("LoadFromSave InitBuildings" + (t4 - t3));

        }

        /// <summary>
        /// 加载默认数据
        /// </summary>
        public void LoadFromDefault()
        {
            LoadAllSpaceCost();

            for (int i = 0; i < this.SO_MapData.Open.Count; i++)
            {
                int x = this.SO_MapData.Open[i].x;
                int y = this.SO_MapData.Open[i].y;
                OpenTileData(x, y);
            }
        }

        #endregion

        /// <summary>
        /// 加载所有的卖地信息
        /// </summary>
        public void LoadAllSpaceCost()
        {
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

            foreach (Data_SpaceCost data in SO_MapData.Cost)
            {
                Data_SpaceCost data_ = data.Clone();
                mapData.Data.spaceCosts.Add(data_);
            }
        }


        /// <summary>
        /// 开放一块土地
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void OpenTiles(int xx, int yy, int w, int h)
        {
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData) as MapData;

            for (int y = yy; y < yy + h; y++)
            {
                for (int x = xx; x < xx + w; x++)
                {
                    this.OpenTile(x, y);
                }
            }

        }

        public void OpenTile(int x, int y)
        {
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData) as MapData;

            mapData.Data.openTile.Add(new Data_Point(x, y));

            this._setMapTiles(x, y);
            Data_CollectableRes data = this._openResourceTile(x, y);
            if (data != null) data.Load();
            Data_BuildingBase data1 = this._openBuildingTile(x, y);
            if (data1 != null) data1.Load();

        }

        public void OpenTileData(int x, int y)
        {
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData) as MapData;

            mapData.Data.openTile.Add(new Data_Point(x, y));
            this._openResourceTile(x, y);
            this._openBuildingTile(x, y);
        }

        #region Ground

        /// <summary>
        /// 初始化tilemap
        /// </summary>
        public void InitTilemap()
        {

            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData) as MapData;

            for (int y = NumDefine.MapYBotom; y < NumDefine.MapYTop; y++)
            {
                for (int x = NumDefine.MapXLeft; x < NumDefine.MapXRight; x++)
                {
                    Vector2Int key = new Vector2Int(x, y);
                    RuleTile tile = Ground.GetTile(new Vector3Int(x, y, 0)) as RuleTile;

                    // 水
                    if (tile == null)
                    {
                        Ground.SetTile(new Vector3Int(x, y, 0), WaterTile);
                    }

                    // 水草
                    if (this.SO_MapData.decorationTiles.ContainsKey(key))
                    {
                        string tileName = this.SO_MapData.decorationTiles[key];
                        if (tileName == "WaterGrass")
                        {
                            //Decoration.SetTile(new Vector3Int(x, y, 0), WaterGrass);
                        }
                    }

                    // 水影子
                    if (this.SO_MapData.seaShadowTiles.ContainsKey(key))
                    {
                        string tileName = this.SO_MapData.seaShadowTiles[key];
                        if (tileName == "SeaShadow1")
                        {
                            SeaShadow.SetTile(new Vector3Int(x, y, 0), SeaShadow1);
                        }
                        else if (tileName == "SeaShadow2")
                        {
                            SeaShadow.SetTile(new Vector3Int(x, y, 0), SeaShadow2);
                        }
                        else if (tileName == "SeaShadow3")
                        {
                            SeaShadow.SetTile(new Vector3Int(x, y, 0), SeaShadow3);
                        }
                        else if (tileName == "SeaShadow4")
                        {
                            SeaShadow.SetTile(new Vector3Int(x, y, 0), SeaShadow4);
                        }
                    }
                }
            }

            foreach (Data_Point data in mapData.Data.openTile)
            {
                this._setMapTiles(data.x, data.y);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mapTile"></param>
        public async UniTask InitCollectableRes()
        {
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);
            foreach (Data_SaveBase data in mapData.Data.collectableRes)
            {
                data.Load();
                //await UniTask.DelayFrame(1);
            }
        }

        /// <summary>
        /// 初始化买地
        /// </summary>
        public async UniTask LoadSpaceLand()
        {
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

            foreach (Data_SpaceCost data in mapData.Data.spaceCosts)
            {
                data.Load();
                if (data.ShowInMap == true)
                {
                    //await UniTask.DelayFrame(1);
                }

            }
        }

        /// <summary>
        /// 初始化建筑
        /// </summary>
        public async UniTask InitBuildings()
        {
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);
            Debug.Log("mapData.Data.buildings InitBuildings" + mapData.Data.buildings.Count);
            foreach (Data_BuildingBase data in mapData.Data.buildings)
            {
                data.Load();
                await UniTask.DelayFrame(1);
            }

        }

        public async UniTask InitWorker()
        {
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

            foreach (Data_SaveBase data in mapData.Data.workers)
            {
                data.Load();
                await UniTask.DelayFrame(1);
            }
        }


        /// <summary>
        /// 设置地片
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void _setMapTiles(int x, int y)
        {
            Vector2Int key = new Vector2Int(x, y);
            if (this.SO_MapData.groundTiles.ContainsKey(key))
            {

                string tileName = this.SO_MapData.groundTiles[key];
                if (tileName == "GroundTile0" || tileName == "GroundTile1")
                {
                    if ((x + y) % 2 == 0)
                    {
                        Ground.SetTile(new Vector3Int(x, y, 0), GroundTile0);
                    }
                    else
                    {
                        Ground.SetTile(new Vector3Int(x, y, 0), GroundTile1);
                    }
                }
                else if (tileName == "land1")
                {
                    Ground.SetTile(new Vector3Int(x, y, 0), Land1);
                }
                else if (tileName == "land2")
                {
                    Ground.SetTile(new Vector3Int(x, y, 0), Land2);
                }
                else if (tileName == "land3")
                {
                    Ground.SetTile(new Vector3Int(x, y, 0), Land3);
                }
                else if (tileName == "WaterTile")
                {
                    Ground.SetTile(new Vector3Int(x, y, 0), WaterTile);
                }
            }

            if (this.SO_MapData.decorationTiles.ContainsKey(key))
            {

                string tileName = this.SO_MapData.decorationTiles[key];
                if (tileName == "Flowers")
                {
                    Decoration.SetTile(new Vector3Int(x, y, 0), FlowerTile);
                }
                else if (tileName == "Grass")
                {
                    Decoration.SetTile(new Vector3Int(x, y, 0), GrassTile);
                }
                else if (tileName == "Grass2")
                {
                    Decoration.SetTile(new Vector3Int(x, y, 0), Grass2Tile);
                }
                else if (tileName == "TrimStone")
                {
                    Decoration.SetTile(new Vector3Int(x, y, 0), TrimStoneTile);
                }
                else if (tileName == "TrimFlosers")
                {
                    Decoration.SetTile(new Vector3Int(x, y, 0), TrimFlowersTile);
                }
                else if (tileName == "StoneCircle")
                {
                    Decoration.SetTile(new Vector3Int(x, y, 0), StoneCircleTile);
                }
                else if (tileName == "WaterGrass")
                {
                    //Decoration.SetTile(new Vector3Int(x, y, 0), WaterGrass);
                }
                else if (tileName == "Sign1")
                {
                    PrefabsUtils.NewSign(tileName, this.Decoration.transform, new Vector3(x + 0.5f, y + 0.5f, 0));
                    //Decoration.SetTile(new Vector3Int(x, y, 0), WaterGrass);
                }
                else if (tileName == "Sign2")
                {
                    //Decoration.SetTile(new Vector3Int(x, y, 0), WaterGrass);
                }
                else
                {
                    Debug.LogWarning("decorationTiles no " + tileName);
                }
            }

            if (this.SO_MapData.decorationTiles1.ContainsKey(key))
            {

                string tileName = this.SO_MapData.decorationTiles1[key];

                if (tileName == "Sign1" || tileName == "Sign2" || tileName == "Sign3"
                    || tileName == "Sign4" || tileName == "Sign5" || tileName == "Sign6"
                    || tileName == "Sign7")
                {
                    PrefabsUtils.NewSign(tileName, this.Decoration.transform, new Vector3(x + 0.5f, y + 0.5f, 0));
                    //Decoration.SetTile(new Vector3Int(x, y, 0), WaterGrass);
                }
                else
                {
                    Debug.LogWarning("decorationTiles no " + tileName);
                }
            }

            if (this.SO_MapData.terrainTiles.ContainsKey(key))
            {

                string tileName = this.SO_MapData.terrainTiles[key];
                if (tileName == "RoadTile")
                {
                    Terrain.SetTile(new Vector3Int(x, y, 0), RoadTile);
                }
            }

            if (this.SO_MapData.fenceTiles.ContainsKey(key))
            {

                string tileName = this.SO_MapData.fenceTiles[key];
                if (tileName == "img_fence5")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), Frence5Tile);
                }
                else if (tileName == "img_fence5_1")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), Frence5_1Tile);
                }
                else if (tileName == "img_fence6")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), Frence6Tile);
                }
                else if (tileName == "img_fence6_1")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), Frence6_1Tile);
                }
                else if (tileName == "img_fence7")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), Frence7Tile);
                }
                else if (tileName == "img_fence8")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), Frence8Tile);
                }
                else if (tileName == "img_fence9")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), Frence9Tile);
                }
                else if (tileName == "img_fence10")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), Frence10Tile);
                }
                else if (tileName == "TrimTree0")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), TrimTree0);
                }
                else if (tileName == "TrimStone")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), TrimStone);
                }
                else if (tileName == "TrimFence0")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), TrimFence0);
                }
                else if (tileName == "TrimFence1")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), TrimFence1);
                }
                else if (tileName == "TrimFence2")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), TrimFence2);
                }
                else if (tileName == "TrimFence3")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), TrimFence3);
                }
                else if (tileName == "TrimFence4")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), TrimFence4);
                }
                else if (tileName == "TrimFence5")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), TrimFence5);
                }
                else if (tileName == "Bucket")
                {
                    Fence.SetTile(new Vector3Int(x, y, 0), BucketTile);
                }
                else
                {
                    Debug.LogWarning("fenceTiles no " + tileName);
                }
            }

            if (this.SO_MapData.seaShadowTiles.ContainsKey(key))
            {
                string tileName = this.SO_MapData.seaShadowTiles[key];

                SeaShadow.SetTile(new Vector3Int(x, y, 0), null);
            }

            if (this.SO_MapData.interactiveTiles.ContainsKey(key))
            {

                string tileName = this.SO_MapData.interactiveTiles[key];
                if (tileName != null && tileName != "")
                {
                    Addressables.InstantiateAsync(tileName).Completed += (obj) =>
                    {
                        GameObject go = obj.Result;
                        go.transform.parent = this.Terrain.transform;
                        go.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                    };
                }
            }
        }

        #endregion Ground



        #region Resource


        /// <summary>
        /// 树石头
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private Data_CollectableRes _openResourceTile(int x, int y)
        {
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

            Vector2Int key = new Vector2Int(x, y);
            if (this.SO_MapData.resourcesTiles.ContainsKey(key))
            {
                string tileName = this.SO_MapData.resourcesTiles[key];
                if (tileName == "Tree_1")
                {
                    Data_CollectableRes data = DataUtils.NewDataTree(x, y);
                    //data.Load();
                    mapData.Data.collectableRes.Add(data);

                    return data;
                }
                else if (tileName == "Cactus")
                {
                    Data_CollectableRes data = DataUtils.NewDataCactus(x, y);
                    mapData.Data.collectableRes.Add(data);

                    return data;
                }
                else if (tileName == "WinterWood")
                {
                    Data_CollectableRes data = DataUtils.NewDataWinterWood(x, y);
                    mapData.Data.collectableRes.Add(data);

                    return data;
                }
                else if (tileName == "DeadWood")
                {
                    Data_CollectableRes data = DataUtils.NewDataDeadWood(x, y);
                    mapData.Data.collectableRes.Add(data);

                    return data;
                }
                else if (tileName == "Bamboo")
                {
                    Data_CollectableRes data = DataUtils.NewDataBamboo(x, y);
                    mapData.Data.collectableRes.Add(data);

                    return data;
                }
                else if (tileName == "Stone")
                {
                    Data_CollectableRes data = DataUtils.NewDataStone(x, y);
                    //data.Load();
                    mapData.Data.collectableRes.Add(data);

                    return data;
                }
                else if (tileName == "Treasure")
                {
                    Data_CollectableRes data = DataUtils.NewDataTreasure(x, y);
                    //data.Load();
                    mapData.Data.collectableRes.Add(data);

                    return data;
                }
                else if (tileName == "Grain")
                {
                    Data_CollectableRes data = DataUtils.NewDataGrain(x, y);
                    //data.Load();
                    mapData.Data.collectableRes.Add(data);

                    return data;
                }
                else if (tileName == "Iron")
                {
                    Data_CollectableRes data = DataUtils.NewDataIron(x, y);
                    //data.Load();
                    mapData.Data.collectableRes.Add(data);

                    return data;
                }
                else if (tileName == "AppleTreeSign")
                {

                }
            }

            return null;
        }

        private Data_BuildingBase _openBuildingTile(int x, int y)
        {
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

            Vector2Int key = new Vector2Int(x, y);
            if (this.SO_MapData.buildTiles.ContainsKey(key))
            {
                Data_BuildingBase tile = this.SO_MapData.buildTiles[key];
                // 拷贝一份数据
                List<Data_GameAsset> price = new List<Data_GameAsset>();
                for (int i = 0; i < tile.Price.Count; i++)
                {
                    price.Add(new Data_GameAsset(tile.Price[i].assetType, tile.Price[i].assetNum));
                }

                Data_BuildingBase data = DataUtils.NewDataBuilding(tile.Pos.x, tile.Pos.y, tile.Size.x, tile.Size.y, tile.BuildingEnum, price, tile.BluePrint, tile.StatueEnum, tile.UnlockTask);

                data.BluePrintPrice = 1;
                data.StatuePrice = 1;

                Data_Task _Task = TaskManager.I.GetTask();

                if (data.UnlockTask > 0)
                {
                    if (_Task.TaskId >= data.UnlockTask)
                    {
                        if (data.Price != null && data.Price.Count > 0)
                        {
                            // 如果任务到了
                            data.Building();
                            //data.OpenSmoke();
                            //data.StateEnum = BuildingStateEnum.Building;
                            data.DataBuff();
                        }
                        else
                        {
                            data.StateEnum = BuildingStateEnum.Open;
                        }
                    }
                    else
                    {
                        // 不显示
                        data.StateEnum = BuildingStateEnum.None;
                    }
                }
                else
                {
                    // 没有任务的
                    data.Building();
                    //data.StateEnum = BuildingStateEnum.Building;
                    data.DataBuff();
                }
                //long taskGuideUId = MapManager.I.GetTaskUniqueId(_Task.TaskId);


                //data.Load();
                mapData.Data.buildings.Add(data);

                return data;
            }

            return null;
        }


        #endregion Resource

        #region Map

        private byte[][] mapdata;

        public byte[][] Mapdata { get => mapdata; set => mapdata = value; }

        public void GeneratePathTiles()
        {
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData) as MapData;

            int row = NumDefine.MapTileHeight;
            int col = NumDefine.MapTileWidth;

            Mapdata = new byte[row][];
            for (int i = 0; i < row; i++)
            {
                Mapdata[i] = new byte[col];
                for (int j = 0; j < col; j++)
                {
                    Mapdata[i][j] = (byte)1;
                }
            }

            // 陆地都能走
            foreach (Data_Point data in mapData.Data.openTile)
            {
                int x = data.x;
                int y = data.y;

                Vector2Int key = new Vector2Int(x, y);
                if (this.SO_MapData.groundTiles.ContainsKey(key))
                {
                    string tileName = this.SO_MapData.groundTiles[key];
                    if (tileName == "GroundTile0" || tileName == "GroundTile1")
                    {
                        Mapdata[y + 100][x + 100] = (byte)0;
                    }
                }
            }

            // 树和石头不能走
            foreach (var item in mapData.Data.collectableRes)
            {
                Data_CollectableRes data = item as Data_CollectableRes;
                if (data.AssetId == AssetEnum.Wood || data.AssetId == AssetEnum.Stone)
                {
                    Data_Point _Point = data.StartGrid();
                    int x = _Point.x;
                    int y = _Point.y;
                    Mapdata[y + 100][x + 100] = (byte)1;
                }
            }

            // 建筑不能走
            foreach (var item in mapData.Data.buildings)
            {
                Data_BuildingBase data = item as Data_BuildingBase;
                if (data.BuildingEnum == BuildingEnum.Bridge)
                {
                    // 桥能走
                    Data_Point _Point = data.StartGrid();
                    int x = _Point.x;
                    int y = _Point.y;

                    for (int yy = 0; yy < data.Size.y; yy++)
                    {
                        for (int xx = 0; xx < data.Size.x; xx++)
                        {
                            Mapdata[y + 100 + yy][x + 100 + xx] = (byte)0;
                        }
                    }
                }
                else if (data.BuildingEnum == BuildingEnum.PumpkinLand || data.BuildingEnum == BuildingEnum.Farm)
                {
                    // 南瓜地能走
                }

                else
                {
                    Data_Point _Point = data.CenterGrid();
                    int x = _Point.x;
                    int y = _Point.y;

                    //Debug.Log("data.BuildingEnum=x" + data.Size.x);

                    for (int yy = 0; yy < 1; yy++)
                    {
                        for (int xx = 0; xx < 1; xx++)
                        {
                            Mapdata[y + 100 + yy][x + 100 + xx] = (byte)1;
                        }
                    }

                    //Data_Point _Point = data.StartGrid();
                    //int x = _Point.x;
                    //int y = _Point.y;

                    //Debug.Log("data.BuildingEnum=x" + data.Size.x);

                    //for (int yy = 0; yy < data.Size.y; yy++)
                    //{
                    //    for (int xx = 0; xx < data.Size.x; xx++)
                    //    {
                    //        Mapdata[y + 100 + yy][x + 100 + xx] = (byte)1;
                    //    }
                    //}
                }
            }


            //for (int i = 0; i < row; i++)
            //{
            //    for (int j = 0; j < col; j++)
            //    {
            //        if (Mapdata[i][j] == 0)
            //        {
            //            this.Decoration.SetTile(new Vector3Int(j - 100, i - 100, 0), GroundTile0);
            //        }
            //        else
            //        {
            //            this.Decoration.SetTile(new Vector3Int(j - 100, i - 100, 0), WaterTile);
            //        }
            //    }
            //}
        }

        #endregion Map

    }

}
