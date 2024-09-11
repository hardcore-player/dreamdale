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
    /// 掉落道具，是一中资产，离着玩家一定距离会自动拾取
    /// </summary>
    public class DropProp : DropPropBase
    {

        public AssetEnum AssetId = AssetEnum.Gold;
        public int AssetNum = 0;
        public int AutoDisTime = 10;


        private void Update()
        {
            if (this.PropState == DropPropState.Idle)
            {
                if (this.AutoDisTime > 0)
                {
                    // 自动消失
                    this._dropingInterval += Time.deltaTime;

                    if (this._dropingInterval >= this.AutoDisTime)
                    {
                        this.box2d.enabled = false;
                        this.PropState = DropPropState.None;
                        Destroy(_shadowIns);
                        PlayManager.I.DespawnDropItem(transform);
                    }
                }
            }

            UpdateBezier();
        }

        public void SetOwner(RoleBase role)
        {
            this._mOwner = role;

            //if (this._mOwner != null)
            //{
            //    if (this._mOwner.tag == "Player")
            //    {
            //        this.srIcon.color = new Color(1, 0, 0);
            //    }
            //    else if (this._mOwner.tag == "Worker")
            //    {
            //        this.srIcon.color = new Color(0, 0, 1);
            //    }
            //}


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="assetNum"></param>
        public void SetProp(AssetEnum assetId, int assetNum)
        {
            this.AssetId = assetId;
            this.AssetNum = assetNum;
            this.PropState = DropPropState.None;
            this.box2d.enabled = false;

            SpriteUtils.SetAssetSprite1(this.srIcon, this.AssetId);
        }



        /// <summary>
        /// 落地以后的逻辑
        /// </summary>
        public override void OnFall2Ground()
        {
            bool fly2worker = false;

            // 工人砍的
            if (this._mOwner != null && this._mOwner.tag == TagDefine.Worker)
            {
                List<Worker> workers = MapManager.I.mapRender.workers;
                for (int i = 0; i < workers.Count; i++)
                {
                    Worker woker = workers[i];
                    if (woker != null && woker.Data.AssetId == this.AssetId)
                    {
                        if (Vector3.Distance(woker.transform.position, this.transform.position) < 3)
                        {
                            Data_Worker _Worker = woker.Data as Data_Worker;
                            bool added = _Worker.AddAsset(woker.Data.AssetId, this.AssetNum);
                            if (added)
                            {
                                this.PropState = DropPropState.Pick;
                                this.Fl2Picker(woker.transform.position);

                                woker.UpdateAssetLabel(this.AssetId, this.AssetNum);

                                fly2worker = true;
                            }
                        }

                    }
                }
            }


            if (this.AssetId == AssetEnum.Exp)
            {
                // 经验直接飞玩家
                Hero hero = PlayManager.I.mHero;
                this.PropState = DropPropState.Pick;
                this.Fl2Picker(hero.transform.position);

                // 飘字
                DealUtils.newPopNum(hero.transform.position, $"+{this.AssetNum}");
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);

                fly2worker = true;
            }

            if (fly2worker == false)
            {
                this.PropState = DropPropState.Idle;
                this.box2d.enabled = true;
                this._dropingInterval = 0;

                SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);
            }
            else
            {

            }

        }

        /// <summary>
        /// 检查拾取
        /// </summary>
        /// <param name="go"></param>
        protected virtual void checkPick(GameObject go)
        {
            if (this.PropState != DropPropState.Idle) return;

            if (go.tag == "Player")
            {
                Hero hero = go.GetComponentInParent<Hero>();

                if (this._mOwner != null && this._mOwner.tag != "Player") return;

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
                        ////更新任务
                        //TaskManager.I.OnTaskCollect(this.AssetId, this.AssetNum);

                        SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);
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
            else if (go.tag == "Worker")
            {
                Worker woker = go.GetComponent<Worker>();

                if (this._mOwner != null && this._mOwner.tag != "Worker") return;

                if (woker != null && woker.Data.AssetId == this.AssetId)
                {
                    Data_Worker _Worker = woker.Data as Data_Worker;
                    bool added = _Worker.AddAsset(woker.Data.AssetId, this.AssetNum);
                    if (added)
                    {
                        this.PropState = DropPropState.Pick;
                        this.Fl2Picker(woker.transform.position);

                        woker.UpdateAssetLabel(this.AssetId, this.AssetNum);
                    }
                }
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
    }
}


