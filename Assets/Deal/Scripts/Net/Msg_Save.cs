using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deal.Msg
{
    [Serializable]
    public class Msg_Save
    {
        public int code;
        public string message;
        public Msg_Data_Save data;

    }

    [Serializable]
    public class Msg_Data_Save
    {
        public string userData;
        public string mapData;
        public string dungeonData;
    }

}


