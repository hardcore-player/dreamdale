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
    public class CmpHomePickAxe : MonoBehaviour
    {

        public GameObject pfbItem;
        public TextMeshProUGUI txtTime;

        private UserData _userData;


        private void Start()
        {
            this._userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            // 监听变化
            this._userData.OnGoldPickaxeChange += OnGoldPickaxeChange;

            // 数据清理

            double nowSeconds = TimeUtils.TimeNowSeconds();

            if (this._userData.Data.IsGoldPickaxe == true)
            {
                if (nowSeconds > this._userData.Data.GoldPickaxeEndSecond)
                {
                    this._userData.Data.IsGoldPickaxe = false;
                    this._userData.RemoveGlodPickaxe();
                }
            }

            _renderUI();

        }



        private void OnDestroy()
        {
            if (this._userData != null)
            {
                this._userData.OnGoldPickaxeChange -= OnGoldPickaxeChange;
            }
        }


        public void OnGoldPickaxeChange()
        {
            _renderUI();
        }

        private void Update()
        {
            if (this._userData.Data.IsGoldPickaxe == true)
            {
                long nowSecond = TimeUtils.TimeNowSeconds();

                long timeLeft = (long)(this._userData.Data.GoldPickaxeEndSecond - nowSecond);
                if (timeLeft > 0)
                {
                    this.txtTime.text = TimeUtils.SecondsFormat(timeLeft);
                }
                else
                {
                    // 到时间了
                    this._userData.RemoveGlodPickaxe();
                }
            }

        }

        /// <summary>
        ///
        private void _renderUI()
        {
            if (this._userData.Data.IsGoldPickaxe == true)
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

