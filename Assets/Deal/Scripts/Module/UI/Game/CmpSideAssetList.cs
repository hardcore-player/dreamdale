using System.Collections;
using System.Collections.Generic;
using Druid;
using UnityEngine;

namespace Deal.UI
{
    /// <summary>
    /// 侧边栏资源列表
    /// </summary>
    public class CmpSideAssetList : MonoBehaviour
    {

        public AssetList assetList;

        /// <summary>
        /// 场景中要显示的列表
        /// </summary>
        private List<AssetEnum> _disAssets = new List<AssetEnum>();

        private void Awake()
        {
            // 监听资产变化
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            userData.OnAssetChange += OnDataChange;

            ///
            /// 当前场景要展示的资源
            /// 
            string cScene = App.I.CurScene.sceneName.ToString();
            ExcelData.Market[] markets = ConfigManger.I.configS.markets;

            for (int i = 0; i < markets.Length; i++)
            {
                if (markets[i].scene == cScene)
                {
                    AssetEnum asset = DealUtils.toAssetEnum(markets[i].name);
                    this._disAssets.Add(asset);
                }
            }

            ///初始化
            foreach (var item in userData.Data.Assets)
            {
                if (this._disAssets.Contains(item.Key))
                {
                    this.assetList.UpdateAssets(item.Key, item.Value);
                }
            }
        }

        private void OnDestroy()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            if (userData != null)
            {
                userData.OnAssetChange -= OnDataChange;
            }
        }

        /// <summary>
        /// 数据变化
        /// </summary>
        /// <param name="o"></param>
        public void OnDataChange(AssetEnum assetEnum, int assetNum)
        {
            if (this._disAssets.Contains(assetEnum))
            {
                this.assetList.UpdateAssets(assetEnum, assetNum);
            }
        }
    }

}

