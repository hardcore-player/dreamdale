using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Druid;
using ExcelData;
using Deal;

namespace Deal.UI
{
    class ItemData
    {
        public HallAbilityEnum id;
        public int lv;
    }


    /// <summary>
    /// 大厅界面
    /// </summary>
    public class UIHallPop : UIBase
    {
        public CmpHallLvUpem cmpHallLvUpem;

        public UIButtonAdAsset btnAd;

        public override void OnUIStart()
        {
            Druid.Utils.UIUtils.AddBtnClick(transform, "Content/BtnClose", this.OnCloseClick);

            this.btnAd.SetReward(AssetEnum.Gold, 100, () =>
            {
                this.CloseSelf();
            });

            Data_Task _Task = TaskManager.I.GetTask();

            List<ItemData> list = new List<ItemData>();

            foreach (HallAbilityEnum item in Enum.GetValues(typeof(HallAbilityEnum)))
            {
                HallUpgrade hallUpgrade = ConfigManger.I.GetHallAbilityCfg(item);


                if (_Task.TargetType == item.ToString())
                {
                    list.Add(new ItemData { id = item, lv = 3 });
                }
                else if (_Task.TaskId >= hallUpgrade.unlock - 1)
                {
                    list.Add(new ItemData { id = item, lv = 2 });
                }
                else
                {
                    list.Add(new ItemData { id = item, lv = 1 });
                }
            }


            //if (_Task != null && _Task.IsDone == false && _Task.TaskType == TaskTypeEnum.upgrade.ToString())
            //{
            //    // 升级任务
            //    HallAbilityEnum abilityEnum = (HallAbilityEnum)Enum.Parse(typeof(HallAbilityEnum), _Task.TargetType);

            //    for (int i = 0; i < list.Count; i++)
            //    {
            //        if (list[i].id == abilityEnum)
            //        {
            //            list[i].lv = 3;
            //        }
            //    }
            //}

            list.Sort((a, b) => b.lv - a.lv);

            for (int i = 0; i < list.Count; i++)
            {
                _NewItem(list[i].id);
            }

            cmpHallLvUpem.gameObject.SetActive(false);
        }


        private void _NewItem(HallAbilityEnum item)
        {

            CmpHallLvUpem cmpItem = Instantiate<CmpHallLvUpem>(this.cmpHallLvUpem, this.cmpHallLvUpem.transform.parent);
            cmpItem.gameObject.SetActive(true);
            cmpItem.SetData(item);
        }

    }
}

