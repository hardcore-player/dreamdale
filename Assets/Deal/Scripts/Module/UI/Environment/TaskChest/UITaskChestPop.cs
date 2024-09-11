using System;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Env;

namespace Deal.UI
{
    public class UITaskChestPop : UIBase
    {
        public AdTicketPrice AdTicket;

        private Action<bool> onOpen;

        public override void OnUIAwake()
        {
            //Druid.Utils.UIUtils.AddBtnClick(transform, "Content/BtnVideoOpen", OnVideoOpenClick);
            Druid.Utils.UIUtils.AddBtnClick(transform, "Content/BtnOpen", OnOpenClick);
            Druid.Utils.UIUtils.AddBtnClick(transform, "Content/Bg/BtnClose", OnCloseClick);

            this.AdTicket.SetAdComplete(this.OnVideoOpenClick);
        }


        public void SetData(Action<bool> onOpen)
        {
            this.onOpen = onOpen;
        }

        public void OnOpenClick()
        {
            if (this.onOpen != null)
            {
                this.onOpen(false);
            }

            this.CloseSelf();
        }

        public void OnVideoOpenClick()
        {
            if (this.onOpen != null)
            {
                this.onOpen(true);
            }

            this.CloseSelf();
        }
    }
}


