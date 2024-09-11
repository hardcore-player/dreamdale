using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using System.Linq;
using TMPro;

namespace Deal.UI
{
    /// <summary>
    /// 背包列表
    /// </summary>
    public class UIBagList : UIBase
    {
        public TextMeshProUGUI txtNums;

        public Transform pfbGoodsItem;

        private List<CmpGoodsItem> goodsItems = new List<CmpGoodsItem>();

        public override void OnUIAwake()
        {
            base.OnUIAwake();

            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/BtnClose", this.OnCloseClick);
        }

        public override void OnUIStart()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            Dictionary<AssetEnum, int> Assets = userData.Data.Assets;

            int bagItemCount = 0;

            List<KeyValuePair<AssetEnum, int>> list = new List<KeyValuePair<AssetEnum, int>>();

            foreach (var item in Assets)
            {
                if (item.Value > 0 && item.Key != AssetEnum.Gold)
                {
                    list.Add(new KeyValuePair<AssetEnum, int>(item.Key, item.Value));
                }
            }

            int assetCounts = list.Count;

            if (assetCounts < 20)
            {
                bagItemCount = 20;
            }
            else
            {
                bagItemCount = (assetCounts / 4 + 1) * 4;
            }


            for (int i = 0; i < bagItemCount; i++)
            {
                Transform goods = Instantiate(this.pfbGoodsItem, this.pfbGoodsItem.parent);
                CmpGoodsItem item = goods.GetComponent<CmpGoodsItem>();

                if (i < assetCounts)
                {
                    item.SetAsset(list[i].Key, list[i].Value);
                }
                else
                {
                    item.SetEmpty();
                }

                this.goodsItems.Add(item);
            }

            this.pfbGoodsItem.gameObject.SetActive(false);

            this.txtNums.text = $"{userData.BagToatl()}/{userData.Data.Data_Hero.BagTotal}";
        }


    }

}
