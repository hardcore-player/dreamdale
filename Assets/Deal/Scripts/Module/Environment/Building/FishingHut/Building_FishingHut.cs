using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.AddressableAssets;
using Deal.UI;
using Deal.Data;
using ExcelData;

namespace Deal.Env
{
    /// <summary>
    /// 钓鱼小屋
    /// </summary>
    public class Building_FishingHut : Building_Mine
    {

        /// <summary>
        /// 建筑的广告配置
        /// </summary>
        public override BuildingAdConfig GetAdConfig()
        {
            Data_FishingHut _Data = this.GetData<Data_FishingHut>();
            ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.FishingHut.ToString());

            BuildingAdConfig config = new BuildingAdConfig();
            config.refreshSecond = 60;
            config.rewardAsset = _Data.AssetId;
            config.assetNum = resBuilding.ad;
            config.onAdCallback = null;
            config.onAdRestore = null;
            return config;
        }


        public override bool IsFull()
        {
            Data_FishingHut data_ = this.GetData<Data_FishingHut>();
            return data_.IsAseetsFull();
        }

        public override void UpdateView()
        {
            base.UpdateView();
            Data_FishingHut data_ = this.GetData<Data_FishingHut>();

            int woodNum = data_.Assets[data_.AssetId];
            CmpAssetItem item = this.assetList.GetAssetItem(data_.AssetId);
            item.UpdateNum(woodNum + "/" + data_.AssetTotal);
        }


        public override void UpdateAsset(AssetEnum assetEnum, int num)
        {
            Data_FishingHut data_ = this.GetData<Data_FishingHut>();
            int woodNum = data_.Assets[data_.AssetId];
            CmpAssetItem item = this.assetList.GetAssetItem(data_.AssetId);
            item.gameObject.SetActive(true);
            item.UpdateNum(num + "/" + data_.AssetTotal);
        }

        public override void OnUpdate()
        {

            base.OnUpdate();

            if (this.Data != null)
            {
                this.Data.Update();
                Data_FishingHut data_Sawmill = this.GetData<Data_FishingHut>();
                this.UpdateAsset(AssetEnum.Fish, data_Sawmill.Assets[AssetEnum.Fish]);
                this.checkAnimation();
            }
        }


    }
}

