using System;
using System.Collections.Generic;
using UnityEngine;

namespace Deal.Msg
{
    [Serializable]
    public class Msg_Response
    {
        public int code;
        public string message;
        public object data;

    }
}


