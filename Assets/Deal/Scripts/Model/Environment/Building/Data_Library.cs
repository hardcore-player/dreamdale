using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;

namespace Deal.Data
{
    [Serializable]
    public class Data_LibraryAssetsLv
    {
        public AssetEnum asset;
        public int prductLv;
        public int speedLv;
        public int capacityLv; //容量

        public Data_LibraryAssetsLv()
        {
            this.prductLv = 1;
            this.speedLv = 1;
            this.capacityLv = 1;
        }

        public Data_LibraryAssetsLv(AssetEnum asset)
        {
            this.asset = asset;
            this.prductLv = 1;
            this.speedLv = 1;
            this.capacityLv = 1;
        }
    }

    /// <summary>
    /// 图书馆
    /// </summary>
    [Serializable]
    public class Data_Library : Data_BuildingBase
    {

        public List<Data_LibraryAssetsLv> LvList = new List<Data_LibraryAssetsLv>();

        public Data_LibraryAssetsLv GetLibraryAssetsLv(AssetEnum asset)
        {
            return this.LvList.Find(v => v.asset == asset);
        }

        public override void Load()
        {

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewLibrary(this, mapRender.Builds.transform, this.WorldPos);
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
    }
}