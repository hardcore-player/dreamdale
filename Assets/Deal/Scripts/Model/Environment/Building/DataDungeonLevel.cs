using System;
using System.Collections.Generic;
using UnityEngine;
using Deal.Env;
using Druid;

namespace Deal.Data
{

    /// <summary>
    /// 地下城
    /// </summary>
    [Serializable]
    public class DataDungeonLevel : Data_BuildingBase
    {
        // 第几关
        public int lvId = 0;

        // 地图编号
        public int mapId = -1;

        public bool isTmp = false;

        // 怪，0：没打死 1 打死
        public List<int> enemies = new List<int>();
        public List<int> treasures = new List<int>();


        public void AddNewEnemey()
        {
            this.enemies.Add(0);
        }

        public void EnemeyDie(int monsterId)
        {
            if (monsterId < this.enemies.Count)
            {
                this.enemies[monsterId] = 1;
            }
        }


        public void AddNewTreasure()
        {
            this.treasures.Add(0);
        }

        public void TreasureOpend(int treasureId)
        {
            if (treasureId < this.treasures.Count)
            {
                this.treasures[treasureId] = 1;
            }
        }
    }
}