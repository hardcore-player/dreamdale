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
    /// <summary>
    /// 七日签到按钮
    /// </summary>
    public class ButtonSevenSign : UIBase
    {
        public GameObject iconRed;
        public GameObject goButton;

        private void Awake()
        {
            Druid.Utils.UIUtils.AddBtnClick(goButton.transform, "", this.OnClick);

            this.AddSelfEventListener(EventDefine.EVENT_ACTIVITY_SEVEN_SIGN, this.OnEventSign);

            // 监听变化
            TaskManager.I.OnDataTaskChange += OnDataTaskChange;

            this.RenderUI();
        }

        public override void OnUIDestroy()
        {
            TaskManager.I.OnDataTaskChange -= OnDataTaskChange;
        }


        public void OnDataTaskChange(Data_Task task, bool isCreate)
        {
            this.RenderUI();
        }

        public void OnClick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UISevenday, UILayer.Dialog);
        }

        public void OnEventSign(object data)
        {
            Debug.Log("OnEventSign");
            this.RenderUI();
        }

        public void RenderUI()
        {
            UserData _user = DataManager.I.Get<UserData>(DataDefine.UserData);
            List<Data_SevenDay> sevenDays = _user.Data.SevenSign;
            Data_Task _Task = TaskManager.I.GetTask();
            if (sevenDays == null || sevenDays.Count == 0 || _Task.TaskId < 26)
            {
                this.goButton.SetActive(false);
                return;
            }

            // 开放下一天
            bool hasRewardUnGet = false;
            int getDay = 0;
            for (int i = 0; i < 7; i++)
            {
                Data_SevenDay data = _user.Data.SevenSign[i];
                if (data.IsOpen == true && data.SignState == 0)
                {
                    hasRewardUnGet = true;
                    break;
                }

                if (data.SignState == 1)
                {
                    getDay++;
                }
            }

            this.iconRed.SetActive(hasRewardUnGet);

            if (getDay == 7)
            {
                this.goButton.SetActive(false);
            }
            else
            {
                this.goButton.SetActive(true);
            }
        }
    }

}

