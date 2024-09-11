using System;
using System.Collections.Generic;
using Druid;
using UnityEngine;
using Deal.UI;
using Druid.Utils;
using Deal.Msg;
using Deal.Data;

namespace Deal
{

    public class ActivityUtils
    {


        public static void CheckSignNewDay()
        {
            UserData _user = DataManager.I.Get<UserData>(DataDefine.UserData);

            ExcelData.SevenDay[] sevenDays = ConfigManger.I.configS.sevenDays;

            // 七日签到
            if (_user.Data.SevenSign == null || _user.Data.SevenSign.Count == 0)
            {
                List<Data_SevenDay> list = new List<Data_SevenDay>();
                for (int i = 0; i < 7; i++)
                {
                    Data_SevenDay data = new Data_SevenDay();
                    data.DayId = i;
                    // 未开放
                    data.IsOpen = false;
                    // 未领取
                    data.SignState = 0;

                    ExcelData.SevenDay dataCfg = sevenDays[i];

                    if (i < 6)
                    {
                        data.RewardType = DealUtils.toAssetEnum(dataCfg.goods);
                        data.RewardNum = dataCfg.num;
                    }

                    if (i == 6)
                    {
                        data.RewardType = AssetEnum.None;
                        data.RewardNum = 1;
                    }

                    list.Add(data);
                }


                _user.Data.SevenSign = list;

            }

            // 开放下一天
            int openDay = -1;
            for (int i = 0; i < 7; i++)
            {
                Data_SevenDay data = _user.Data.SevenSign[i];
                if (data.IsOpen == false)
                {
                    openDay = i;
                    data.IsOpen = true;
                    break;
                }
            }


            EventManager.I.Emit(EventDefine.EVENT_ACTIVITY_SEVEN_SIGN, null);

        }


        /// <summary>
        /// 每日任务
        /// </summary>
        public static void CheckDailyTaskNewDay()
        {
            UserData _user = DataManager.I.Get<UserData>(DataDefine.UserData);

            Debug.Log("_user.Data.DailyTasks.Count" + _user.Data.DailyTasks.Count);
            ExcelData.Daily[] dailys = ConfigManger.I.configS.dailys;

            // 
            if (_user.Data.DailyTasks == null || _user.Data.DailyTasks.Count == 0)
            {
                List<Data_DailyTask> list = new List<Data_DailyTask>();


                for (int i = 0; i < dailys.Length; i++)
                {
                    ExcelData.Daily dailyCfg = dailys[i];

                    Data_DailyTask data = new Data_DailyTask();
                    data.TaskId = i;
                    data.TaskInfo = dailyCfg.desc;
                    data.CurProgress = 0;
                    data.TotalProgress = dailyCfg.req;
                    data.IsDone = false;
                    data.HasReward = false;

                    //AssetEnum asset;
                    //Enum.TryParse<AssetEnum>(dailyCfg.goods, true, out asset);

                    data.RewardType = DealUtils.toAssetEnum(dailyCfg.goods);
                    data.RewardNum = dailyCfg.num;

                    data.TaskType = dailyCfg.type;

                    list.Add(data);

                }
                _user.Data.DailyTasks = list;
            }

            EventManager.I.Emit(EventDefine.EVENT_ACTIVITY_DAILY_TASK, null);
        }


        public static void DoDailyTask(DailyTaskTypeEnum dailyTaskType, int num)
        {
            UserData _user = DataManager.I.Get<UserData>(DataDefine.UserData);

            // 
            if (_user.Data.DailyTasks == null || _user.Data.DailyTasks.Count == 0)
            {
                return;
            }

            List<Data_DailyTask> list = _user.Data.DailyTasks;


            for (int i = 0; i < list.Count; i++)
            {
                Data_DailyTask data = list[i];
                if (data.IsDone == false && data.TaskType == dailyTaskType.ToString())
                {
                    data.CurProgress += num;
                    if (data.CurProgress >= data.TotalProgress)
                    {
                        data.IsDone = true;
                        data.CurProgress = data.TotalProgress;
                        EventManager.I.Emit(EventDefine.EVENT_ACTIVITY_DAILY_TASK, null);
                    }
                }
            }

        }
    }
}
