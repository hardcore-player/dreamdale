using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;

namespace Deal.Data
{

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Data_Default : Data_BuildingBase
    {


        public override void Load()
        {
            MapRender mapRender = MapManager.I.mapRender;
            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewDefault(this, mapRender.Builds.transform, this.WorldPos);
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