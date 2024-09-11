using System.Collections;
using System.Collections.Generic;
using Druid;
using Druid.Utils;
using UnityEngine;
using TMPro;


namespace Deal.UI
{
    /// <summary>
    /// 限时礼包侧边栏
    /// </summary>
    public class CmpHomeSpecialItem : MonoBehaviour
    {
        public TextMeshProUGUI txtTime;

        private Data_SpecialOffer _data;

        private void Start()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "", this.OnItemClick);
        }


        public void SetData(Data_SpecialOffer data)
        {
            this._data = data;
        }

        private void Update()
        {
            if (this._data == null) return;
            long nowSecond = TimeUtils.TimeNowSeconds();

            long timeLeft = this._data.EndSeconds - nowSecond;
            if (timeLeft > 0)
            {
                this.txtTime.text = TimeUtils.SecondsFormat(timeLeft);
            }
            else
            {
                // 到时间了
                this._data = null;

                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                userData.DelSpecialOffer(this._data);
            }
        }


        public void OnItemClick()
        {
            if (this._data == null) return;

            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UISpecialOffer, UILayer.Dialog, new UIParamStruct(this._data));
        }

    }
}

