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
    /// 采矿厂
    /// </summary>
    public class Building_IronMine : Building_Mine
    {
        /// <summary>
        /// 建筑的广告配置
        /// </summary>
        public override BuildingAdConfig GetAdConfig()
        {
            Data_IronMine _Data = this.GetData<Data_IronMine>();
            ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.IronMine.ToString());

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
            Data_IronMine data_ = this.GetData<Data_IronMine>();
            return data_.IsAseetsFull();
        }

        public override void UpdateView()
        {
            base.UpdateView();
            Data_IronMine data_ = this.GetData<Data_IronMine>();

            int woodNum = data_.Assets[data_.AssetId];
            CmpAssetItem item = this.assetList.GetAssetItem(data_.AssetId);
            item.UpdateNum(woodNum + "/" + data_.AssetTotal);
        }


        public override void UpdateAsset(AssetEnum assetEnum, int num)
        {
            Data_IronMine data_ = this.GetData<Data_IronMine>();
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
                Data_IronMine data_Sawmill = this.GetData<Data_IronMine>();
                this.UpdateAsset(AssetEnum.Iron, data_Sawmill.Assets[AssetEnum.Iron]);
                this.checkAnimation();
            }
        }


    }
}

