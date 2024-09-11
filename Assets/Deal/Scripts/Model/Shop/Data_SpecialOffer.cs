using System;
using System.Collections.Generic;
using Deal.Data;
using UnityEngine;

/// <summary>
/// 限时礼包数据
/// </summary>
[Serializable]
public class Data_SpecialOffer : Data_SaveBase
{
    // 任务id
    public int Id;
    // 结束时间
    public long EndSeconds;
}