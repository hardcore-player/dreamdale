using System;
using System.Collections.Generic;
using UnityEngine;
using Druid.Utils;
using UnityEngine.AddressableAssets;

namespace Deal.Data
{



    /// <summary>
    /// 喷泉
    /// </summary>
    [Serializable]
    public class Data_Fountain : Data_BuildingBase
    {

        public override void Load()
        {
            MapRender mapRender = MapManager.I.mapRender;
            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewFountain(this, mapRender.Builds.transform, this.WorldPos);
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
