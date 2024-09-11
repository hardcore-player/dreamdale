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
    /// 炼铁厂
    /// </summary>
    public class Building_IronFactory : BuildingChangeFactory
    {
        /// <summary>
        /// 建筑的广告配置
        /// </summary>
        public override BuildingAdConfig GetAdConfig()
        {
            Data_IronFactory _Data = this.GetData<Data_IronFactory>();
            ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.IronFactory.ToString());

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

