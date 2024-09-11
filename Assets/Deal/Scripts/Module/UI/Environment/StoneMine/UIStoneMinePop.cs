using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using ExcelData;
using Deal;

namespace Deal.UI
{
    /// <summary>
    /// 采石场界面
    /// </summary>
    public class UIStoneMinePop : UIBase
    {
        public override void OnUIStart()
        {
            Druid.Utils.UIUtils.AddBtnClick(transform, "ui/BtnUp", this.OnBtnUpClick);

        }

        async void OnBtnUpClick()
        {
            UIFactoryLvUpPop ui = await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIFactoryLvUpPop) as UIFactoryLvUpPop;
            ui.SetData(HallAbilityEnum.StoneMineLevel);
        }
    }
}

