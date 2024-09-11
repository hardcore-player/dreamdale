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
    /// 储物仓库
    /// </summary>
    [Serializable]
    public class Data_Storage : DataResWarehouse
    {


        public override void Load()
        {
            this.DataBuff();

            this.RefreshState();

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewStorage(this, mapRender.Builds.transform, this.WorldPos);
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

        public override void Open()
        {
            base.Open();

            this.CDAt = TimeUtils.TimeNowMilliseconds();

            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);
            Data_Worker _Worker = DataUtils.NewDataWorker();
            _Worker.AssetId = AssetEnum.Wood;
            _Worker.HouseId = this.UniqueId();
            mapData.Data.workers.Add(_Worker);

            _Worker.Load();

        }

        /// <summary>
        /// 采集完，才会长出新的
        /// </summary>
        public override void RefreshState()
        {
            // 离线增加

            //// 差多少满
            //int emepytCount = this.AssetTotal - GetAseetCount();

            //Debug.Log("Data_Storage this.AssetTotal" + this.AssetTotal);
            //Debug.Log("Data_Storage GetAseetCount()" + this.GetAseetCount());


            //if (emepytCount <= 0)
            //{
            //    return;
            //}

            //// 离线时间
            //long offlineSecond = TimeUtils.TimeNowMilliseconds() - this.CDAt;

            //Debug.Log("Data_Storage offlineSecond" + offlineSecond);

            ////每分钟每个工人的资源增加5
            //int perWorkAdds = (int)((int)(offlineSecond / 1000f) / 60 * 5);

            //Debug.Log("Data_Storage perWorkAdds" + perWorkAdds);

            //MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

            //List<Data_Worker> workers = mapData.Data.workers;

            //// 有哪些类型工人在采集
            //Dictionary<AssetEnum, int> nums = new Dictionary<AssetEnum, int>();
            //for (int i = 0; i < workers.Count; i++)
            //{
            //    AssetEnum assetEnum = workers[i].AssetId;
            //    if (assetEnum != AssetEnum.None && assetEnum != AssetEnum.Ticket && assetEnum != AssetEnum.Task)
            //    {
            //        if (!nums.ContainsKey(assetEnum))
            //        {
            //            nums.Add(assetEnum, 0);
            //        }

            //        nums[assetEnum]++;

            //        Debug.Log("assetEnum nums.Count" + assetEnum);
            //    }
            //}

            //Debug.Log("Data_Storage nums.Count" + nums.Count);
            //Debug.Log("Data_Storage emepytCount" + emepytCount);


            //if (emepytCount >= nums.Count * perWorkAdds)
            //{
            //    // 加上也不满
            //    foreach (var item in nums)
            //    {
            //        this.AddAsset(item.Key, perWorkAdds);
            //    }

            //    Debug.Log("Data_Storage 加上也不满");

            //}
            //else
            //{
            //    // 能加满的时候，一个一个加
            //    for (int i = 0; i < perWorkAdds; i++)
            //    {
            //        foreach (var item in nums)
            //        {
            //            if (!this.IsAseetsFull())
            //            {
            //                this.AddAsset(item.Key, 1);
            //            }
            //            else
            //            {
            //                break;
            //            }
            //        }
            //    }
            //}


        }

        public override void Update()
        {
            this.CDAt = TimeUtils.TimeNowMilliseconds();
        }

        /// <summary>
        /// 数值加成
        /// </summary>
        public override void DataBuff()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            float buffVal = MathUtils.GetStatueBuff(StatueEnum.Miner);

            this.AssetTotal = (int)(this.AssetTotal * (1 + buffVal));
        }

    }
}