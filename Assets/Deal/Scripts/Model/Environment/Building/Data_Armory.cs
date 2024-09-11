using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;

namespace Deal.Data
{

    /// <summary>
    /// 武器库
    /// </summary>
    [Serializable]
    public class Data_Armory : Data_BuildingBase
    {

        // 正在制造的工具ID
        public int EquipId = 0;
        public int ToolPrice = 0;


        public override void Load()
        {
            this.RefreshCommonState();

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewArmory(this, mapRender.Builds.transform, this.WorldPos);
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