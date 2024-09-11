using System;
using System.Collections.Generic;
using UnityEngine;
using Druid.Utils;
using UnityEngine.AddressableAssets;
using Druid;

namespace Deal.Data
{



    /// <summary>
    /// 传送点
    /// </summary>
    [Serializable]
    public class Data_Teleport : Data_BuildingBase
    {


        public override void Load()
        {
            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewTeleport(this, mapRender.Builds.transform, this.WorldPos);
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

            if (this.UnlockTask == 72)
            {
                // 第一个

                // 传送点
                Data_Teleport data = new Data_Teleport();

                data.Pos = new Data_Point(3 * 2, 4 * 2);
                data.Size = new Data_Point(2, 2);
                data.BuildingEnum = BuildingEnum.Teleport;
                data.StateEnum = BuildingStateEnum.Open;
                data.UnlockTask = -1;
                data.BluePrint = BluePrintEnum.None;
                data.StatueEnum = StatueEnum.None;
                data.Price = null;

                MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);
                mapData.Data.buildings.Add(data);

                data.Load();
            }
        }

    }

}
