using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Deal.UI
{
    public class CmpShopGoodsItem : CmpShopItem
    {

        public TextMeshProUGUI txtTitle;
        public TextMeshProUGUI txtName;
        public TextMeshProUGUI txtPrice;
        public Image imgIcon;
        public Image imgBg;


        public override void SetData(ExcelData.Shop shopData)
        {
            base.SetData(shopData);

            this.txtTitle.text = "" + shopData.num[0];
            this.txtName.text = shopData.name;
            this.txtPrice.text = "¥" + shopData.price;

            SpriteUtils.SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasShop, this.imgBg, shopData.bg);
            SpriteUtils.SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasShop, this.imgIcon, shopData.icon);


            //SpriteUtils.SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasShop, this.imgBg, this._getBgName(shopData.type));
            //SpriteUtils.SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasShop, this.imgIcon, this._getIconName(shopData.type, shopData.num[0]));
        }



        private string _getBgName(string type)
        {
            if (type == "金币")
            {
                return "img_shop_bgL_1";
            }
            else if (type == "广告券")
            {
                return "img_shop_bgL_2";
            }
            else if (type == "卷轴")
            {
                return "img_shop_bgL_3";
            }
            else if (type == "宝石")
            {
                return "img_shop_bgL_4";
            }

            return "img_shop_bgL_4";
        }


        private string _getIconName(string type, int num)
        {
            if (type == "金币")
            {
                if (num < 10000)
                {
                    return "img_shop_gold1";
                }
                else if (num < 20000)
                {
                    return "img_shop_gold2";
                }
                else if (num < 50000)
                {
                    return "img_shop_gold3";
                }
                else if (num < 100000)
                {
                    return "img_shop_gold4";
                }
                else if (num < 200000)
                {
                    return "img_shop_gold5";
                }
                else
                {
                    return "img_shop_gold6";
                }
            }
            else if (type == "广告券")
            {
                if (num < 20)
                {
                    return "img_shop_adcard1";
                }
                else if (num < 100)
                {
                    return "img_shop_adcard2";
                }
                else
                {
                    return "img_shop_adcard3";
                }
            }
            else if (type == "卷轴")
            {
                if (num < 200)
                {
                    return "img_shop_scroll1";
                }
                else if (num < 500)
                {
                    return "img_shop_scroll2";
                }
                else
                {
                    return "img_shop_scroll3";
                }
            }
            else if (type == "宝石")
            {
                if (num < 1000)
                {
                    return "img_shop_gem1";
                }
                else if (num < 2000)
                {
                    return "img_shop_gem2";
                }
                else
                {
                    return "img_shop_gem3";
                }
            }

            return "img_shop_gold1";
        }
    }
}