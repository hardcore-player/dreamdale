using System;
using System.Collections.Generic;
using Deal.Env;
using Druid;
using UnityEngine;


namespace Deal.Data
{
    /// <summary>
    /// 工人
    /// </summary>
    [Serializable]
    public class Data_Worker : Data_SaveBase
    {

        // 采集资源类型
        public AssetEnum AssetId = AssetEnum.Wood;

        // 当前资源数量
        public int AssetNum = 0;

        // 背包上限
        public int BagTotal = 20;

        // 能力
        public float AbilityVal = 1.0f;

        public long HouseId = 0;

        public override void Load()
        {
            MapRender mapRender = MapManager.I.mapRender;
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

            foreach (Data_BuildingBase data_build in mapData.Data.buildings)
            {
                if (data_build.UniqueId() == this.HouseId)
                {
                    BuildingBase buildingBase = mapRender.GetBuilding(data_build);
                    PrefabsUtils.NewWorker(this, mapRender.transform, buildingBase.transform.position + new Vector3(0, -1.2f, 0));
                    break;
                }
            }

        }


        public bool AddAsset(AssetEnum assetEnum, int num)
        {
            if (assetEnum == this.AssetId && this.AssetNum + num <= this.BagTotal && this.AssetNum + num >= 0)
            {
                AssetNum += num;
                return true;
            }

            return false;
        }
    }

}
