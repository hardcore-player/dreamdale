using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;
using Druid.Utils;

namespace Deal.Data
{

    /// <summary>
    /// 木板厂,制砖厂
    /// </summary>
    [Serializable]
    public class DataChangeFactory : Data_BuildingBase
    {
        public AssetEnum FromAsset;
        public AssetEnum ToAsset;

        //木头数量
        public int FromNum = 0;
        public int FromTotal = 0;
        // 木板
        public int ToNum = 0;
        public int ToTotal = 0;

        public float NumBuff = 0;

        public int ChangeNeed = 2;
        public float RefreshNeedBuff = 0;

        // 刷新时间
        public long CDAt = 0;
        // 秒
        public int RefreshNeed = 0;

        // 生产中
        public bool IsStop = true;

        public int GetRefreshNeed()
        {
            return (int)(this.RefreshNeed * (1 - this.RefreshNeedBuff));

        }

        public int GetToTotal()
        {
            return (int)(this.ToTotal * (1 + this.NumBuff));
        }

        public int GetFromTotal()
        {
            return (int)(this.FromTotal * (1 + this.NumBuff));
        }

        //public override void Load()
        //{
        //    this.RefreshState();

        //    MapRender mapRender = MapManager.I.mapRender;

        //    if (this.StateEnum == BuildingStateEnum.Open)
        //    {
        //        PrefabsUtils.NewStoneFactory(this, mapRender.Builds.transform, this.WorldPos);
        //    }
        //    else if (this.StateEnum == BuildingStateEnum.Building)
        //    {
        //        PrefabsUtils.NewDefaultUnlocked(this, mapRender.Builds.transform, this.WorldPos);
        //    }
        //    else
        //    {
        //        PrefabsUtils.NewDefaultNone(this, mapRender.Builds.transform, this.WorldPos);
        //    }
        //}

        public override void Update()
        {
            base.Update();
            this.RefreshState();
        }

        public virtual void RefreshState()
        {
            if (this.StateEnum != BuildingStateEnum.Open)
            {
                return;
            }

            int RefreshNeed = (int)(this.RefreshNeed * (1 - this.RefreshNeedBuff));
            int ToTotal = (int)(this.ToTotal * (1 + this.NumBuff));
            int FromTotal = (int)(this.FromTotal * (1 + this.NumBuff));

            if (this.FromNum <= 0)
            {
                this.IsStop = true;
                return;
            }

            if (this.FromNum < ChangeNeed)
            {
                this.IsStop = true;
                return;
            }

            if (this.ToNum >= ToTotal)
            {
                this.IsStop = true;
                return;
            }

            if (this.IsStop == true)
            {
                this.IsStop = false;
                // 开始时间
                this.CDAt = TimeUtils.TimeNowMilliseconds();
            }

            long timePassed = TimeUtils.TimeNowMilliseconds() - this.CDAt;


            if (timePassed >= RefreshNeed * 1000)
            {
                // 时间是长了几个
                int growed = (int)(timePassed / (RefreshNeed * 1000));

                if (growed > this.FromNum / this.ChangeNeed)
                {
                    growed = this.FromNum / this.ChangeNeed;
                }

                if (growed > ToTotal - this.ToNum)
                {
                    growed = ToTotal - this.ToNum;
                }

                this.ToNum += growed;
                this.FromNum -= growed * this.ChangeNeed;


                if (this.ToNum >= ToTotal || this.FromNum < this.ChangeNeed)
                {
                    this.CDAt = TimeUtils.TimeNowMilliseconds();
                }
                else
                {
                    // 没长满
                    this.CDAt = this.CDAt + growed * RefreshNeed * 1000;
                }
            }

        }

        /// <summary>
        /// 数值加成
        /// </summary>
        public override void DataBuff()
        {
            //工厂容量增加
            float buffVal = MathUtils.GetStatueBuff(StatueEnum.Elemental);
            this.NumBuff = buffVal;

            //工厂生产时间减少
            float buffVal1 = MathUtils.GetStatueBuff(StatueEnum.Time);
            this.RefreshNeedBuff = buffVal1;
        }
    }
}