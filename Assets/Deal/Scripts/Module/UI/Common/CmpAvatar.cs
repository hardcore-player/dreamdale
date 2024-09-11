using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Druid;
using Deal.Data;

namespace Deal.UI
{

    public class CmpAvatar : MonoBehaviour
    {
        public Text txtName;
        public RawImage imgAvatar;

        public void SetInfo(string uName, string url)
        {
            this.txtName.text = uName;
            WXManager.I.setFont(this.txtName);

            Druid.Utils.GameUtils.SetUrlImage(url, this.imgAvatar);
        }


        public void SetSelfInfo()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            this.SetInfo(_Userinfo.NickName, _Userinfo.AvatarUrl);
        }




    }
}
