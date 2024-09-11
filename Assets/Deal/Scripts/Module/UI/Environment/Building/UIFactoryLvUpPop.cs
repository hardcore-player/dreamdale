using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using ExcelData;
using Deal;
using Deal.Data;

namespace Deal.UI
{
    /// <summary>
    /// 市场的弹出界面
    /// </summary>
    public class UIFactoryLvUpPop : UIBase
    {
        public CmpHallLvUpem cmpHallLvUpem;

        public UIButtonAdAsset uiButtonAd;

        public override void OnUIStart()
        {

            Druid.Utils.UIUtils.AddBtnClick(transform, "Mask", this.OnCloseClick);
            Druid.Utils.UIUtils.AddBtnClick(transform, "Content/BtnClose", this.OnCloseClick);

            uiButtonAd.SetReward(AssetEnum.Gold, 100, () =>
            {
                this.OnCloseClick();
            });
        }


        public void SetData(HallAbilityEnum abilityEnum)
        {
            this.cmpHallLvUpem.SetData(abilityEnum);
        }
    }
}

