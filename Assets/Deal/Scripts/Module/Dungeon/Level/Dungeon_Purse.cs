using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Deal.Dungeon
{

    /// <summary>
    /// 宝藏
    /// </summary>
    public class Dungeon_Purse : Dungeon_Treasure
    {

        public override void OnHeroEnter(Hero hero)
        {
            if (this.treasureState == DungeonTreasureStateType.Open) return;
            base.OnHeroEnter(hero);
            this.SetState(DungeonTreasureStateType.Open);
        }
    }

}
