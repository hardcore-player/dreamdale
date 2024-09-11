using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using ExcelData;
using Deal;
using Deal.Data;
using Deal.Env;

namespace Deal.UI
{
    /// <summary>
    /// 房屋界面
    /// </summary>
    public class UIHousePop : UIBase
    {
        public CmpToWorkItem pfbButton;

        private List<CmpToWorkItem> listItems = new List<CmpToWorkItem>();

        private Data_House _dataHouse;

        public override void OnUIAwake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Mask", this.OnCloseClick);

        }

        public override void OnInit(UIParamStruct param)
        {
            this._dataHouse = param.param as Data_House;

            this._initAssets();
            this._renderButtons();
        }

        /// <summary>
        /// 初始化资源列表
        /// </summary>
        private void _initAssets()
        {
            List<AssetEnum> tolist = new List<AssetEnum>();
            tolist.Add(AssetEnum.Wood);
            tolist.Add(AssetEnum.Stone);
            tolist.Add(AssetEnum.Iron);
            tolist.Add(AssetEnum.Pumpkin);
            tolist.Add(AssetEnum.Apple);

            tolist.Add(AssetEnum.WinterWood);
            tolist.Add(AssetEnum.DeadWood);
            tolist.Add(AssetEnum.Cactus);
            tolist.Add(AssetEnum.Orange);
            tolist.Add(AssetEnum.Carrot);


            Data_Task _Task = TaskManager.I.GetTask();
            List<AssetEnum> list = new List<AssetEnum>();

            for (int i = 0; i < tolist.Count; i++)
            {
                ExcelData.Market market = ConfigManger.I.GetMarketCfg(tolist[i].ToString());
                if (_Task.TaskId >= market.task)
                {
                    list.Add(tolist[i]);
                }
            }


            for (int i = 0; i < list.Count; i++)
            {
                CmpToWorkItem item = Instantiate(this.pfbButton, this.pfbButton.transform.parent);
                item.SetData(list[i]);

                this.listItems.Add(item);

                AssetEnum asset = list[i];
                Druid.Utils.UIUtils.AddBtnClick(item.transform, "", () =>
                {
                    this.OnAssetClick(asset);
                });
            }

            this.pfbButton.gameObject.SetActive(false);
        }

        private void _renderButtons()
        {
            Dictionary<AssetEnum, int> nums = new Dictionary<AssetEnum, int>();

            List<Worker> wokers = MapManager.I.mapRender.workers;

            for (int i = 0; i < wokers.Count; i++)
            {
                AssetEnum assetEnum = wokers[i].Data.AssetId;

                if (!nums.ContainsKey(assetEnum))
                {
                    nums.Add(assetEnum, 0);
                }

                nums[assetEnum]++;
            }

            for (int i = 0; i < this.listItems.Count; i++)
            {
                this.listItems[i].SetSelect(this._dataHouse.WorkerType);

                if (nums.ContainsKey(this.listItems[i].asset))
                {
                    this.listItems[i].SetNum(nums[this.listItems[i].asset]);
                }
                else
                {
                    this.listItems[i].SetNum(0);
                }

            }
        }

        /// <summary>
        /// 切换资源
        /// </summary>
        /// <param name="asset"></param>
        void OnAssetClick(AssetEnum asset)
        {
            if (this._dataHouse == null) return;
            if (this._dataHouse.WorkerType == asset) return;

            List<Worker> wokers = MapManager.I.mapRender.workers;

            //最多三个
            int c = 0;
            for (int i = 0; i < wokers.Count; i++)
            {
                if (wokers[i].Data.AssetId == asset)
                {
                    c++;
                }
            }

            if (c >= 3) return;

            // 设置工作种类
            this._dataHouse.WorkerType = asset;

            //找到房子，更新ui
            BuildingBase _House = MapManager.I.mapRender.GetBuilding(this._dataHouse);
            if (_House != null)
            {
                _House.UpdateView();
            }


            //变更工人工作
            for (int i = 0; i < wokers.Count; i++)
            {
                if (wokers[i].Data.HouseId == this._dataHouse.UniqueId())
                {
                    wokers[i].Data.AssetId = this._dataHouse.WorkerType;
                    wokers[i].Data.AssetNum = 0;
                }
            }

            // 更新任务
            TaskManager.I.OnTaskAssign(this._dataHouse.WorkerType.ToString());

            this._renderButtons();

            // 保存
            DataManager.I.Save(DataDefine.UserData);
            DataManager.I.Save(DataDefine.MapData);
        }
    }
}

