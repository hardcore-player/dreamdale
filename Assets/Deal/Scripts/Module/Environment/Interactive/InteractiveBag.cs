using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using Deal.UI;
using Druid.Utils;
using UnityEngine;
using Druid;


namespace Deal.Env
{
    /// <summary>
    /// 地上的背包
    /// </summary>
    public class InteractiveBag : InteractiveBase
    {
        public Transform view;

        public CollectableResState State = CollectableResState.DONE;

        public BoxCollider2D _collider2D;

        // 刷新时间
        public long CDAt = 0;
        // 秒
        public int RefreshNeed = 10 * 60;


        public override void OnUIPop()
        {
            if (this._popUI)
            {
                Hero hero = PlayManager.I.mHero;

                UIBagExpansion ui = this._popUI as UIBagExpansion;
                ui.SetAdCallback(() =>
                {
                    this.view.gameObject.SetActive(false);
                    this._collider2D.enabled = false;
                    this.State = CollectableResState.CD;
                    this.CDAt = TimeUtils.TimeNowMilliseconds();

                    DealUtils.newDropItem(AssetEnum.Bag, 50, hero.transform.position, false);
                });
            }
        }


        private void Update()
        {
            if (this.State == CollectableResState.CD)
            {
                if (TimeUtils.TimeNowMilliseconds() - this.CDAt > this.RefreshNeed * 1000)
                {

                    this.State = CollectableResState.DONE;
                    this.view.gameObject.SetActive(true);
                    this._collider2D.enabled = true;

                }
            }
        }
    }

}
