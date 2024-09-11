using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;
using UnityEngine.AddressableAssets;

namespace Deal.Data
{

    /// <summary>
    /// 南瓜地
    /// </summary>
    [Serializable]
    public class Data_PumpkinLand : Data_BuildingBase
    {

        // 一块南瓜地九个南瓜
        public List<Data_CollectableRes> Pumpkis = new List<Data_CollectableRes>();

        public override void Load()
        {
            Debug.Log("南瓜地 Load" + BuildingStateEnum.Open);

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                this.CreatePumpkinList();

                PrefabsUtils.NewPumpkinLand(this, mapRender.Resource.transform, this.WorldPos);
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

        private void CreatePumpkinList()
        {
            // 开放了
            if (this.Pumpkis.Count == 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    int x = i % 3;
                    int y = i / 3;

                    Data_Point start = this.StartGrid();
                    Data_CollectableRes data = DataUtils.NewDataPumpkin(start.x + x, start.y + y);
                    this.Pumpkis.Add(data);
                }
            }
        }
    }
}