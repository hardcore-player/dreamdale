using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;

namespace Deal.Data
{

    /// <summary>
    /// 雕塑
    /// </summary>
    [Serializable]
    public class Data_Statue : Data_BuildingBase
    {
        // 雕塑编号
        public StatueEnum StatueID = StatueEnum.None;

        public override void Load()
        {

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewStatue(this, mapRender.Builds.transform, this.WorldPos);
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