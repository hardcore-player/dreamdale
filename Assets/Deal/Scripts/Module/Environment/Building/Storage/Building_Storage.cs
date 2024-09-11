using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.AddressableAssets;
using Deal.UI;
using Deal.Data;
using TMPro;

namespace Deal.Env
{
    /// <summary>
    /// 储物仓库
    /// </summary>
    public class Building_Storage : BuildingAssetWarehouse
    {
        public AssetList assetList;
        public TextMeshPro txtNums;

        public override void UpdateView()
        {
            Data_Storage data_ = this.GetData<Data_Storage>();
            this.assetList.SetAssets(data_.Assets);

            if (data_.GetAseetCount() == 0)
            {
                this.txtNums.gameObject.SetActive(true);
                this.txtNums.text = "0/" + data_.AssetTotal;
            }
            else
            {
                this.txtNums.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        public override void UpdateAsset(Data_GameAsset asset)
        {
            this.UpdateAsset(asset.assetType, asset.assetNum);
        }

        /// <summary>
        /// 更新资产
        /// </summary>
        /// <param name="asset"></param>
        public override void UpdateAsset(AssetEnum assetEnum, int num)
        {
            this.assetList.UpdateAssets(assetEnum, num);

            if (num == 0)
            {
                Data_Storage data_ = this.GetData<Data_Storage>();

                if (data_.GetAseetCount() == 0)
                {
                    this.txtNums.gameObject.SetActive(true);
                    this.txtNums.text = "0/" + data_.AssetTotal;
                }
                else
                {
                    this.txtNums.gameObject.SetActive(false);
                }
            }
            else
            {
                this.txtNums.gameObject.SetActive(false);
            }


        }

    }
}

