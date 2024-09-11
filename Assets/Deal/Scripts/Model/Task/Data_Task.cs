using System;
using System.Collections.Generic;
using Deal.Data;
using UnityEngine;

[Serializable]
public class Data_Task : Data_SaveBase
{
    // 任务id
    public int TaskId;

    public string TaskInfo;
    // 任务类型
    public string TaskType;

    public string TargetType;

    // 进度
    public int CurProgress;

    public int TotalProgress;
    // 是否完成
    public bool IsDone;

    // 奖励类型
    public int Reward;

    // 是否领取奖励，点击了领取的按钮
    public bool HasReward;

    public int Auto;
}