using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Deal.Dungeon
{
    public enum DungeonTreasureType
    {
        None,//
        Chest,//宝箱
        Purse,//钱袋
        Box,//钱袋
        Altar,//祭坛
    }

    public enum DungeonTreasureStateType
    {
        Close = 0,//
        Open = 1
    }

    /// <summary>
    /// 宝藏
    /// </summary>
    public class Dungeon_Treasure : MonoBehaviour
    {
        public DungeonTreasureType treasureType = DungeonTreasureType.None;

        private int _treasureId = 0;
        public int TreasureId { get => _treasureId; set => _treasureId = value; }

        protected DungeonTreasureStateType treasureState = DungeonTreasureStateType.Close;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == TagDefine.Player)
            {
                Hero hero = collision.gameObject.GetComponent<Hero>();
                this.OnHeroEnter(hero);
            }
        }

        /// <summary>
        /// 0:close 1:open
        /// </summary>
        /// <param name="state"></param>
        public virtual void SetState(DungeonTreasureStateType state)
        {
            this.treasureState = state;
            this.gameObject.SetActive(state == 0);
        }

        public virtual void OnHeroEnter(Hero hero)
        {
            if (this.treasureState == DungeonTreasureStateType.Open) return;

            this.treasureState = DungeonTreasureStateType.Open;

            hero.OpenTreasure(this);

            this.OnTreasureOpen();

            if (this.treasureType == DungeonTreasureType.Box)
            {
                //hero.PlayWeapon(WorkshopToolEnum.Axe);
            }


        }

        public virtual void OnTreasureOpen()
        {
            if (this.treasureType == DungeonTreasureType.Chest)
            {
                DealUtils.DungeonChestDrop(this.transform.position);
            }
            else
            {
                DealUtils.DungeonBuildingDrop(this.transform.position);
            }
        }
    }

}
