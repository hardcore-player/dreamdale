using System;
using System.Collections.Generic;
using Druid;
using UnityEngine;


namespace Deal.Data
{
    /// <summary>
    /// 工人
    /// </summary>
    [Serializable]
    public class Data_Hero : Data_SaveBase
    {
        // 背包基础容量,捡起来的等
        public int BagBase = 0;
        // 大厅背包能力
        public int BagHall = 0;
        // 背包上限
        public int BagTotal = 20;
        // 斧子能力
        public float AxeVal = 1f;
        // 镐头能力
        public float PickAxeVal = 1f;

        public float FishRodVal = 1f;
        // 镰刀
        public float SickleVal = 1f;
        public float WorkerAbilityVal = 1f;

        public Data_Hero()
        {

        }

        public void DefaultData()
        {
            this.BagBase = 0;
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            float BagLevelValue = userData.GetHallAbilityVal(HallAbilityEnum.BagLevel);
            this.SetBagHall((int)BagLevelValue);

            float AxeLevelValue = userData.GetHallAbilityVal(HallAbilityEnum.AxeLevel);
            this.AxeVal = AxeLevelValue;
            float PickAxeLevel = userData.GetHallAbilityVal(HallAbilityEnum.PickAxeLevel);
            this.PickAxeVal = PickAxeLevel;
            float FishRodLevel = userData.GetHallAbilityVal(HallAbilityEnum.FishingRobLevel);
            this.FishRodVal = FishRodLevel;
            float SickleVal = userData.GetHallAbilityVal(HallAbilityEnum.SickleLevel);
            this.SickleVal = SickleVal;
            float WorkerAbilityVal = userData.GetHallAbilityVal(HallAbilityEnum.WorkerAbilityLevel);
            this.WorkerAbilityVal = WorkerAbilityVal;
        }


        public override void Load()
        {
            //MapRender mapRender = MapManager.I.mapRender;

            //PrefabsUtils.NewWorker(this, mapRender.Resource.transform, new Vector3(0, 0, 0));
        }


        public int GetAxeValInt()
        {
            return (int)AxeVal;
        }


        public void AddBagBase(int val)
        {
            this.BagBase += val;

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            //工厂容量增加
            float buffVal = MathUtils.GetStatueBuff(StatueEnum.Lumber);

            int vipAdd = userData.Data.IsVip ? 1000 : 0;

            this.BagTotal = (int)((this.BagBase + this.BagHall + vipAdd) * (1 + buffVal));
        }

        public void SetBagHall(int val)
        {
            this.BagHall = val;

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            //工厂容量增加
            float buffVal = MathUtils.GetStatueBuff(StatueEnum.Lumber);

            int vipAdd = userData.Data.IsVip ? 1000 : 0;


            this.BagTotal = (int)((this.BagBase + this.BagHall + vipAdd) * (1 + buffVal));
        }



    }

}
