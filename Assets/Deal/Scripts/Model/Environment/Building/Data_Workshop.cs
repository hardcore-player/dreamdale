using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;

namespace Deal.Data
{

    /// <summary>
    /// 工坊
    /// </summary>
    [Serializable]
    public class Data_Workshop : Data_BuildingBase
    {
        // 正在制造的工具ID
        public WorkshopToolEnum ToolEnum = WorkshopToolEnum.None;
        public int ToolPrice = 100;

        //武器
        public List<Data_GameAsset> EPrice = new List<Data_GameAsset>();
        public int EquipId = 0;
        public int EquipPrice = 100;

        public override void Load()
        {

            MapRender mapRender = MapManager.I.mapRender;

            if (this.StateEnum == BuildingStateEnum.Open)
            {
                PrefabsUtils.NewWorkshop(this, mapRender.Builds.transform, this.WorldPos);
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


        public override List<Data_GameAsset> GetPrice()
        {
            if (this.ToolEnum != WorkshopToolEnum.None)
            {
                return this.Price;
            }
            else if (this.EquipId > 0)
            {
                return this.EPrice;
            }
            else
            {
                return this.Price;
            }
        }


        public void NewTool(WorkshopToolEnum toolType)
        {
            this.ToolEnum = toolType;
            ExcelData.Workshop workshop = ConfigManger.I.GetWorkshopCfg(toolType.ToString());
            this.Price.Clear();
            this.Price.Add(new Data_GameAsset(AssetEnum.Gold, workshop.gold));
            this.ToolPrice = workshop.gold;
        }


        public void NewEquip(int equipId)
        {
            ExcelData.Equip equip = ConfigManger.I.GetEquipCfg(equipId);
            this.EPrice.Clear();
            this.EquipId = equipId;

            int priceNum = 0;
            for (int i = 0; i < equip.assets.Length; i++)
            {
                AssetEnum asset = (AssetEnum)Enum.Parse(typeof(AssetEnum), equip.assets[i]);
                int assetNum = (int)equip.num[i];
                priceNum += assetNum;
                this.EPrice.Add(new Data_GameAsset(asset, assetNum));

            }
            this.EquipPrice = priceNum;
        }


        // 是否走cd
        public override bool InCommonCd()
        {
            if (this.ToolEnum != WorkshopToolEnum.None)
            {
                return false;
            }

            return this.InCd;
        }
    }
}