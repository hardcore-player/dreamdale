using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Druid;

namespace Deal.UI
{
    public class UIButtonAdAsset : MonoBehaviour
    {
        public Image imgAssetIcon;
        public TextMeshProUGUI txtAssetNum;

        private AssetEnum _assetEnum = AssetEnum.None;
        private int _assetNum = 0;

        public Action _onAdComplete;

        public AdTicketPrice btnAdPice;


        private void Awake()
        {
            //Druid.Utils.UIUtils.AddBtnClick(transform, "", OnAdClick);

            SpriteUtils.SetAssetSprite(this.imgAssetIcon, this._assetEnum);
            this.txtAssetNum.text = $"+{this._assetNum}";


            btnAdPice.SetAdComplete(this.OnReward);
        }

        public void SetReward(AssetEnum asset, int num, Action onAd)
        {
            this._assetEnum = asset;
            this._assetNum = num;
            this._onAdComplete = onAd;

            SpriteUtils.SetAssetSprite(this.imgAssetIcon, asset);
            this.txtAssetNum.text = $"+{num}";
        }

        private void OnReward()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            Hero hero = PlayManager.I.mHero;

            DealUtils.newDropItem(this._assetEnum, this._assetNum, hero.transform.position, true);

            if (this._onAdComplete != null) this._onAdComplete();

            userData.Save();
        }

    }

}
