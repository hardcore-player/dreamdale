using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Deal.Dungeon
{

    /// <summary>
    /// 宝藏
    /// </summary>
    public class Dungeon_Chest : Dungeon_Treasure
    {
        public Animator animator;


        public override void SetState(DungeonTreasureStateType state)
        {
            this.treasureState = state;
            if (state == 0)
            {
                animator.Play("ani_chest1_clsoe", 0, 0);
            }
            else
            {
                animator.Play("ani_chest1_open", 0, 0);
            }
        }



        public override void OnHeroEnter(Hero hero)
        {
            if (this.treasureState == DungeonTreasureStateType.Open) return;
            base.OnHeroEnter(hero);
            animator.Play("ani_chest1", 0, 0);
        }
    }

}
