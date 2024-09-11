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
    public class Building_ConeFactory : BuildingChangeFactory
    {

        /// <summary>
        /// 建筑的广告配置
        /// </summary>
        public override BuildingAdConfig GetAdConfig()
        {
            Data_ConeFactory _Data = this.GetData<Data_ConeFactory>();
            ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.ConeFactory.ToString());

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

