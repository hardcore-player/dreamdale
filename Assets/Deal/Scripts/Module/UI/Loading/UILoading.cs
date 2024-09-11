using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using System.Text;
using Deal.Data;
using Deal.Msg;

namespace Deal.UI
{
    /// <summary>
    /// 初始化的加载
    /// </summary>
    public class UILoading : LoadingBase
    {
        public Slider slider;

        public override void OnProgress(object param)
        {
            base.OnProgress(param);
            Debug.Log("OnProgress ==" + this.MaxProgress);
        }

        private void Update()
        {
            if (this.CurProgress < this.MaxProgress)
            {
                this.CurProgress += 2;

                if (this.CurProgress > this.MaxProgress)
                {
                    this.CurProgress = this.MaxProgress;
                }

                this.slider.value = this.CurProgress / 100;


                if (this.CurProgress == 100)
                {
                    this.EndLogin();

                    this.CurProgress = 1000;
                }
            }
        }

        public void EndLogin()
        {
            App.I.OnLogin();
            TransitionManager.I.RemoveLoading();
        }

    }
}

