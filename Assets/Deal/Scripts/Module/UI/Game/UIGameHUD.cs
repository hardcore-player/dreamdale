using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;

namespace Deal.UI
{
    public class UIGameHUD : UIBase
    {
        public AssetList assetList;

        public override void OnUIAwake()
        {
            // 监听资产变化
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            userData.OnAssetChange += OnDataChange;

            // Init UI
            this.assetList.SetAssets(userData.Data.Assets);


            //userData.UpdateAsset(AssetEnum.Wood, 100);
            //this.AddDatatableWatch(userData, this.OnDataChange);
        }

        /// <summary>
        /// 数据变化
        /// </summary>
        /// <param name="o"></param>
        public void OnDataChange(AssetEnum assetEnum, int assetNum)
        {
            Debug.Log("OnDataChange ");
            this.assetList.UpdateAssets(assetEnum, assetNum);
        }

    }

}
