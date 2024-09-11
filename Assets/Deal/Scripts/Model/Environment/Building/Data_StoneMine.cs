using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;
using Druid.Utils;

namespace Deal.Data
{

    /// <summary>
    /// 采石厂
    /// </summary>
    [Serializable]
    public class Data_StoneMine : DataResWarehouse
    {

        public override void Load()
        {
            this.RefreshState();

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewStoneMine(this, mapRender.Builds.transform, this.WorldPos);
            }
            else if (this.StateEnum == BuildingStateEnum.Building)
            {
                PrefabsUtils.NewDefaultUnlocked(this, mapRender.Builds.transform, this.WorldPos);
            }
            else
            {
                PrefabsUtils.NewDefaultNone(this, mapRender.Builds.transform, this.WorldPos);
            }
        }

        public override void Open()
        {
            base.Open();

            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);
            Data_Worker _Worker = DataUtils.NewDataWorker();
            _Worker.AssetId = AssetEnum.Stone;
            _Worker.HouseId = this.UniqueId();
            mapData.Data.workers.Add(_Worker);

            _Worker.Load();

        }

    }
}