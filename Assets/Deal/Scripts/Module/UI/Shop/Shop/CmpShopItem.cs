using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Deal.UI
{
    public class CmpShopItem : MonoBehaviour
    {
        public string ShopType = "";

        private ExcelData.Shop shopData;

        private void Awake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "", OnBuyClick);
        }

        public virtual void SetData(ExcelData.Shop shopData)
        {
            this.shopData = shopData;
        }


        public virtual void OnBuyClick()
        {
            if (this.shopData == null) return;

            int goodsId = this.shopData.id;
            int price = this.shopData.price;
            string shopType = "shop";

            ShopUtils.payByOs(goodsId, shopType, price);
        }

    }
}