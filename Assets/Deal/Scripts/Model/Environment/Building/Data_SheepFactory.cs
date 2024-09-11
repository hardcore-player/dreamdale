using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;
using Druid.Utils;

namespace Deal.Data
{

    /// <summary>
    /// 羊毛
    /// </summary>
    [Serializable]
    public class Data_SheepFactory : DataChangeFactory
    {

        public override void Load()
        {
            this.DataBuff();
            this.RefreshState();

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewSheepFactory(this, mapRender.Builds.transform, this.WorldPos);
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