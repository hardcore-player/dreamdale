using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;

namespace Deal.Data
{

    /// <summary>
    /// 临时
    /// </summary>
    [Serializable]
    public class Data_TmpPortal : DataDungeonLevel
    {
        public override void Load()
        {

            MapRender mapRender = MapManager.I.mapRender;

            Debug.Log("Data_TmpPortal" + this.StateEnum);


            if (this.StateEnum == BuildingStateEnum.Open && this.lvId == 0)
            {
                PrefabsUtils.NewTmpPortal(this, mapRender.Builds.transform, this.WorldPos);
            }
            //else if (this.StateEnum == BuildingStateEnum.Building)
            //{
            //PrefabsUtils.NewDefaultUnlocked(this, mapRender.Builds.transform, this.WorldPos);
            //}
            else
            {
                PrefabsUtils.NewDefaultNone(this, mapRender.Builds.transform, this.WorldPos);
            }
        }
    }
}