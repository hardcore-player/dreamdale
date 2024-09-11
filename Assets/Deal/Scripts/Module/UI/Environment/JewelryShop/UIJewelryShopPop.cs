using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Druid;
using ExcelData;
using Deal;

namespace Deal.UI
{


    /// <summary>
    /// 实验室界面
    /// </summary>
    public class UIJewelryShopPop : UIBase
    {

        public override void OnUIStart()
        {
            //Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Top/BtnClose", this.OnClose1Click);

        }

        /// <summary>
        /// 蓝买红
        /// </summary>
        public void OnBuy1Click()
        {
            UserData user = DataManager.I.Get<UserData>(DataDefine.UserData);

            if (user.CostAsset(AssetEnum.Sapphire, 5))
            {
                user.AddAsset(AssetEnum.Ruby, 1);
                user.Save();
            }
        }

        /// <summary>
        /// 红买绿
        /// </summary>
        public void OnBuy2Click()
        {
            UserData user = DataManager.I.Get<UserData>(DataDefine.UserData);

            if (user.CostAsset(AssetEnum.Ruby, 5))
            {
                user.AddAsset(AssetEnum.Emerald, 1);
                user.Save();
            }
        }


        /// <summary>
        /// 绿买紫
        /// </summary>
        public void OnBuy3Click()
        {
            UserData user = DataManager.I.Get<UserData>(DataDefine.UserData);

            if (user.CostAsset(AssetEnum.Emerald, 5))
            {
                user.AddAsset(AssetEnum.Amethyst, 1);
                user.Save();
            }
        }

    }
}

