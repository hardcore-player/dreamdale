using System;
using System.Collections.Generic;
using Druid;
using UnityEngine;
using Deal.Data;
using Druid.Utils;

namespace Deal
{
    /// <summary>
    /// 地图数据
    /// </summary>
    public class MapData : DataTable
    {
        // 存档
        public string SaveFile = "Mary_MapData";
        public MapDataSlot Data;

        public MapData(string tableName) : base(tableName)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Load()
        {
            MapDataSlot Slot = null;
            string jsonStr = SaveLoadManager.LoadJson(SaveFile);
            if (jsonStr != null)
            {
                Slot = LitJsonEx.JsonMapper.ToObject<MapDataSlot>(jsonStr);
                if (Slot != null)
                {
                    Slot.Load(jsonStr);
                }
            }

            if (Slot == null)
            {
                Debug.Log("MapData Load null");
                Slot = new MapDataSlot();
                Slot.DefaultData();
                this.Data = Slot;
            }
            else
            {
                Debug.Log("MapData Load not null" + Slot);

                this.Data = Slot;
            }


            Debug.Log("MapData Load");
        }

        public override void Save()
        {
            Debug.Log("Save MapData");
            this.Data.Save();
            this.Data.SaveSecond = TimeUtils.TimeNowSeconds();
            SaveLoadManager.Save(this.Data, SaveFile);
        }

    }

    /// <summary>
    /// 存档数据
    /// </summary>
    [Serializable]
    public class MapDataSlot
    {
        // 保存时间
        public double SaveSecond = 0;

        //解锁的格子
        [SerializeField]
        public List<Data_Point> openTile = new List<Data_Point>();

        //解锁的资源
        [SerializeField]
        public List<Data_CollectableRes> collectableRes = new List<Data_CollectableRes>();

        //解锁的建筑
        [SerializeField]
        public List<Data_BuildingBase> buildings = new List<Data_BuildingBase>();

        //解锁的土地
        [SerializeField]
        public List<Data_SpaceCost> spaceCosts = new List<Data_SpaceCost>();

        [SerializeField]
        public List<Data_Worker> workers = new List<Data_Worker>();


        public void DefaultData()
        {
        }

        public void Save()
        {

        }


        public void Load(string jsonStr)
        {
            Debug.Log("MapData 转化json to 建筑");
            if (jsonStr != null)
            {
                this.buildings.Clear();
                LitJsonEx.JsonData data = LitJsonEx.JsonMapper.ToObject(jsonStr);

                LitJsonEx.JsonData pa = data["buildings"];
                for (int i = 0; i < pa.Count; i++)
                {
                    LitJsonEx.JsonData b = pa[i];
                    int buildingEnum = (int)b["BuildingEnum"];

                    Data_BuildingBase data_Building = DataUtils.DataBuildingFromJson(buildingEnum, b.ToJson());

                    this.buildings.Add(data_Building);
                }
            }

        }
    }

}

