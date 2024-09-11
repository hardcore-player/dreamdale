using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Druid;

namespace Deal.UI
{
    public class CmpAssetPrice : MonoBehaviour
    {
        public Image imgIcon;
        public TextMeshProUGUI txtNum;

        public AssetEnum assetEnum;
        public int assetNum;

        private void Awake()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            userData.OnAssetChange += OnDataChange;

            this.SetAsset(this.assetEnum, this.assetNum);
        }

        private void OnDestroy()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            if (userData != null)
            {
                userData.OnAssetChange -= OnDataChange;

            }

        }

        public void SetAsset(AssetEnum asset, int num)
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            this.assetEnum = asset;
            this.assetNum = num;

            SpriteUtils.SetAssetSprite(imgIcon, asset);


            int tatal = userData.GetAssetNum(asset);

            this._updatePrice(this.assetNum, tatal);
        }


        public void OnDataChange(AssetEnum assetEnum, int assetNum)
        {
            if (assetEnum == this.assetEnum)
            {

                this._updatePrice(this.assetNum, assetNum);
            }

        }


        private void _updatePrice(int price, int total)
        {
            this.txtNum.text = price + "";

            if (total >= price)
            {
                this.txtNum.color = new Color(1, 1, 1);
            }
            else
            {
                this.txtNum.color = new Color(255 / 255f, 53 / 255f, 53 / 255f);
            }
        }
    }
}

