using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using ExcelData;
using Deal.Data;
using Deal.Env;
using System;

namespace Deal.UI
{
    /// <summary>
    /// 图书馆界面
    /// </summary>
    public class UILibrary : UIBase
    {
        public CmpLibraryItem itemProduct;
        public CmpLibraryItem itemSpeed;
        public CmpLibraryItem itemCapacity;

        public Image Icon;

        private Data_Library _data;
        private int _pageId;

        public override void OnUIAwake()
        {
            for (int i = 0; i < 4; i++)
            {
                Toggle toggle = Druid.Utils.UIUtils.FindCmp<Toggle>(this.transform, "Content/Top/Tog" + i);
                toggle.isOn = i == 0;
                int x = i;
                toggle.onValueChanged.AddListener((bool yes) =>
                {
                    if (yes == true)
                    {
                        this.OnTogChange(x);
                    }

                });
            }

            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Panel/item0/btnSell", this.OnProductionClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Panel/item1/btnSell", this.OnSpeedClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Panel/item2/btnSell", this.OnCapacityClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/BtnClose", this.OnCloseClick);
        }


        public void SetData(Data_Library _Library)
        {
            this._data = _Library;

            this._pageId = 0;
            AssetEnum asset = this.GetPageAsset(this._pageId);
            this.RenderPage(asset);
        }



        private void OnTogChange(int x)
        {
            if (this._pageId == x) return;

            this._pageId = x;
            AssetEnum asset = this.GetPageAsset(this._pageId);
            this.RenderPage(asset);
        }


        private AssetEnum GetPageAsset(int x)
        {
            AssetEnum asset = AssetEnum.DeadWood;

            if (x == 0)
            {
                asset = AssetEnum.Sapphire;
            }
            else if (x == 1)
            {
                asset = AssetEnum.Ruby;

            }
            else if (x == 2)
            {
                asset = AssetEnum.Emerald;

            }
            else if (x == 3)
            {
                asset = AssetEnum.Amethyst;

            }

            return asset;
        }

        private BuildingEnum GetPageBuilding(int x)
        {
            BuildingEnum asset = BuildingEnum.DiamondMine;

            if (x == 0)
            {
                asset = BuildingEnum.DiamondMine;
            }
            else if (x == 1)
            {
                asset = BuildingEnum.RubyMine;

            }
            else if (x == 2)
            {
                asset = BuildingEnum.EmeraldMine;

            }
            else if (x == 3)
            {
                asset = BuildingEnum.AmethystMine;

            }

            return asset;
        }

        private void RenderPage(AssetEnum asset)
        {
            BuildingEnum building = this.GetPageBuilding(this._pageId);
            SpriteUtils.SetDiamondMineSprite(this.Icon, building);

            Data_LibraryAssetsLv _LibraryAssetsLv = this._data.GetLibraryAssetsLv(asset);
            ExcelData.Diamond resBuilding = ConfigManger.I.GetDiamondsCfg(asset.ToString());

            // 产量
            int prductLv = _LibraryAssetsLv.prductLv;

            itemProduct.SetData(prductLv, resBuilding.production, resBuilding.pValue);

            // 速度
            int speedLv = _LibraryAssetsLv.speedLv;
            itemSpeed.SetData(speedLv, resBuilding.speed, resBuilding.sValue);
            // 容量
            int capacityLv = _LibraryAssetsLv.capacityLv;
            itemCapacity.SetData(capacityLv, resBuilding.capacity, resBuilding.cValue);
        }

        /// <summary>
        /// 刷新建筑数据
        /// </summary>
        private void RefreshBuilding()
        {
            AssetEnum asset = this.GetPageAsset(this._pageId);
            BuildingEnum building = this.GetPageBuilding(this._pageId);
            Data_LibraryAssetsLv _LibraryAssetsLv = this._data.GetLibraryAssetsLv(asset);

            Building_DiamondMine _DiamondMine = MapManager.I.GetSingleBuilding(building) as Building_DiamondMine;
            if (_DiamondMine != null)
            {
                Data_DiamondMine data_Diamond = _DiamondMine.GetData<Data_DiamondMine>();
                ExcelData.Diamond resBuilding = ConfigManger.I.GetDiamondsCfg(asset.ToString());

                // 宝石矿场
                data_Diamond.UpdateNumWithLibraryLv(resBuilding, _LibraryAssetsLv);
                _DiamondMine.UpdateView();
            }
        }

        #region Click 
        public void OnProductionClick()
        {

            AssetEnum asset = this.GetPageAsset(this._pageId);
            Data_LibraryAssetsLv _LibraryAssetsLv = this._data.GetLibraryAssetsLv(asset);
            ExcelData.Diamond resBuilding = ConfigManger.I.GetDiamondsCfg(asset.ToString());

            // 产量
            int prductLv = _LibraryAssetsLv.prductLv;
            if (prductLv < 4)
            {
                int prductPrice = resBuilding.production[prductLv - 1];

                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                if (userData.CostAsset(AssetEnum.Scroll, prductPrice))
                {
                    _LibraryAssetsLv.prductLv++;

                    this.RefreshBuilding();

                    TaskManager.I.OnTaskLibrary("DiamondMine", 1);
                    DataManager.I.Save(DataDefine.UserData);
                    DataManager.I.Save(DataDefine.MapData);

                    this.RenderPage(asset);
                }
            }

        }

        public void OnSpeedClick()
        {

            AssetEnum asset = this.GetPageAsset(this._pageId);
            Data_LibraryAssetsLv _LibraryAssetsLv = this._data.GetLibraryAssetsLv(asset);
            ExcelData.Diamond resBuilding = ConfigManger.I.GetDiamondsCfg(asset.ToString());

            // 产量
            int speedLv = _LibraryAssetsLv.speedLv;
            if (speedLv < 4)
            {
                int speedPrice = resBuilding.speed[speedLv - 1];

                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                if (userData.CostAsset(AssetEnum.Scroll, speedPrice))
                {
                    _LibraryAssetsLv.speedLv++;
                    this.RefreshBuilding();

                    TaskManager.I.OnTaskLibrary("DiamondMine", 1);
                    DataManager.I.Save(DataDefine.UserData);
                    DataManager.I.Save(DataDefine.MapData);

                    this.RenderPage(asset);
                }
            }

        }


        public void OnCapacityClick()
        {

            AssetEnum asset = this.GetPageAsset(this._pageId);
            Data_LibraryAssetsLv _LibraryAssetsLv = this._data.GetLibraryAssetsLv(asset);
            ExcelData.Diamond resBuilding = ConfigManger.I.GetDiamondsCfg(asset.ToString());

            // 产量
            int capacityLv = _LibraryAssetsLv.capacityLv;
            if (capacityLv < 4)
            {
                int capacityPrice = resBuilding.capacity[capacityLv - 1];

                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                if (userData.CostAsset(AssetEnum.Scroll, capacityPrice))
                {
                    _LibraryAssetsLv.capacityLv++;
                    this.RefreshBuilding();
                    TaskManager.I.OnTaskLibrary("DiamondMine", 1);

                    DataManager.I.Save(DataDefine.UserData);
                    DataManager.I.Save(DataDefine.MapData);

                    this.RenderPage(asset);
                }
            }

        }
        #endregion Click 

    }
}

