using System;
using System.Collections.Generic;
using UnityEngine;
using Druid.Utils;
using UnityEngine.AddressableAssets;

namespace Deal.Data
{
    [Serializable]
    public enum CollectableResState
    {
        None,
        CD,
        DONE
    }

    /// <summary>
    /// 可收集方式,
    /// </summary>
    public enum CollectableRes_CollectType
    {
        None,
        Touch, //碰到采集
        Idle   // 停下采集
    }

    /// <summary>
    /// 可采集资源等数据结构，要保存用
    /// </summary>
    [Serializable]
    public class Data_CollectableRes : Data_SaveBase
    {
        // 坐标
        public Data_Point Pos;
        // 资源类型
        public AssetEnum AssetId = AssetEnum.Gold;
        // 资源总数
        public int AssetTotal = 6;
        // 资源剩余
        public int AssetLeft = 6;
        // 刷新时间
        public long CDAt = 0;
        // 秒
        public int RefreshNeed = 0;

        public CollectableResState State = CollectableResState.None;

        public CollectableRes_CollectType CollectType = CollectableRes_CollectType.Idle;

        public long UniqueId()
        {
            Data_Point center = this.CenterGrid();
            return 2000000000 + center.y * 10000 + center.x;
        }

        public Data_Point CenterGrid()
        {
            return this.Pos;
        }

        public Data_Point StartGrid()
        {
            return this.Pos;
        }

        public override void Load()
        {
            this.RefreshState();

            MapRender mapRender = MapManager.I.mapRender;

            Transform parent = mapRender.Resource.transform;
            Vector3 position = new Vector3(this.Pos.x + 0.5f, this.Pos.y + 0.5f, 0);

            if (this.AssetId == AssetEnum.Wood)
            {
                PrefabsUtils.NewTree(this, parent, position);
            }
            else if (this.AssetId == AssetEnum.Stone)
            {
                PrefabsUtils.NewStone(this, parent, position);
            }
            else if (this.AssetId == AssetEnum.Pumpkin)
            {
                PrefabsUtils.NewPumkin(this, parent, position);
            }
            else if (this.AssetId == AssetEnum.Stone)
            {
                PrefabsUtils.NewApplTree(this, parent, position);
            }
            else if (this.AssetId == AssetEnum.Treasure)
            {
                PrefabsUtils.NewTreasure(this, parent, position);
            }
            else if (this.AssetId == AssetEnum.Grain)
            {
                PrefabsUtils.NewGrain(this, parent, position);
            }
            else if (this.AssetId == AssetEnum.Iron)
            {
                PrefabsUtils.NewIron(this, parent, position);
            }
            else if (this.AssetId == AssetEnum.Cactus)
            {
                PrefabsUtils.NewCactus(this, parent, position);
            }
            else if (this.AssetId == AssetEnum.WinterWood)
            {
                PrefabsUtils.NewWinterWood(this, parent, position);
            }
            else if (this.AssetId == AssetEnum.DeadWood)
            {
                PrefabsUtils.NewDeadWood(this, parent, position);
            }
            else if (this.AssetId == AssetEnum.Bamboo)
            {
                PrefabsUtils.NewBamboo(this, parent, position);
            }
        }

        public override void Update()
        {
            base.Update();
            this.RefreshState();
        }

        public void SetCDState()
        {
            this.State = CollectableResState.CD;
            this.CDAt = TimeUtils.TimeNowMilliseconds();
        }

        /// <summary>
        /// 采集完，才会长出新的
        /// </summary>
        private void RefreshState()
        {
            if (this.State == CollectableResState.CD)
            {
                if (TimeUtils.TimeNowMilliseconds() - this.CDAt > this.RefreshNeed * 1000)
                {
                    this.AssetLeft = this.AssetTotal;
                    this.State = CollectableResState.DONE;
                }
            }
        }

        public float GetCdProgress()
        {
            float passed = TimeUtils.TimeNowMilliseconds() - this.CDAt;
            float need = this.RefreshNeed * 1000;

            return passed / need;
        }
    }

}
