using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deal.Msg
{
    [Serializable]
    public class Msg_WxLogin
    {
        public int code;
        public string message;
        public Msg_Data_WxLogin data;


    }

    [Serializable]
    public class Msg_Data_WxLogin
    {
        public string avatar_url;
        public long create_time;
        public long id;
        public string openid;
        public string nickname;
        public string token;
        public string uuid;
        public int is_auth; //是否授权信息 0否 1是
    }
}


