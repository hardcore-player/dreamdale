using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.UI;
using Deal.UI;
using Deal.Data;

namespace Deal.Env
{
    /// <summary>
    /// 传送门
    /// </summary>
    public class Building_PortalBase : BuildingBase
    {
        public override void OnHeroEnter(Hero mHero)
        {
            DataDungeonLevel data_ = this.GetData<DataDungeonLevel>();
            if (data_.InCd == true)
            {
                Debug.Log("OnHeroEnter incd");
                return;
            }

            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            Data_DungeonStage dungeonStage = dungeonData.Data.DataStage;

            dungeonStage.DungeonBuilding = data_.UniqueId();
            dungeonStage.DataDungeonLevel = data_;


            Hero hero = PlayManager.I.mHero;
            hero.Controller.SetStatePause();

            PlayManager.I.enterDungeonPos = hero.transform.position;

            PlayManager.I.LoadBattleScene();
        }

    }
}

