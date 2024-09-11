using System;
using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using UnityEngine;

namespace Deal.Data
{
    [Serializable]
    public class Data_Userinfo : Data_SaveBase
    {
        public string AvatarUrl;
        public long IsAuthCreateTime;
        public long UserId;
        public string OpenId;
        public string NickName;
        public string Token;
        public string UserCode; //登录code
        public long CreateTime;
        public int IsAuth;
        public string UUID;
    }
}
