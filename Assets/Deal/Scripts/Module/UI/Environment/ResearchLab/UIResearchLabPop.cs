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
    public class UIResearchLabPop : UIBase
    {
        public CmpResearchLabItem pfbItem;

        public override void OnUIStart()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Top/BtnClose", this.OnClose1Click);

            ExcelData.Research[] researchs = ConfigManger.I.configS.researchs;

            for (int i = 0; i < researchs.Length; i++)
            {
                CmpResearchLabItem item = Instantiate(this.pfbItem, this.pfbItem.transform.parent);
                item.gameObject.SetActive(true);

                item.SetData(researchs[i]);
            }

            this.pfbItem.gameObject.SetActive(false);
        }


        /// <summary>
        ///   当前任务为实验室购买蓝图任务，且关闭实验室窗口时任务未完成，则弹出宝石优惠礼包，
        ///   宝石优惠礼包有多个，每个礼包对应相应的蓝图任务，具体配在限时礼包配置表中
        /// </summary>
        public void OnClose1Click()
        {
            Data_Task _Task = TaskManager.I.GetTask();
            if (_Task.TaskType == TaskTypeEnum.research.ToString() && _Task.IsDone == false)
            {
                if (!ShopUtils.hasSpecialOfferType(SpecialOfferEnum.gem))
                {
                    ShopUtils.newSpecialOfferGem(_Task.TaskId);
                }
            }

            this.OnCloseClick();
        }

    }
}

