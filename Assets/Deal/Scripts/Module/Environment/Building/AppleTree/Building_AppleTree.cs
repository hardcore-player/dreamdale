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
    public class Building_AppleTree : BuildingBase
    {


        /// <summary>
        /// 建筑的广告配置
        /// </summary>
        public override BuildingAdConfig GetAdConfig()
        {
            ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.AppleTree.ToString());

            BuildingAdConfig config = new BuildingAdConfig();
            config.refreshSecond = 60;
            config.rewardAsset = AssetEnum.Apple;
            config.assetNum = resBuilding.ad;
            config.onAdCallback = null;
            config.onAdRestore = null;
            return config;
        }

        public override void UpdateView()
        {
            this.InitApples();
        }

        private void InitApples()
        {
            if (this.Data == null)
            {
                return;
            }

            Data_AppleTree data_apple = this.GetData<Data_AppleTree>();

            MapRender mapRender = MapManager.I.mapRender;
            for (int i = 0; i < data_apple.AppleTree.Count; i++)
            {
                Data_CollectableRes pumpkin = data_apple.AppleTree[i];
                PrefabsUtils.NewApplTree(pumpkin, this.transform, data_apple.WorldPos);
            }
        }

    }

}

