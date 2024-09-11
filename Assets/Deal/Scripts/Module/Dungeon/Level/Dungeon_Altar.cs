using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Deal.Dungeon
{

    /// <summary>
    /// 宝藏
    /// </summary>
    public class Dungeon_Altar : Dungeon_Treasure
    {
        public Animator animator;


        public override void SetState(DungeonTreasureStateType state)
        {
            this.treasureState = state;

            if (state == DungeonTreasureStateType.Close)
            {
                animator.Play("ani_altar", 0, 0);
            }
            else
            {
                animator.Play("ani_altar_default", 0, 0);
            }
        }

        public override void OnHeroEnter(Hero hero)
        {
            if (this.treasureState == DungeonTreasureStateType.Open) return;
            base.OnHeroEnter(hero);
            this.SetState(DungeonTreasureStateType.Open);
        }

        public override void OnTreasureOpen()
        {
            // 满血
            Hero hero = PlayManager.I.mHero;

            int rHp = (int)(hero.CurAtt.MaxHP - hero.CurAtt.HP);
            hero.CurAtt.HP = hero.CurAtt.MaxHP;
            hero.UpdateHP();

            DealUtils.newRecoverHpNum(rHp, hero);
        }
    }

}
