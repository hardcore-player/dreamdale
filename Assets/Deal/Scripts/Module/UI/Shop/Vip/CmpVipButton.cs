using System.Collections;
using System.Collections.Generic;
using Deal;
using UnityEngine;
using TMPro;
using Druid;

namespace Deal.UI
{

    public class CmpVipButton : MonoBehaviour
    {
        public int Days = 7;

        public TextMeshProUGUI txtPrice;

        private ExcelData.Shop shopData;

        public void Awake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "", OnBuyClick);

            ExcelData.Shop[] shops = ConfigManger.I.configS.shops;

            Data_Task _Task = TaskManager.I.GetTask();

            for (int i = 0; i < shops.Length; i++)
            {
                if (shops[i].type == "VIP" && shops[i].num[0] == this.Days)
                {
                    this.shopData = shops[i];

                    this.txtPrice.text = "¥" + this.shopData.price;

                    break;
                }
            }
        }

        void OnBuyClick()
        {
            if (this.shopData == null) return;

            int goodsId = this.shopData.id;
            int price = this.shopData.price;
            string shopType = "shop";

            ShopUtils.payByOs(goodsId, shopType, price);

            UIManager.I.Pop(AddressbalePathEnum.PREFAB_UIVipPop);
        }

    }

}
