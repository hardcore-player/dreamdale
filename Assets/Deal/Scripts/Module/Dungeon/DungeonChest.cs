using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace Deal
{
    public class DungeonChest : MonoBehaviour
    {
        public Animator animator;

        private bool _isOpened = false;
        // 打开的回调
        private Action _openCall;

        public Action OpenCall { get => _openCall; set => _openCall = value; }

        public void OnOpenChest()
        {
            if (this._isOpened == true) return;

            this._isOpened = true;

            animator.Play("ani_taskchest0", 0, 0);

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(0.7f);
            sequence.AppendCallback(() =>
            {
                this.OnEventAnimationOpen();
            });
            sequence.AppendInterval(0.2f);
            sequence.AppendCallback(() =>
            {
                this.OnEventAnimationEnd();
            });
        }

        public void OnEventAnimationOpen()
        {

            //for (int i = 0; i < 4; i++)
            //{
            DealUtils.newDropItem(AssetEnum.Gem, 4, transform.position);
            //}
        }

        public void OnEventAnimationEnd()
        {
            if (this._openCall != null) this._openCall();
            Destroy(this.gameObject);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == TagDefine.Player)
            {
                this.OnOpenChest();
            }
        }

    }
}

