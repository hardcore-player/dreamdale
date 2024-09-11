using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Druid;
using Deal.Env;


namespace Deal.UI
{
    /// <summary>
    /// 建筑的广告
    /// </summary>
    public class BuildingAd : MonoBehaviour
    {
        //
        public Image imgAssetIcon;
        public TextMeshProUGUI txtAssetNum;
        public GameObject uiRoot;

        public AdTicketPrice btnAdPice;

        //建筑
        public BuildingBase buildingBase;

        private AssetEnum _assetEnum = AssetEnum.None;
        private int _assetNum = 0;


        // 广告泪却时间
        private float _adCdAt = 0;
        private float _refreshTime = 0;
        private bool _isAdCoolDone = true;

        public Action _onAdComplete;
        public Action _onAdRestore;

        private bool _adInit = false;

        private void Start()
        {
            btnAdPice.SetAdComplete(this.OnReward);

            //Druid.Utils.UIUtils.AddBtnClick(transform, "button", OnAdClick);
        }

        public bool OnUpdateRefreshTime()
        {
            if (this._isAdCoolDone == false)
            {
                if (Druid.Utils.TimeUtils.TimeNowMilliseconds() - this._adCdAt >= this._refreshTime * 1000)
                {
                    if (this._onAdRestore != null) this._onAdRestore();
                    this._isAdCoolDone = true;
                    this.uiRoot.SetActive(true);
                    return true;
                }
            }

            return false;
        }

        private void AdInit()
        {
            // 配置
            if (this._adInit == false && this.buildingBase != null && this.buildingBase.Data != null)
            {
                BuildingAdConfig config = this.buildingBase.GetAdConfig();

                this._refreshTime = config.refreshSecond;
                this._assetEnum = config.rewardAsset;
                this._assetNum = config.assetNum;

                this._onAdComplete = config.onAdCallback;
                this._onAdRestore = config.onAdRestore;

                SpriteUtils.SetAssetSprite(this.imgAssetIcon, this._assetEnum);

                this.txtAssetNum.text = $"+{MathUtils.ToKBM(this._assetNum)}";

                this._adInit = true;
            }
        }

        private void Update()
        {
            this.OnUpdateRefreshTime();

            if (this._adInit == false)
            {
                this.AdInit();
            }
        }


        private void OnReward()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            Hero hero = PlayManager.I.mHero;

            DealUtils.newDropItem(this._assetEnum, this._assetNum, hero.transform.position, true);

            if (this._onAdComplete != null) this._onAdComplete();

            this._adCdAt = Druid.Utils.TimeUtils.TimeNowMilliseconds();
            this._isAdCoolDone = false;
            this.uiRoot.SetActive(false);

            userData.Save();
        }

    }

}
