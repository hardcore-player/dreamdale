using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;

namespace Deal.Data
{

    /// <summary>
    /// 灯塔
    /// </summary>
    [Serializable]
    public class Data_Academy : Data_BuildingBase
    {
        public override void Load()
        {

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewAcademy(this, mapRender.Builds.transform, this.WorldPos);
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