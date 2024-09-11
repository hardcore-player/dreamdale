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
    /// 苹果树，开放
    /// </summary>
    public class Building_OrangeTree : BuildingBase
    {


        /// <summary>
        /// 建筑的广告配置
        /// </summary>
        public override BuildingAdConfig GetAdConfig()
        {
            ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.OrangeTree.ToString());

            BuildingAdConfig config = new BuildingAdConfig();
            config.refreshSecond = 60;
            config.rewardAsset = AssetEnum.Orange;
            config.assetNum = resBuilding.ad;
            config.onAdCallback = null;
            config.onAdRestore = null;
            return config;
        }

        public override void UpdateView()
        {
            this.InitOranges();
        }

        private void InitOranges()
        {
            if (this.Data == null)
            {
                return;
            }

            Data_OrangeTree data_orange = this.GetData<Data_OrangeTree>();

            MapRender mapRender = MapManager.I.mapRender;
            for (int i = 0; i < data_orange.OrangeTree.Count; i++)
            {
                Data_CollectableRes orange = data_orange.OrangeTree[i];
                PrefabsUtils.NewOrangeTree(orange, this.transform, data_orange.WorldPos);
            }
        }

    }

}

