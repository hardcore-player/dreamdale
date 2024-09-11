using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.UI;
using Deal.UI;
using Deal.Data;
using Deal.Tools;
using ExcelData;

namespace Deal.Env
{
    /// <summary>
    /// 面包厂
    /// </summary>
    public class Building_DeadWoodFactory : BuildingChangeFactory
    {

        /// <summary>
        /// 建筑的广告配置
        /// </summary>
        public override BuildingAdConfig GetAdConfig()
        {
            Data_DeadWoodFactory _Data = this.GetData<Data_DeadWoodFactory>();
            ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.DeadWoodFactory.ToString());

            BuildingAdConfig config = new BuildingAdConfig();
            config.refreshSecond = 60;
            config.rewardAsset = _Data.ToAsset;
            config.assetNum = resBuilding.ad;
            config.onAdCallback = null;
            config.onAdRestore = null;
            return config;
        }

    }
}

