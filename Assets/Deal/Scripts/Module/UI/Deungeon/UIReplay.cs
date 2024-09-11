using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using TMPro;

namespace Deal.UI
{
    public class UIReplay : UIBase
    {
        public TextMeshProUGUI txtTime;
        public Image imgTimeBar;

        private float _cdInterval = 5;

        private Action _onViedo;
        private Action _onClose;

        public AdTicketPrice btnAdPice;

        public override void OnUIAwake()
        {
            //Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/UIBuildAD", this.OnVideoClick);

            btnAdPice.SetAdComplete(this.OnVideoClick);
        }

        public void SetCallback(Action videoCall, Action closeCall)
        {
            this._onViedo = videoCall;
            this._onClose = closeCall;
        }

        public void OnVideoClick()
        {
            if (this._onViedo != null) this._onViedo();
            this.CloseSelf();
        }

        public void OnTimeClose()
        {
            if (this._onClose != null) this._onClose();
            this.CloseSelf();
        }

        private void Update()
        {
            if (this._cdInterval <= 0) return;

            this._cdInterval -= Time.deltaTime;

            this.imgTimeBar.fillAmount = (this._cdInterval / 5f);
            this.txtTime.text = ((int)(this._cdInterval) + 1) + "";
            if (this._cdInterval <= 0)
            {
                OnTimeClose();
            }
        }
    }
}

