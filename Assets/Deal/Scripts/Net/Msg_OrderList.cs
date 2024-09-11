using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deal.Msg
{
    /// <summary>
    /// 订单列表
    /// </summary>
    [Serializable]
    public class Msg_OrderList
    {
        public int code;
        public string message;
        public Msg_Data_OrderList[] data;


    }

    [Serializable]
    public class Msg_Data_OrderList
    {
        public string orderno; //订单号

        public int status;//订单状态 0待支付 1已取消 2已支付
        public string store;//商品类型  //specialOffer | shop
        public int goods_id;//商品Id
        public double price;//商品价格
        public int channel;//订单渠道 1H5 2米大师
        public string os;// //系统类型
        public string channel_order_id;////渠道订单号

    }
}


