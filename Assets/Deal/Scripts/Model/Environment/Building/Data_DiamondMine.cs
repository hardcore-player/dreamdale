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
    /// 蓝宝石矿场
    /// </summary>
    [Serializable]
    public class Data_DiamondMine : DataResWarehouse
    {

        public override void Load()
        {
            this.RefreshState();

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewDiamondMine(this, mapRender.Builds.transform, this.WorldPos);
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

        public void UpdateNumWithLibraryLv(ExcelData.Diamond resBuilding, Data_LibraryAssetsLv data_Library)
        {
            this.AssetTotal = resBuilding.cValue[data_Library.capacityLv - 1];
            this.RefreshNeed = (int)resBuilding.sValue[data_Library.speedLv - 1] * 60 * 60;
            this.ProductNum = resBuilding.pValue[data_Library.prductLv - 1];
        }

        public override void Open()
        {
            //打开的时候默认10秒
            this.StateEnum = BuildingStateEnum.Open;
            this.CDAt = TimeUtils.TimeNowMilliseconds() - (this.RefreshNeed - 2) * 1000;
            this.IsStop = false;
        }

    }
}