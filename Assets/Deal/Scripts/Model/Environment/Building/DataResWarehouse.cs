using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;
using Druid.Utils;

namespace Deal.Data
{
    /// <summary>
    /// 资源工厂厂
    /// </summary>
    [Serializable]
    public class DataResWarehouse : Data_BuildingBase
    {

        //// 资产变化
        //public DelegateAssetChange OnAssetChange;

        // 资源类型
        [SerializeField]
        public Dictionary<AssetEnum, int> Assets = new Dictionary<AssetEnum, int>();

        // 生产资源
        public AssetEnum AssetId = AssetEnum.Wood;
        // 资源总数
        public int AssetTotal = 28;
        // 生产数量
        public int ProductNum = 1;
        // 刷新时间
        public long CDAt = 0;
        // 一个的生产速度（秒）
        public int RefreshNeed = 0;

        // 生产中
        public bool IsStop = true;


        public bool IsAseetsFull()
        {
            int total = 0;
            foreach (AssetEnum item in Assets.Keys)
            {
                total += Assets[item];
            }

            return total >= this.AssetTotal;
        }

        public int GetAseetCount()
        {
            int total = 0;
            foreach (AssetEnum item in Assets.Keys)
            {
                total += Assets[item];
            }

            return total;
        }

        public int AddAsset(AssetEnum assetEnum, int num)
        {
            if (!this.Assets.ContainsKey(assetEnum))
            {
                this.Assets.Add(assetEnum, 0);
            }
            this.Assets[assetEnum] = this.Assets[assetEnum] + num;

            return this.Assets[assetEnum];
        }

        public override void Update()
        {
            base.Update();
            this.RefreshState();
        }


        /// <summary>
        /// 采集完，才会长出新的
        /// </summary>
        public virtual void RefreshState()
        {
            if (this.StateEnum != BuildingStateEnum.Open)
            {
                return;
            }

            if (this.Assets.Count <= 0)
            {
                return;
            }

            if (this.IsAseetsFull())
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
            if (timePassed >= this.RefreshNeed * 1000)
            {
                int growed = (int)(timePassed / (this.RefreshNeed * 1000)) * this.ProductNum;

                this.Assets[this.AssetId] += growed;
                if (this.Assets[this.AssetId] > this.AssetTotal)
                {
                    // 长满了
                    this.Assets[this.AssetId] = this.AssetTotal;

                    this.CDAt = TimeUtils.TimeNowMilliseconds();
                }
                else
                {
                    // 没长满
                    this.CDAt = this.CDAt + growed * this.RefreshNeed * 1000;
                }
            }

        }
    }
}