using System;
using System.Collections.Generic;
using UnityEngine;
using Druid.Utils;
using UnityEngine.AddressableAssets;

namespace Deal.Data
{

    /// <summary>
    /// 养鸡场
    /// </summary>
    [Serializable]
    public class Data_ChickenFarm : Data_BuildingBase
    {
        public int ChickenTotal = 12;
        public int ChickenNum = 0;

        public override void Load()
        {

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewChickenFarm(this, mapRender.Builds.transform, this.WorldPos);
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
