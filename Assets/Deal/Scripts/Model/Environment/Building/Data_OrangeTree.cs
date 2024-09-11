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
    public class Data_OrangeTree : Data_BuildingBase
    {

        // 一颗苹果树
        public List<Data_CollectableRes> OrangeTree = new List<Data_CollectableRes>();

        public override void Load()
        {
            Debug.Log("橘子树 Load" + BuildingStateEnum.Open);

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                this.CreateRes();

                PrefabsUtils.NewOrangeTreeLand(this, mapRender.Resource.transform, this.WorldPos);
            }
            else if (this.StateEnum == BuildingStateEnum.Building)
            {
                PrefabsUtils.NewOrangeTreeUnlock(this, mapRender.Builds.transform, this.WorldPos);
            }
            else
            {
                PrefabsUtils.NewDefaultNone(this, mapRender.Builds.transform, this.WorldPos);
            }
        }

        private void CreateRes()
        {
            // 开放了
            if (this.OrangeTree.Count == 0)
            {
                for (int i = 0; i < 1; i++)
                {
                    Data_Point start = this.StartGrid();
                    Data_CollectableRes data = DataUtils.NewDataOrangeTree(start.x, start.y);
                    this.OrangeTree.Add(data);
                }
            }
        }
    }
}