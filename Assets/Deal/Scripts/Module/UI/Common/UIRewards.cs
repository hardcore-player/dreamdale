using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Druid;

namespace Deal.UI
{
    public class UIRewards : UIBase
    {
        public CmpRewardItem pfbRewarditem;

        public override void OnInit(UIParamStruct param)
        {
            List<Data_GameAsset> rewards = param.param as List<Data_GameAsset>;

            //奖励展示
            for (int i = 0; i < rewards.Count; i++)
            {
                CmpRewardItem item = Instantiate(this.pfbRewarditem, this.pfbRewarditem.transform.parent);
                item.gameObject.SetActive(true);

                item.SetAsset(rewards[i].assetType, rewards[i].assetNum);
            }
        }
    }

}
