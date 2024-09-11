using System;
using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using UnityEngine;

namespace Deal.Data
{
    [Serializable]
    public class Data_SevenDay
    {
        // 第几天
        public int DayId;

        public bool IsOpen;

        // 签到状态 0 可以领取，1，可以领取
        public int SignState;
        // 签到状态
        public AssetEnum RewardType;

        public int RewardNum;

    }
}
