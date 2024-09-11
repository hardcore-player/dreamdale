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
    public class InteractiveRide : InteractiveBase
    {
        public Transform view;

        public CollectableResState State = CollectableResState.DONE;

        public BoxCollider2D _collider2D;

        // 刷新时间
        public long CDAt = 0;
        // 秒
        public int RefreshNeed = 8 * 60;


        public override void OnUIPop()
        {
            if (this._popUI)
            {
                UIRide ui = this._popUI as UIRide;
                ui.SetAdCallback(() =>
                {
                    this.view.gameObject.SetActive(false);
                    this._collider2D.enabled = false;
                    this.State = CollectableResState.CD;
                    this.CDAt = TimeUtils.TimeNowMilliseconds();

                    UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                    userData.UnlockMapRider(8 * 60);
                    //userData.UnlockMapRider(-1);
                    //userData.UnlockGlodAxe(24 * 60 * 60);
                    //userData.UnlockGlodPickAxe(24 * 60 * 60);
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
