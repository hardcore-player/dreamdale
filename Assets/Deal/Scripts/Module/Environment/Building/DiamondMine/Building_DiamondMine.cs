using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using UnityEngine.AddressableAssets;
using Deal.UI;
using Deal.Data;
using ExcelData;
using Druid.Utils;
using TMPro;

namespace Deal.Env
{
    /// <summary>
    /// 蓝宝石矿场
    /// </summary>
    public class Building_DiamondMine : Building_Mine
    {

        public GameObject goCd;
        public TextMeshPro txtTime;
        public Image imgTime;
        public SpriteRenderer srMain;

        public override void SetData(Data_SaveBase data)
        {
            base.SetData(data);

            Data_DiamondMine _Data = this.GetData<Data_DiamondMine>();

            SpriteUtils.SetDiamondMineSprite(this.srMain, _Data.BuildingEnum);
        }

        public override bool IsFull()
        {
            Data_DiamondMine data_ = this.GetData<Data_DiamondMine>();
            return data_.IsAseetsFull();
        }

        public override void UpdateView()
        {
            base.UpdateView();
            Data_DiamondMine data_ = this.GetData<Data_DiamondMine>();

            int woodNum = data_.Assets[data_.AssetId];
            CmpAssetItem item = this.assetList.GetAssetItem(data_.AssetId);
            item.UpdateNum(woodNum + "/" + data_.AssetTotal);

            this.UpdateCd();
        }


        private void UpdateCd()
        {
            if (this.IsFull())
            {
                this.goCd.SetActive(false);
            }
            else
            {
                this.goCd.SetActive(true);

                Data_DiamondMine data_ = this.GetData<Data_DiamondMine>();

                long timePassed = TimeUtils.TimeNowMilliseconds() - data_.CDAt;
                long timeNeed = data_.RefreshNeed - timePassed / 1000;

                txtTime.text = TimeUtils.SecondsFormat(timeNeed);

                this.imgTime.fillAmount = timeNeed / data_.RefreshNeed;
            }


        }

        public override void UpdateAsset(AssetEnum assetEnum, int num)
        {
            Data_DiamondMine data_ = this.GetData<Data_DiamondMine>();
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
                Data_DiamondMine data_Sawmill = this.GetData<Data_DiamondMine>();
                this.UpdateAsset(data_Sawmill.AssetId, data_Sawmill.Assets[data_Sawmill.AssetId]);
                this.UpdateCd();
                this.checkAnimation();
            }
        }


    }
}

