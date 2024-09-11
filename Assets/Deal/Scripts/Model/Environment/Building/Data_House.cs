using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;

namespace Deal.Data
{

    /// <summary>
    /// 房屋
    /// </summary>
    [Serializable]
    public class Data_House : Data_BuildingBase
    {
        public AssetEnum WorkerType;

        public override void Load()
        {

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewHouse(this, mapRender.Builds.transform, this.WorldPos);
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
            _Worker.AssetId = AssetEnum.Task;
            _Worker.HouseId = this.UniqueId();
            mapData.Data.workers.Add(_Worker);

            _Worker.Load();

        }
    }
}