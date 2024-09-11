using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deal.Msg
{
    [Serializable]
    public class Msg_Mailbox
    {
        public int code;
        public string message;

        public Msg_Data_Mailbox[] data;


    }

    [Serializable]
    public class Msg_Data_Mailbox
    {
        public long id;
        public long uid;
        public string title;
        public string content;
        public string created_at;
        public string expire_at;
        public int is_receive; //是否已领取 0未领取 1已领取

        public Msg_Data_Mailbox_Rewards[] rewards;

    }

    [Serializable]
    public class Msg_Data_Mailbox_Rewards
    {
        public int id;
        public string key;
        public int num;


    }

}


