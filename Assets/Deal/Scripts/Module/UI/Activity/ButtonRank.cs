using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using Deal.Data;
using TMPro;
using System;

namespace Deal.UI
{
    public class ButtonRank : UIBase
    {
        public GameObject iconRed;
        public GameObject goButton;


        private void Awake()
        {
            Druid.Utils.UIUtils.AddBtnClick(goButton.transform, "", this.OnClick);
            // 监听变化
            TaskManager.I.OnDataTaskChange += OnDataTaskChange;

            this.RenderUI();
        }

        public override void OnUIDestroy()
        {
            TaskManager.I.OnDataTaskChange -= OnDataTaskChange;
        }

        public void OnClick()
        {

            NetUtils.postAttrSave((ok) =>
            {
                if (ok == true)
                {
                    UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIRank, UILayer.Dialog);
                }
            });
            //UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIRank, UILayer.Dialog);
        }

        public void OnEventSign(object data)
        {
            Debug.Log("OnEventSign");
            this.RenderUI();
        }

        public void OnDataTaskChange(Data_Task task, bool isCreate)
        {
            this.RenderUI();
        }

        public void RenderUI()
        {
            UserData _user = DataManager.I.Get<UserData>(DataDefine.UserData);
            List<Data_DailyTask> dailyTask = _user.Data.DailyTasks;

            Debug.Log("UIDailyTask ==" + dailyTask.Count);

            Data_Task _Task = TaskManager.I.GetTask();

            if (_Task.TaskId < 16)
            {
                this.goButton.SetActive(false);
                return;
            }
            else
            {
                this.goButton.SetActive(true);
            }

            //bool hasRewardUnGet = false;
            //for (int i = 0; i < dailyTask.Count; i++)
            //{
            //    Data_DailyTask data = dailyTask[i];
            //    if (data.IsDone == true && data.HasReward == false)
            //    {
            //        hasRewardUnGet = true;
            //        break;
            //    }
            //}

            //this.iconRed.SetActive(hasRewardUnGet);
        }
    }

}

