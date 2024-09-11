using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;


namespace Deal.UI
{
    public class UIVipPop : UIBase
    {
        public override void OnUIAwake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Btn/Day7", OnBuy7Click);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Btn/Day30", OnBuy30Click);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Btn/Day365", OnBuy356Click);

        }


        public void OnBuy7Click()
        {

        }


        public void OnBuy30Click()
        {

        }


        public void OnBuy356Click()
        {

        }

    }

}


