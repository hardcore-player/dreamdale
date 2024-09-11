using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.Tilemaps;
using UnityEngine.AddressableAssets;
using Deal.Data;
using Deal.Env;

namespace Deal
{
    public class MapManager : PersistentSingleton<MapManager>
    {
        public MapRender mapRender;

        /// <summary>
        /// 唯一的建筑
        /// </summary>
        public Dictionary<BuildingEnum, BuildingBase> SingleBuilding = new Dictionary<BuildingEnum, BuildingBase>();
        //public Dictionary<BuildingEnum, BuildingBase> SingleBuilding = new Dictionary<BuildingEnum, BuildingBase>();

        public void LoadMap()
        {
            this.SingleBuilding.Clear();

            mapRender.Load();

            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData) as MapData;
            // 加载已经开放对格子
            if (mapData.Data.openTile.Count == 0)
            {
                Debug.Log("加载初始化开放的");
                mapRender.LoadFromDefault();
                mapData.Save();
            }
            else
            {

                Debug.Log("加载已经有的");
            }

            mapRender.LoadFromSave();
        }


        /// <summary>
        /// 打开一片地
        /// </summary>
        /// <param name="xx"></param>
        /// <param name="yy"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        public void OpenTiles(int xx, int yy, int w, int h)
        {
            this.mapRender.OpenTiles(xx, yy, w, h);
            // 刷买地
            this.mapRender.LoadSpaceLand();
            this.mapRender.GeneratePathTiles();
        }


        public BuildingBase SetSingleBuilding(BuildingEnum buildingEnum, BuildingBase building)
        {
            if (this.SingleBuilding.ContainsKey(buildingEnum))
            {
                this.SingleBuilding[buildingEnum] = building;
            }
            else
            {
                this.SingleBuilding.Add(buildingEnum, building);
            }

            return null;
        }

        /// <summary>
        /// 唯一型的建筑
        /// </summary>
        /// <param name="buildingEnum"></param>
        /// <returns></returns>
        public BuildingBase GetSingleBuilding(BuildingEnum buildingEnum)
        {
            if (this.SingleBuilding.ContainsKey(buildingEnum))
            {
                return this.SingleBuilding[buildingEnum];
            }

            return null;
        }


        /// <summary>
        /// 获得建筑和资源
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns></returns>
        public BindingSaveData GetBindingByUniqueId(long uniqueId)
        {
            return this.mapRender.GetBuyUniqueId(uniqueId);
        }


        /// <summary>
        /// 获取任务对应的建筑ID
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public long GetTaskUniqueId(int taskId)
        {
            if (this.mapRender.SO_MapData.guides.ContainsKey(taskId - 1))
            {
                return this.mapRender.SO_MapData.guides[taskId - 1];
            }
            return 0;
        }
    }
}
