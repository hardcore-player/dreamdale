using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deal.Msg
{
    /// <summary>
    /// 订单列表
    /// </summary>
    [Serializable]
    public class Msg_Version
    {
        public int code;
        public string message;
        public Msg_Data_Version data;


    }

    [Serializable]
    public class Msg_Data_Version
    {

        public string version;
        public string os;
        public bool shop;
        public bool identity; // true 开 false 关


    }
}


