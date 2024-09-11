using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using Druid;
using Druid.Utils;
using UnityEngine;


namespace Deal.Dungeon
{


    /// <summary>
    /// 传送门
    /// </summary>
    public class Dungeon_Teleport : MonoBehaviour
    {


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == TagDefine.Player)
            {
                this.OnOpen();
            }
        }


        public void OnOpen()
        {

            DungeonData dungeon = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

            Data_DungeonStage dataStage = dungeon.Data.DataStage;
            double dungeonLv = dataStage.DungeonBuilding;

            // 找到传送门
            for (int i = 0; i < mapData.Data.buildings.Count; i++)
            {
                Data_BuildingBase buildingBase = mapData.Data.buildings[i];
                if (buildingBase.UniqueId() == dungeonLv)
                {
                    DataDungeonLevel dungeonLevel = buildingBase as DataDungeonLevel;
                    dungeonLevel.lvId++;
                    dungeonLevel.enemies.Clear();
                    dungeonLevel.treasures.Clear();

                    dungeonLevel.InCd = true;
                    if (Config.IsDebug())
                    {
                        dungeonLevel.CommonRefreshNeed = 10;
                    }
                    else
                    {
                        dungeonLevel.CommonRefreshNeed = 10 * 60;
                    }
                    dungeonLevel.CommonCDAt = TimeUtils.TimeNowMilliseconds();

                    break;
                }
            }

            TaskManager.I.OnTaskComplete(dungeonLv);

            DataManager.I.Save(DataDefine.DungeonData);
            DataManager.I.Save(DataDefine.MapData);
            DataManager.I.Save(DataDefine.UserData);

            Hero hero = PlayManager.I.mHero;
            hero.Controller.SetStatePause();
            PlayManager.I.LoadGameScene();
        }
    }

}
