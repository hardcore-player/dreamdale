using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Data;


namespace Deal.UI
{
    public class UIDailyTask : UIBase
    {
        public CmpDailyTaskItem pfbItem;
        public List<CmpDailyTaskItem> list = new List<CmpDailyTaskItem>();


        public override void OnUIStart()
        {

            UserData _user = DataManager.I.Get<UserData>(DataDefine.UserData);
            List<Data_DailyTask> dailyTasks = _user.Data.DailyTasks;

            if (dailyTasks == null || dailyTasks.Count <= 0)
            {
                return;
            }

            // 硬排序
            List<Data_DailyTask> sortList = new List<Data_DailyTask>();

            for (int i = 0; i < dailyTasks.Count; i++)
            {
                // 完成
                if (dailyTasks[i].IsDone == true && dailyTasks[i].HasReward == false)
                {
                    sortList.Add(dailyTasks[i]);
                }
            }

            for (int i = 0; i < dailyTasks.Count; i++)
            {
                // 进行中
                if (dailyTasks[i].IsDone == false)
                {
                    sortList.Add(dailyTasks[i]);
                }
            }

            for (int i = 0; i < dailyTasks.Count; i++)
            {
                // 已经领取
                if (dailyTasks[i].IsDone == true && dailyTasks[i].HasReward == true)
                {
                    sortList.Add(dailyTasks[i]);
                }
            }



            for (int i = 0; i < sortList.Count; i++)
            {
                CmpDailyTaskItem cmp = Instantiate<CmpDailyTaskItem>(this.pfbItem, this.pfbItem.transform.parent);

                //cmp.transform.parent = this.pfbItem.transform.parent;
                cmp.gameObject.SetActive(true);
                cmp.SetData(sortList[i]);

                this.list.Add(cmp);
            }

            //ResourceRequest rr = Resources.LoadAsync<GameObject>("");
            //rr.completed += (res) => { };
        }
    }

}

