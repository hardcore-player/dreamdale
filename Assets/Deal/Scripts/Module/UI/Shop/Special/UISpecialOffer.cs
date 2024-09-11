using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using Druid.Utils;
using TMPro;

namespace Deal.UI
{

    /// <summary>
    /// 限时礼包
    /// </summary>
    public class UISpecialOffer : UIBase
    {
        public TextMeshProUGUI txtTime;
        public TextMeshProUGUI txtTitle;
        //public TextMeshProUGUI txtName;
        public TextMeshProUGUI txtOriPrice;
        public TextMeshProUGUI txtPrice;

        public CmpShopReward pfbReward;


        public Image imgIcon;
        public Image imgBg;

        private Data_SpecialOffer _speciaData;


        //UserData userData;

        public override void OnUIStart()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/BtnClose", this.OnCloseClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/BtnBuy", this.OnBuyClick);

        }


        public override void OnInit(UIParamStruct param)
        {
            base.OnInit(param);

            Data_SpecialOffer shopData = param.param as Data_SpecialOffer;

            this._speciaData = shopData;
            this.SetData(shopData);
        }


        public void SetData(Data_SpecialOffer specialData)
        {

            ExcelData.SpecialOffer shopData = ConfigManger.I.GetSpecialOfferCfg(specialData.Id);

            //this.txtTitle.text = "" + shopData.txtTitle[0];
            this.txtTitle.text = shopData.name;
            this.txtOriPrice.text = "¥" + shopData.origin;
            this.txtPrice.text = "¥" + shopData.price;

            for (int i = 0; i < shopData.goods.Length; i++)
            {
                int num = shopData.num[i];

                CmpShopReward shopReward = Instantiate(this.pfbReward, this.pfbReward.transform.parent);
                shopReward.gameObject.SetActive(true);

                shopReward.SetAsset(DealUtils.toAssetEnum(shopData.goods[i]), num);
            }

            //SpriteUtils.SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasShop, this.imgBg, this._getBgName(shopData.type));
            //SpriteUtils.SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasShop, this.imgIcon, this._getIconName(shopData.type, shopData.num[0]));
        }


        private void Update()
        {
            if (this._speciaData == null) return;
            long nowSecond = TimeUtils.TimeNowSeconds();

            long timeLeft = this._speciaData.EndSeconds - nowSecond;
            if (timeLeft > 0)
            {
                this.txtTime.text = TimeUtils.SecondsFormat(timeLeft);
            }
            else
            {
                // 到时间了
                this._speciaData = null;
                this.CloseSelf();
            }
        }


        protected void OnBuyClick()
        {
            if (this._speciaData == null) return;

            ExcelData.SpecialOffer data = ConfigManger.I.GetSpecialOfferCfg(this._speciaData.Id);

            int goodsId = data.id;
            int price = data.price;
            string shopType = "specialOffer";

            ShopUtils.payByOs(goodsId, shopType, price);


            //UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            //userData.HasSpecialOffer(this._speciaData.Id);
            //userData.DelSpecialOffer(this._speciaData);

            //userData.Save();

            this.CloseSelf();
        }


    }
}
