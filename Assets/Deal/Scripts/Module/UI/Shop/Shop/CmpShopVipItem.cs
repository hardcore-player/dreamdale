using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Druid;
using Druid.Utils;
using System;
using TMPro;

namespace Deal.UI
{
    public class CmpShopVipItem : CmpShopItem
    {
        public GameObject goNormal;
        public GameObject goBuy;
        public TextMeshProUGUI txtTimeEnd;

        private void Awake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Normal/Bg", OnBuyClick);

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            if (userData.Data.IsVip)
            {
                Debug.Log("userData.Data.IsVip true");

                this.goNormal.SetActive(false);
                this.goBuy.SetActive(true);

                DateTime dateTime = TimeUtils.DateTimeFromSeconds(userData.Data.VipEndSecond);
                txtTimeEnd.text = $"{dateTime.Year}年 {dateTime.Month}月 {dateTime.Day}日";
            }
            else
            {
                Debug.Log("userData.Data.IsVip false");

                this.goNormal.SetActive(true);
                this.goBuy.SetActive(false);
            }
        }


        public override void OnBuyClick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIVipPop, UILayer.Dialog);
        }


    }
}