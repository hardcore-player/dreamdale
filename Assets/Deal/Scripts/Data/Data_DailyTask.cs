using System;
using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using UnityEngine;

namespace Deal.Data
{
    public enum DailyTaskTypeEnum
    {
        video,
        collect, //采集
        collectFactory, //采集工厂
        action, //挖出宝藏
        land,//
        login,//
        kill, //击杀25个敌人

        //build, //建造
        //sell, //在市场上出售
        //land, //购买新的土地
        //upgrade, //升级
        //tool, //解锁搞头

        //equip, //
        //complete, //完成地下城
        //research, //购买蓝图
        //buy,
        //tower, //宝石塔
        //library, //宝石矿场
        //assign, //安排工人
        //kill, //击杀25个敌人
    }

    /// <summary>
    /// 每日任务
    /// </summary>
    [Serializable]
    public class Data_DailyTask
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

        // 是否领取奖励，点击了领取的按钮 true 领取了  false 没领
        public bool HasReward;

        public AssetEnum RewardType;

        public int RewardNum;

    }
}
