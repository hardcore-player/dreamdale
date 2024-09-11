using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;

namespace Deal.Data
{

    /// <summary>
    /// 空地
    /// </summary>
    [Serializable]
    public class Data_SpaceCost : Data_BuildingBase
    {
        public bool ShowInMap = false;

        public override long UniqueId()
        {
            Data_Point center = this.CenterGrid();
            return 1000000000 + center.y * 10000 + center.x;
        }


        public Data_SpaceCost Clone()
        {
            Data_SpaceCost data_ = new Data_SpaceCost();
            List<Data_GameAsset> price = new List<Data_GameAsset>();
            for (int i = 0; i < this.Price.Count; i++)
            {
                price.Add(new Data_GameAsset(this.Price[i].assetType, this.Price[i].assetNum));
            }

            data_.Price = price;
            data_.Pos = this.Pos;
            data_.Size = this.Size;
            data_.StateEnum = this.StateEnum;
            data_.BuildingEnum = this.BuildingEnum;
            data_.UnlockTask = this.UnlockTask;

            return data_;
        }

        public override void Load()
        {
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

            this.ShowInMap = false;

            if (!this.IsShowPrice())
            {
                // 开放了，不显示

                //Debug.Log("不显示");

                this.ShowInMap = false;
            }
            else
            {
                MapRender mapRender = MapManager.I.mapRender;

                this.ShowInMap = true;

                if (mapRender.GetSpaceLand(this) == null)
                {
                    PrefabsUtils.NewSpaceLand(this, mapRender.Resource.transform, this.WorldPos, (res) =>
                    {
                        //res.transform.localScale = new Vector3(this.Size.x / 3, this.Size.y / 3, 0);
                    });
                }
                else
                {
                }

            }
        }


        private bool IsShowPrice()
        {
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);
            // 开过了
            if (this.StateEnum == BuildingStateEnum.Open)
            {
                return false;
            }

            Data_Point StartGrid = this.StartGrid();
            int sx = StartGrid.x;
            int sy = StartGrid.y;

            if (mapData.Data.openTile.Contains(StartGrid))
            {
                // 不显示
                return false;
            }

            // 只有特定任务才显示
            Data_Task _Task = TaskManager.I.GetTask();

            if (this.UnlockTask >= 0)
            {
                if (_Task != null && _Task.TaskId < this.UnlockTask)
                {
                    return false;
                }

            }


            // 下排
            for (int x = 0; x < this.Size.x; x++)
            {
                if (mapData.Data.openTile.Contains(new Data_Point(sx + x, sy - 1)))
                {
                    return true;
                }
            }

            // 上排
            for (int x = 0; x < this.Size.x; x++)
            {
                if (mapData.Data.openTile.Contains(new Data_Point(sx + x, sy + 1 + this.Size.y)))
                {
                    return true;
                }
            }

            // 左排
            for (int y = 0; y < this.Size.y; y++)
            {
                if (mapData.Data.openTile.Contains(new Data_Point(sx - 1, sy + y)))
                {
                    return true;
                }
            }

            // 右排
            for (int y = 0; y < this.Size.y; y++)
            {
                if (mapData.Data.openTile.Contains(new Data_Point(sx + 1 + this.Size.x, sy + y)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}