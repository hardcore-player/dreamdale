using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using Deal.UI;
using ExcelData;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Deal.Env
{
    /// <summary>
    /// 南瓜地，开放
    /// </summary>
    public class Building_PumpkinLand : BuildingBase
    {

        public List<Transform> Pumpkins = new List<Transform>();

        /// <summary>
        /// 建筑的广告配置
        /// </summary>
        public override BuildingAdConfig GetAdConfig()
        {
            Data_IronMine _Data = this.GetData<Data_IronMine>();
            ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.Farm.ToString());

            BuildingAdConfig config = new BuildingAdConfig();
            config.refreshSecond = 60;
            config.rewardAsset = AssetEnum.Pumpkin;
            config.assetNum = resBuilding.ad;
            config.onAdCallback = null;
            config.onAdRestore = null;
            return config;
        }

        public override void UpdateView()
        {
            this.InitPumpkins();
        }


        private void InitPumpkins()
        {
            if (this.Data == null)
            {
                return;
            }

            Data_PumpkinLand data_Pumpkin = this.GetData<Data_PumpkinLand>();

            MapRender mapRender = MapManager.I.mapRender;
            for (int i = 0; i < data_Pumpkin.Pumpkis.Count; i++)
            {
                Data_CollectableRes pumpkin = data_Pumpkin.Pumpkis[i];
                PrefabsUtils.NewPumkin(pumpkin, this.transform, Pumpkins[i].position);
            }
        }

    }

}

