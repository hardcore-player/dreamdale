using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Deal.UI
{
    public enum AssetZeroEnum
    {
        Hide, //不显示
        Zero, //显示0
        OK, //显示OK
    }

    public class AssetList : MonoBehaviour
    {
        public CmpAssetItem cmpAssetItem;

        public Dictionary<AssetEnum, CmpAssetItem> assetsList = new Dictionary<AssetEnum, CmpAssetItem>();

        public AssetZeroEnum ZeroEnum = AssetZeroEnum.OK;

        private void Awake()
        {
            this.cmpAssetItem.gameObject.SetActive(false);
        }


        public CmpAssetItem GetAssetItem(AssetEnum asset)
        {
            if (!assetsList.ContainsKey(asset))
            {
                this.NewAsset(asset, 0);
            }

            return assetsList[asset];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assets"></param>
        public void Clear()
        {
            foreach (var item in assetsList)
            {
                //item.Value.gameObject.SetActive(false);

                Destroy(item.Value.gameObject);
            }
            assetsList.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assets"></param>
        public void SetAssets(List<Data_GameAsset> assets)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                //if (assets[i].assetNum > 0)
                //{
                Debug.Log("SetAssets======  assets.Count " + i + "   " + assets[i].assetType);
                this.NewAsset(assets[i].assetType, assets[i].assetNum);
                //}
            }

            cmpAssetItem.gameObject.SetActive(false);
        }

        public void SetAssets(Dictionary<AssetEnum, int> assets)
        {
            foreach (var item in assets)
            {
                //if (item.Value > 0)
                //{
                this.UpdateAssets(item.Key, item.Value);
                //}
            }

            cmpAssetItem.gameObject.SetActive(false);
        }



        public void UpdateAssets(Data_GameAsset asset)
        {
            this.UpdateAssets(asset.assetType, asset.assetNum);
        }


        public void UpdateAssets(AssetEnum assetEnum, int assetNum)
        {
            if (!assetsList.ContainsKey(assetEnum))
            {
                this.NewAsset(assetEnum, assetNum);
            }

            assetsList[assetEnum].UpdateNum(assetNum);

            if (this.ZeroEnum == AssetZeroEnum.Hide)
            {
                // 隐藏
                if (assetNum > 0)
                {
                    assetsList[assetEnum].gameObject.SetActive(true);
                }
                else
                {
                    assetsList[assetEnum].gameObject.SetActive(false);
                }
            }
            else if (this.ZeroEnum == AssetZeroEnum.Zero)
            {
                assetsList[assetEnum].gameObject.SetActive(true);
            }
            else if (this.ZeroEnum == AssetZeroEnum.OK)
            {
                if (assetNum > 0)
                {
                    assetsList[assetEnum].SetUnCompoleted();
                }
                else
                {
                    assetsList[assetEnum].SetCompoleted();
                }
            }
        }



        private void NewAsset(AssetEnum assetEnum, int assetNum)
        {

            if (!assetsList.ContainsKey(assetEnum))
            {
                CmpAssetItem item = Instantiate<CmpAssetItem>(this.cmpAssetItem, this.cmpAssetItem.transform.parent);
                item.gameObject.SetActive(true);
                item.SetAsset(assetEnum, assetNum);
                assetsList.Add(assetEnum, item);

                this.UpdateAssets(assetEnum, assetNum);
            }
            else
            {
                CmpAssetItem item = assetsList[assetEnum];
                item.gameObject.SetActive(true);
                item.SetAsset(assetEnum, assetNum);
                assetsList.Add(assetEnum, item);

                this.UpdateAssets(assetEnum, assetNum);
            }
        }

    }

}
