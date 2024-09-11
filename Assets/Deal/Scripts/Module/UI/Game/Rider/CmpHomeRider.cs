using System.Collections;
using System.Collections.Generic;
using Druid;
using Druid.Utils;
using UnityEngine;
using TMPro;


namespace Deal.UI
{
    /// <summary>
    /// 坐骑侧边栏
    /// </summary>
    public class CmpHomeRider : MonoBehaviour
    {

        public GameObject pfbItem;
        public TextMeshProUGUI txtTime;

        private UserData _userData;


        private void Start()
        {
            this._userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            // 监听变化
            this._userData.OnMapRiderChange += OnMapRiderChange;

            // 数据清理

            double nowSeconds = TimeUtils.TimeNowSeconds();

            if (this._userData.Data.IsInMapRider == true)
            {
                if (this._userData.Data.MapRiderEndSecond == -1)
                {
                    // 无限时间
                }
                else if (nowSeconds > this._userData.Data.MapRiderEndSecond)
                {
                    this._userData.Data.IsInMapRider = false;
                    this._userData.RemoveMapRider();
                }
            }

            _renderUI();

        }



        private void OnDestroy()
        {
            if (this._userData != null)
            {
                this._userData.OnMapRiderChange -= OnMapRiderChange;
            }
        }


        public void OnMapRiderChange()
        {
            _renderUI();
        }

        private void Update()
        {
            if (this._userData.Data.IsInMapRider == true)
            {
                long nowSecond = TimeUtils.TimeNowSeconds();

                if (this._userData.Data.MapRiderEndSecond == -1)
                {
                    this.txtTime.text = "无限";
                    return;
                }

                long timeLeft = (long)(this._userData.Data.MapRiderEndSecond - nowSecond);
                if (timeLeft > 0)
                {
                    this.txtTime.text = TimeUtils.SecondsFormat(timeLeft);
                }
                else
                {
                    // 到时间了
                    this._userData.RemoveMapRider();
                }
            }

        }

        /// <summary>
        ///
        private void _renderUI()
        {
            if (this._userData.Data.IsInMapRider == true)
            {
                this.pfbItem.SetActive(true);
            }
            else
            {
                this.pfbItem.SetActive(false);
            }
        }

    }
}

