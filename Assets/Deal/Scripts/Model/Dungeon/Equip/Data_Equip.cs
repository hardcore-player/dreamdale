using System;
using System.Collections.Generic;
using Druid;
using UnityEngine;

namespace Deal.Data
{
    [Serializable]
    public class Data_Equip : Data_SaveBase
    {
        public int equipId = 1;
        public int equipLv = 1;
        public int equipQuality = 1; // 品质

        public EquipPointEnum point;

        public Data_Equip()
        {

        }

        public Data_Equip(int equipId, int equipLv, int equipQuality)
        {
            this.equipId = equipId;
            this.equipLv = equipLv;
            this.equipQuality = equipQuality;

            ExcelData.Equip equipCfg = ConfigManger.I.GetEquipCfg(equipId);
            EquipPointEnum point = (EquipPointEnum)Enum.Parse(typeof(EquipPointEnum), equipCfg.region);
            this.point = point;
        }


        public bool IsEquipOn()
        {
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);

            // 身上的装备
            Dictionary<EquipPointEnum, Data_Equip> EquipPoints = dungeonData.Data.EquipPoints;

            if (EquipPoints.ContainsKey(this.point))
            {
                Data_Equip onEquip = EquipPoints[this.point];
                if (onEquip != null && onEquip.equipId == this.equipId)
                {
                    return true;
                }
            }

            return false;
        }

    }

}
