using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Druid;
using Deal.Data;
using TMPro;

namespace Deal
{


    /// <summary>
    /// 掉落道具
    /// </summary>
    public class DropTool : DropPropBase
    {

        public WorkshopToolEnum toolEnum = WorkshopToolEnum.Axe;

        private void Update()
        {
            UpdateBezier();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="assetNum"></param>
        public void SetTool(WorkshopToolEnum toolEnum)
        {
            this.toolEnum = toolEnum;
            this.PropState = DropPropState.None;
            this.box2d.enabled = false;

            SpriteUtils.SetToolSprite1(this.srIcon, toolEnum);
        }

        public void SetEquip(int equipId)
        {
            this.PropState = DropPropState.None;
            this.box2d.enabled = false;

            SpriteUtils.SetEquipIcon(this.srIcon, equipId);
        }



        /// <summary>
        /// 落地以后的逻辑
        /// </summary>
        public override void OnFall2Ground()
        {
            this.PropState = DropPropState.Idle;
            this.box2d.enabled = true;
            //this._dropingInterval = 0;

            SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(2);
            sequence.AppendCallback(() =>
            {
                Hero hero = PlayManager.I.mHero;
                this.Fl2Picker(hero.transform.position);
            });

        }


        /// <summary>
        /// 检查拾取
        /// </summary>
        /// <param name="go"></param>
        private void checkPick(GameObject go)
        {
            if (this.PropState != DropPropState.Idle) return;

            if (go.tag == "Player")
            {
                this.Fl2Picker(go.transform.position);
            }

        }


        void OnTriggerEnter2D(Collider2D collision)
        {
            checkPick(collision.gameObject);
        }

        //void OnTriggerStay2D(Collider2D collision)
        //{
        //    checkPick(collision.gameObject);
        //}

        public override void DestroyItem()
        {
            Destroy(this.gameObject);
        }
    }
}


