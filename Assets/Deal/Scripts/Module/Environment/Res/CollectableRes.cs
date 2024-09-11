using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Deal.Data;
using Druid;

namespace Deal.Env
{

    /// <summary>
    /// 可以采集资产，树，石头，南瓜等
    /// </summary>
    public class CollectableRes : BindingSaveData
    {
        private CollectableResState State = CollectableResState.None;

        /// <summary>
        /// 更新表现
        /// </summary>
        public override void UpdateView()
        {
        }

        public virtual void UpdateCD()
        {
        }

        /// <summary>
        /// 掉落触发
        /// </summary>
        public virtual void OnFallAsset(int fallNum, RoleBase role)
        {
            Sequence s = DOTween.Sequence();
            s.Append(transform.DOScale(new Vector3(1.1f, 0.9f, 1), 0.1f));
            s.Append(transform.DOScale(new Vector3(1.0f, 1.0f, 1), 0.05f));
            s.AppendCallback(() =>
            {
                this.UpdateView();
            });
        }


        public bool CanCollect()
        {
            Data_CollectableRes _Data = this.GetData<Data_CollectableRes>();

            if (_Data.State != CollectableResState.DONE) return false;
            if (_Data.AssetLeft <= 0) return false;

            return true;
        }

        /// <summary>
        /// 检查拾取
        /// </summary>
        /// <param name="go"></param>
        private void checkCollect(GameObject go)
        {
            Data_CollectableRes _Data = this.GetData<Data_CollectableRes>();

            if (_Data.State != CollectableResState.DONE) return;

            if (go.tag != "Player")
            {
                return;
            }

            Hero hero = go.GetComponent<Hero>();
            if (hero != null)
            {
                if (_Data.CollectType == CollectableRes_CollectType.Touch)
                {
                    UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                    WorkshopToolEnum tool = userData.GetCollectResTool(_Data.AssetId);

                    int fallNum = userData.GetCollectRes(_Data.AssetId);
                    if (fallNum > 0)
                    {
                        hero.PlayWeapon(tool);
                        this.FallAsset(fallNum, hero);
                    }

                    //int fallNum = userData.GetCollectRes(collectAsset);
                    if (fallNum > 0)
                    {
                        //if (collectAsset == AssetEnum.Treasure)
                        //{
                        //    TaskManager.I.OnTaskAction(TaskActionEnum.Treasure);
                        //}
                        //this.Hero.PlayWeapon(tool);

                        //DealUtils.RoleCollectAsset(this.Hero, this.collectableAsset, collectAsset, tool, fallNum);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void FallAsset(int fallNum, RoleBase role, bool isPower = false)
        {
            Data_CollectableRes _Data = this.GetData<Data_CollectableRes>();

            if (_Data.State != CollectableResState.DONE) return;

            if (_Data.AssetLeft > 0)
            {
                if (isPower)
                {
                    // 全部掉落
                    fallNum = fallNum * _Data.AssetLeft;
                    _Data.AssetLeft = 0; ;
                }
                else
                {
                    _Data.AssetLeft -= 1;
                }


                if (_Data.AssetLeft <= 0)
                {
                    _Data.SetCDState();
                }

                this.OnFallAsset(fallNum, role);
            }
        }


        void OnTriggerEnter2D(Collider2D collision)
        {
            Data_CollectableRes _Data = this.GetData<Data_CollectableRes>();
            if (_Data == null) return;


            if (_Data.CollectType == CollectableRes_CollectType.Touch)
            {
                checkCollect(collision.gameObject);
            }
        }

        private void Update()
        {
            Data_CollectableRes _Data = this.GetData<Data_CollectableRes>();

            if (_Data != null)
            {
                this.Data.Update();

                if (this.State != _Data.State)
                {
                    this.State = _Data.State;
                    this.UpdateView();
                }

                if (this.State == CollectableResState.CD)
                {
                    this.UpdateCD();
                }
            }

            //TODU 检查表现和数据不一致要更新
        }




        //void OnTriggerStay2D(Collider2D collision)
        //{
        //    checkCollect(collision.gameObject);
        //}
    }

}
