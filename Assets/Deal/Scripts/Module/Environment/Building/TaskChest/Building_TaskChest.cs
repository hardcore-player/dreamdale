using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.AddressableAssets;
using Deal.UI;
using DG.Tweening;

namespace Deal.Env
{
    /// <summary>
    /// 宝箱
    /// </summary>
    public class Building_TaskChest : BuildingBase
    {
        public Animator animator;

        private bool _isOpened = false;

        public override void OnUIPop()
        {
            if (this._popUI)
            {
                UITaskChestPop ui = this._popUI as UITaskChestPop;
                ui.SetData((bool video) =>
                {
                    this.OnOpenChest(video);
                });
            }
        }

        public void OnOpenChest(bool video)
        {
            if (this._isOpened == true) return;

            this._isOpened = true;
            animator.Play("ani_taskchest0", 0, 0);

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(0.7f);
            sequence.AppendCallback(() =>
            {
                this.OnEventAnimationOpen(video);
            });
            sequence.AppendInterval(0.2f);
            sequence.AppendCallback(() =>
            {
                this.OnEventAnimationEnd();
            });
        }

        public void OnEventAnimationOpen(bool video)
        {
            int count = video ? 25 * 6 : 25;
            bool big = video ? true : false;

            DealUtils.newDropItem(AssetEnum.Gold, count, transform.position, big);
        }

        public void OnEventAnimationEnd()
        {
            TaskManager.I.taskChest = null;
            Destroy(this.gameObject);
        }
    }
}

