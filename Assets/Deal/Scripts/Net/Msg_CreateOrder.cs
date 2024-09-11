using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deal.Msg
{
    /// <summary>
    /// 创建订单
    /// </summary>
    [Serializable]
    public class Msg_CreateOrder
    {
        public int code;
        public string message;
        public Msg_Data_CreateOrder data;


    }

    [Serializable]
    public class Msg_Data_CreateOrder
    {
        public string order_id;
    }
}


