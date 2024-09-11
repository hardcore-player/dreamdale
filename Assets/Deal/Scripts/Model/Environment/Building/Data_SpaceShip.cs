using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;

namespace Deal.Data
{

    /// <summary>
    /// 工坊
    /// </summary>
    [Serializable]
    public class Data_SpaceShip : Data_BuildingBase
    {
        // 正在制造的工具ID
        public int ProcessId = 0;
        //public int ToolPrice = 0;


        public override void Load()
        {

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewSpaceShip(this, mapRender.Builds.transform, this.WorldPos);
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

        public override void Open()
        {
            base.Open();

            this.NewTool();
        }

        public void NewTool()
        {
            ExcelData.SpaceShip[] workshop = ConfigManger.I.configS.spaceShips;
            if (workshop.Length > this.ProcessId)
            {
                ExcelData.SpaceShip cfg = workshop[this.ProcessId];
                this.ProcessId++;

                this.Price.Clear();

                for (int i = 0; i < cfg.assets.Length; i++)
                {
                    Debug.Log("SetAssets======  1= " + DealUtils.toAssetEnum(cfg.assets[i]));
                    this.Price.Add(new Data_GameAsset(DealUtils.toAssetEnum(cfg.assets[i]), cfg.num[i]));
                }
            }
        }
    }
}