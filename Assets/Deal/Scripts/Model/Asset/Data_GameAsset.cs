using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deal
{
    /// <summary>
    /// 资产定义
    /// </summary>
    [System.Serializable]
    public class Data_GameAsset
    {
        public AssetEnum assetType = AssetEnum.Gold;
        public int assetNum = 0;

        public Data_GameAsset()
        {

        }

        public Data_GameAsset(AssetEnum assetType, int assetNum)
        {
            this.assetType = assetType;
            this.assetNum = assetNum;
        }
    }
}
