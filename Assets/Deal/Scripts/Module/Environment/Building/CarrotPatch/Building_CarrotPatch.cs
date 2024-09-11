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
    public class Building_CarrotPatch : BuildingBase
    {

        public List<Transform> resList = new List<Transform>();

        /// <summary>
        /// 建筑的广告配置
        /// </summary>
        public override BuildingAdConfig GetAdConfig()
        {
            ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.CarrotPatch.ToString());

            BuildingAdConfig config = new BuildingAdConfig();
            config.refreshSecond = 60;
            config.rewardAsset = AssetEnum.Carrot;
            config.assetNum = resBuilding.ad;
            config.onAdCallback = null;
            config.onAdRestore = null;
            return config;
        }

        public override void UpdateView()
        {
            this.InitRes();
        }


        private void InitRes()
        {
            if (this.Data == null)
            {
                return;
            }

            Data_CarrotPatch data_carrot = this.GetData<Data_CarrotPatch>();

            MapRender mapRender = MapManager.I.mapRender;
            for (int i = 0; i < data_carrot.Carrots.Count; i++)
            {
                Data_CollectableRes carrot = data_carrot.Carrots[i];
                PrefabsUtils.NewCarrot(carrot, this.transform, resList[i].position);
            }
        }

    }

}

