using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Deal.UI
{
    public class CmpShopGiftItem : CmpShopItem
    {
        public CmpShopReward pfbReward;
        public TextMeshProUGUI txtName;
        public TextMeshProUGUI txtPrice;
        public Image imgIcon;
        public Image imgBg;
        public Image imgTitleBg;

        public override void SetData(ExcelData.Shop shopData)
        {
            base.SetData(shopData);

            this.txtName.text = shopData.name;
            this.txtPrice.text = "¥" + shopData.price;

            Debug.Log("_getNameColor" + shopData.titleBg);

            if (shopData.titleBg != null && shopData.titleBg != "")
            {
                this.imgTitleBg.color = this._getNameColor(int.Parse(shopData.titleBg));
            }

            //SpriteUtils.SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasShop, this.imgBg, this._getBgName(shopData.id));
            SpriteUtils.SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasShop, this.imgBg, shopData.bg);
            SpriteUtils.SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasShop, this.imgIcon, shopData.icon);

            for (int i = 0; i < shopData.goods.Length; i++)
            {
                int num = shopData.num[i];

                CmpShopReward shopReward = Instantiate(this.pfbReward, this.pfbReward.transform.parent);

                shopReward.SetAsset(DealUtils.toAssetEnum(shopData.goods[i]), num);


                if (shopData.iconBg != null && shopData.iconBg != "")
                {
                    SpriteUtils.SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasShop, shopReward.imgBg, shopData.iconBg);
                }

            }

            this.pfbReward.gameObject.SetActive(false);
        }


        private Color _getNameColor(int i)
        {
            if (i == 1)
            {
                return new Color(42 / 255f, 148 / 255f, 0);
            }
            else if (i == 2)
            {
                return new Color(228 / 255f, 139 / 255f, 0);
            }
            else if (i == 3)
            {
                return new Color(183 / 255f, 13 / 255f, 184 / 255f);
            }
            else
            {
                return new Color(0, 121 / 255f, 197 / 255f);
            }
        }


        private string _getBgName(int id)
        {
            int a = id % 4;
            return "img_shop_bgB_" + (a + 1);
        }
    }
}