using System;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Env;

namespace Deal.UI
{
    public class UIBagExpansion : UIBase
    {

        public AdTicketPrice btnAdPice;

        public Action OnADComplete;

        public override void OnUIAwake()
        {
            Druid.Utils.UIUtils.AddBtnClick(transform, "Content/Bg/BtnClose", OnCloseClick);

            btnAdPice.SetAdComplete(this.OnReward);
        }


        public void SetAdCallback(Action adCallback)
        {
            this.OnADComplete = adCallback;
        }

        public void OnReward()
        {
            if (OnADComplete != null)
            {
                OnADComplete();
            }

            this.CloseSelf();
        }

    }

}
