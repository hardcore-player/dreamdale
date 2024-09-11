using System.Collections;
using System.Collections.Generic;
using Druid;
using UnityEngine;

namespace Deal.UI
{
    /// <summary>
    /// 顶部资源列表
    /// </summary>
    public class CmpTopAssetList : MonoBehaviour
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

            this._disAssets.Add(AssetEnum.Gold);
            this._disAssets.Add(AssetEnum.Gem);
            this._disAssets.Add(AssetEnum.Scroll);
            this._disAssets.Add(AssetEnum.Ticket);

            this.assetList.UpdateAssets(AssetEnum.Gold, 0);
            this.assetList.UpdateAssets(AssetEnum.Gem, 0);
            this.assetList.UpdateAssets(AssetEnum.Scroll, 0);
            this.assetList.UpdateAssets(AssetEnum.Ticket, 0);

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

