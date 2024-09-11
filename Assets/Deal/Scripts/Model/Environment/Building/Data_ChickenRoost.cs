using System;
using System.Collections.Generic;
using UnityEngine;
using Druid.Utils;
using UnityEngine.AddressableAssets;

namespace Deal.Data
{

    [Serializable]
    public class Data_ChickenRoostItem
    {
        public CollectableResState State = CollectableResState.None;
        // 刷新时间
        public long CDAt = 0;  // 鸡开始下蛋的时间
        public int ChikcId = -1; //  鸡的id

        public bool HasEgg = false; //是否有鸡蛋
    }

    /// <summary>
    /// 鸡舍
    /// </summary>
    [Serializable]
    public class Data_ChickenRoost : Data_BuildingBase
    {

        public List<Data_ChickenRoostItem> RoostItem = new List<Data_ChickenRoostItem>();

        // 下蛋时间
        public int RefreshNeed = 0;


        public override void Load()
        {
            this.DataBuff();
            this.RefreshState();

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                if (this.BuildingEnum == BuildingEnum.ChickenRoost)
                {
                    PrefabsUtils.NewChickenFactory(this, mapRender.Builds.transform, this.WorldPos);
                }
                else if (this.BuildingEnum == BuildingEnum.ChickenRoostTD)
                {
                    PrefabsUtils.NewChickenFactoryTD(this, mapRender.Builds.transform, this.WorldPos);
                }
            }
            else if (this.StateEnum == BuildingStateEnum.Building)
            {
                PrefabsUtils.NewDefaultUnlocked(this, mapRender.Builds.transform, this.WorldPos);
            }
            else
            {
                PrefabsUtils.NewDefaultNone(this, mapRender.Builds.transform, this.WorldPos);
            }
        }

        //public override void Update()
        //{
        //    base.Update();
        //    this.RefreshState();
        //}



        /// <summary>
        /// 采集完，才会长出新的
        /// </summary>
        private void RefreshState()
        {
            for (int i = 0; i < this.RoostItem.Count; i++)
            {
                Data_ChickenRoostItem item = this.RoostItem[i];

                if (item.State == CollectableResState.CD)
                {
                    if (TimeUtils.TimeNowMilliseconds() - item.CDAt > this.RefreshNeed * 1000)
                    {
                        item.State = CollectableResState.DONE;
                    }
                }
            }
        }

        /// <summary>
        /// 单个刷新
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public float RefreshStateId(int id)
        {

            Data_ChickenRoostItem item = this.RoostItem[id];

            if (item.State == CollectableResState.CD)
            {
                float passed = TimeUtils.TimeNowMilliseconds() - item.CDAt;
                float need = this.RefreshNeed * 1000;

                if (passed > need)
                {
                    item.State = CollectableResState.DONE;
                }



                return passed / need;
            }

            return 0f;
        }


    }

}
