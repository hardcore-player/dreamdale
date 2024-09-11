using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Druid;

namespace Deal.UI
{
    /// <summary>
    ///  资产展示
    /// </summary>
    public class CmpAssetDisplay : MonoBehaviour
    {
        public Image imgIcon;
        public TextMeshProUGUI txtNum;

        public AssetEnum assetEnum;

        private void Awake()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            userData.OnAssetChange += OnDataChange;

            this.SetAsset(this.assetEnum);
        }

        private void OnDestroy()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            if (userData != null)
            {
                userData.OnAssetChange -= OnDataChange;
            }

        }

        public void SetAsset(AssetEnum asset)
        {
            this.assetEnum = asset;

            SpriteUtils.SetAssetSprite(imgIcon, asset);
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            int tatal = userData.GetAssetNum(asset);
            this._updateNum(tatal);
        }


        public void OnDataChange(AssetEnum assetEnum, int assetNum)
        {
            if (assetEnum == this.assetEnum)
            {
                this._updateNum(assetNum);
            }

        }


        private void _updateNum(int total)
        {
            this.txtNum.text = MathUtils.ToKBM(total) + "";

        }
    }
}

