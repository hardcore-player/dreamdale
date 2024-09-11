using System.Collections;
using System.Collections.Generic;
using Druid;
using Druid.Utils;
using UnityEngine;
using TMPro;


namespace Deal.UI
{
    /// <summary>
    /// 金斧子侧边栏
    /// </summary>
    public class CmpHomeAxe : MonoBehaviour
    {

        public GameObject pfbItem;
        public TextMeshProUGUI txtTime;

        private UserData _userData;


        private void Start()
        {
            this._userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            // 监听变化
            this._userData.OnGoldAxeChange += OnGoldAxeChange;
            // 数据清理
            double nowSeconds = TimeUtils.TimeNowSeconds();

            if (this._userData.Data.IsGoldAxe == true)
            {
                if (nowSeconds > this._userData.Data.GoldAxeEndSecond)
                {
                    this._userData.Data.IsGoldAxe = false;
                    this._userData.RemoveGlodAxe();
                }
            }

            _renderUI();

        }


        private void OnDestroy()
        {
            if (this._userData != null)
            {
                this._userData.OnGoldAxeChange -= OnGoldAxeChange;
            }
        }


        public void OnGoldAxeChange()
        {
            _renderUI();
        }

        private void Update()
        {
            if (this._userData.Data.IsGoldAxe == true)
            {
                long nowSecond = TimeUtils.TimeNowSeconds();

                long timeLeft = (long)(this._userData.Data.GoldAxeEndSecond - nowSecond);
                if (timeLeft > 0)
                {
                    this.txtTime.text = TimeUtils.SecondsFormat(timeLeft);
                }
                else
                {
                    // 到时间了
                    this._userData.RemoveGlodAxe();
                }
            }

        }

        /// <summary>
        ///
        private void _renderUI()
        {
            if (this._userData.Data.IsGoldAxe == true)
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

