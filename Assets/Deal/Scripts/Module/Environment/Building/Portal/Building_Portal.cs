using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.UI;
using Deal.UI;
using Deal.Data;
using Druid.Utils;
using TMPro;

namespace Deal.Env
{
    /// <summary>
    /// 传送门
    /// </summary>
    public class Building_Portal : Building_PortalBase
    {
        public GameObject cdGo;
        public TextMeshProUGUI txtCd;

        public override void UpdateView()
        {
            Data_Portal data_ = this.GetData<Data_Portal>();

            data_.RefreshCommonState();
            if (data_.InCd == true)
            {
                this.cdGo.SetActive(true);
            }
            else
            {
                this.cdGo.SetActive(false);
            }


        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            Data_Portal data_ = this.GetData<Data_Portal>();

            bool cdChangeed = data_.RefreshCommonState();
            if (data_.InCd == true)
            {
                float left = TimeUtils.TimeNowMilliseconds() - data_.CommonCDAt;
                this.txtCd.text = TimeUtils.SecondsFormat(data_.CommonRefreshNeed - (int)left / 1000);
            }

            if (cdChangeed)
            {
                this.UpdateView();
            }

        }

    }
}

