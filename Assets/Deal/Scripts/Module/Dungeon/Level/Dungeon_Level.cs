using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Data;

namespace Deal.Dungeon
{
    /// <summary>
    /// 地牢的关卡
    /// </summary>
    public class Dungeon_Level : MonoBehaviour
    {
        // 出生点
        public Transform bornPoint;

        public List<Enemy> enemies = new List<Enemy>();
        public List<Dungeon_Treasure> treasures = new List<Dungeon_Treasure>();

        private void Awake()
        {
            DungeonData dungeon = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);

            Data_DungeonStage dataStage = dungeon.Data.DataStage;
            double dungeonLv = dataStage.DungeonBuilding;
            DataDungeonLevel dataDungeon = dataStage.DataDungeonLevel;

            /// 遍历地图上的信息
            Transform objectCell = this.transform.Find("Object");
            for (int i = 0; i < objectCell.childCount; i++)
            {
                Enemy enemy = objectCell.GetChild(i).GetComponent<Enemy>();
                Dungeon_Treasure treasure = objectCell.GetChild(i).GetComponent<Dungeon_Treasure>();
                if (enemy != null)
                {
                    enemy.MonsterId = this.enemies.Count;
                    this.enemies.Add(enemy);

                }
                else if (treasure != null)
                {
                    treasure.TreasureId = this.treasures.Count;
                    this.treasures.Add(treasure);
                    treasure.SetState(DungeonTreasureStateType.Close);
                }
            }


            if (dataDungeon != null)
            {
                // 新建数据
                if (dataDungeon.enemies.Count == 0)
                {
                    for (int i = 0; i < this.enemies.Count; i++)
                    {
                        dataDungeon.AddNewEnemey();
                    }
                }

                if (dataDungeon.treasures.Count == 0)
                {
                    for (int i = 0; i < this.treasures.Count; i++)
                    {
                        dataDungeon.AddNewTreasure();
                    }
                }


                // 复制数据

                ExcelData.Dungeon dungeonCfg = ConfigManger.I.GetDungeonCfg(dataStage.DataDungeonLevel.lvId);

                for (int i = 0; i < this.enemies.Count; i++)
                {
                    int enemyState = dataDungeon.enemies[i];
                    this.enemies[i].gameObject.SetActive(enemyState == 0);

                    this.enemies[i].SetAttNum(dungeonCfg.num);
                }

                for (int i = 0; i < this.treasures.Count; i++)
                {
                    int treasureState = dataDungeon.treasures[i];
                    this.treasures[i].SetState((DungeonTreasureStateType)treasureState);
                }
            }
            else
            {
                ExcelData.Dungeon dungeonCfg = ConfigManger.I.GetDungeonCfg(0);

                for (int i = 0; i < this.enemies.Count; i++)
                {
                    this.enemies[i].gameObject.SetActive(true);

                    this.enemies[i].SetAttNum(dungeonCfg.num);
                }

                for (int i = 0; i < this.treasures.Count; i++)
                {
                    this.treasures[i].SetState(DungeonTreasureStateType.Close);
                }
            }

            DataManager.I.Save(DataDefine.DungeonData);
            DataManager.I.Save(DataDefine.MapData);

        }
    }
}

