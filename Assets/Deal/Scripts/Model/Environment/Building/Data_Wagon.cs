using System;
using System.Collections.Generic;
using UnityEngine;
using Druid.Utils;
using UnityEngine.AddressableAssets;

namespace Deal.Data
{



    /// <summary>
    /// 资源马车
    /// </summary>
    [Serializable]
    public class Data_Wagon : Data_BuildingBase
    {

        // 资源类型
        public AssetEnum AssetId = AssetEnum.Gold;
        // 资源总数
        public int AssetTotal = 6;
        // 刷新时间
        public long CDAt = 0;
        // 秒
        public int RefreshNeed = 0;


        public override void Load()
        {
            this.RefreshState();

            MapRender mapRender = MapManager.I.mapRender;

            Transform parent = mapRender.Builds.transform;

            PrefabsUtils.NewWagon(this, parent, this.WorldPos);
        }

        public override void Update()
        {
            base.Update();
            this.RefreshState();
        }

        public void SetCDState()
        {
            this.StateEnum = BuildingStateEnum.Building;
            this.CDAt = TimeUtils.TimeNowMilliseconds();
        }

        /// <summary>
        /// 采集完，才会长出新的
        /// </summary>
        private void RefreshState()
        {
            if (this.StateEnum == BuildingStateEnum.Building)
            {
                if (TimeUtils.TimeNowMilliseconds() - this.CDAt > this.RefreshNeed * 1000)
                {
                    this.StateEnum = BuildingStateEnum.Open;
                }
            }
        }
    }

}
