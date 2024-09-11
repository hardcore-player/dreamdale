using System;
using System.Collections.Generic;
using Deal;
using DG.Tweening;
using Druid;
using UnityEngine;

namespace Deal
{
    public class DropPropPlank : DropProp
    {

        public Action onPick;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        public override void Fl2Picker(Vector3 pos)
        {
            if (this.PropState != DropPropState.None)
            {
                this.box2d.enabled = false;
                this.PropState = DropPropState.None;
                Destroy(_shadowIns);
                Sequence s = DOTween.Sequence();
                s.Append(transform.DOMove(pos, 0.2f));
                s.AppendCallback(() =>
                {
                    Destroy(this.gameObject);
                    //PlayManager.I.DespawnDropItem(transform);
                });
            }

        }

        /// <summary>
        /// 落地以后的逻辑
        /// </summary>
        public override void OnFall2Ground()
        {
            this.PropState = DropPropState.Idle;
            //this.box2d.enabled = true;
            this._dropingInterval = 0;

            SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);

        }

        public override void DestroyItem()
        {
            Destroy(this.gameObject);
        }

        protected override void checkPick(GameObject go)
        {
            if (this.PropState != DropPropState.Idle) return;

            if (go.tag == "Player")
            {
                Hero hero = go.GetComponentInParent<Hero>();
                if (hero != null)
                {
                    // 更新资产
                    UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                    bool added = userData.AddAsset(this.AssetId, this.AssetNum);
                    if (added)
                    {
                        this.PropState = DropPropState.Pick;
                        this.Fl2Picker(hero.transform.position);

                        // 飘字
                        DealUtils.newPopNum(hero.transform.position, $"+{this.AssetNum}");
                        //更新任务
                        //TaskManager.I.OnTaskCollect(this.AssetId, this.AssetNum);

                        SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);

                        if (this.onPick != null) this.onPick();
                    }
                    else
                    {
                        Vector3 cp = transform.position;
                        Sequence s = DOTween.Sequence();
                        s.Append(transform.DOMove(cp + new Vector3(0, 0.2f, 0), 0.1f));
                        s.Append(transform.DOMove(cp + new Vector3(0, -0.1f, 0), 0.1f));
                        s.Append(transform.DOMove(cp + new Vector3(0, 0.1f, 0), 0.1f));
                        s.Append(transform.DOMove(cp + new Vector3(0, 0f, 0), 0.1f));

                    }
                }
            }

        }

    }

}
