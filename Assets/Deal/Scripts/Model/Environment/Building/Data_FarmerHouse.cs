using System;
using System.Collections.Generic;
using UnityEngine;
using Druid.Utils;
using UnityEngine.AddressableAssets;

namespace Deal.Data
{



    /// <summary>
    /// 羊圈
    /// </summary>
    [Serializable]
    public class Data_FarmerHouse : Data_BuildingBase
    {
        public int SheepTotal = 12;
        public int SheepNum = 0;

        public override void Load()
        {

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewFarmerHouse(this, mapRender.Builds.transform, this.WorldPos);
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

    }

}
