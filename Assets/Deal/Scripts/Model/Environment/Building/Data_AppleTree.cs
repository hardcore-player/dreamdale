using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;
using UnityEngine.AddressableAssets;

namespace Deal.Data
{

    /// <summary>
    /// 苹果树
    /// </summary>
    [Serializable]
    public class Data_AppleTree : Data_BuildingBase
    {

        // 一颗苹果树
        public List<Data_CollectableRes> AppleTree = new List<Data_CollectableRes>();

        public override void Load()
        {
            Debug.Log("苹果树 Load" + BuildingStateEnum.Open);

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                this.CreateRes();

                PrefabsUtils.NewAppleTreeLand(this, mapRender.Resource.transform, this.WorldPos);
            }
            else if (this.StateEnum == BuildingStateEnum.Building)
            {
                PrefabsUtils.NewAppleTreeUnlock(this, mapRender.Builds.transform, this.WorldPos);
            }
            else
            {
                PrefabsUtils.NewDefaultNone(this, mapRender.Builds.transform, this.WorldPos);
            }
        }

        private void CreateRes()
        {
            // 开放了
            if (this.AppleTree.Count == 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    Data_Point start = this.StartGrid();
                    Data_CollectableRes data = DataUtils.NewDataAppleTree(start.x, start.y);
                    this.AppleTree.Add(data);
                }
            }
        }
    }
}