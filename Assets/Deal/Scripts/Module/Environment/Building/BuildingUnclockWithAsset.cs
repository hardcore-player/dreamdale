using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deal.Data;
using Deal.Tools;
using Druid;
using DG.Tweening;

namespace Deal.Env
{
    /// <summary>
    /// 建筑用地，投放资产，可以解锁建筑
    /// </summary>
    public class BuildingUnclockWithAsset : BuildingBase
    {

        private Hero mHero;
        private float _flyInterval;
        private float _flyTime = 0.1f;
        private bool _isStart = false;

        private AssetFlyNumTool _flyNumTool;

        /// <summary>
        /// 更新价格
        /// </summary>
        /// <param name="asset"></param>
        public virtual void UpdatePrice(Data_GameAsset asset)
        {
        }

        public void StartDonate(Hero hero)
        {
            if (this._isStart == true)
            {
                return;
            }
            this._isStart = true;
            this._flyTime = 0.1f;
            this.mHero = hero;

            if (this._flyNumTool == null)
            {
                this._flyNumTool = new AssetFlyNumTool();
            }
            this._flyNumTool.stop();
        }

        public void StopDonate()
        {
            this._isStart = false;
            this.mHero = null;

        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (mHero && _isStart)
            {
                Data_BuildingBase _Data = this.GetData<Data_BuildingBase>();

                if (_Data.InCommonCd() == true) return;

                if (_Data.BluePrint != BluePrintEnum.None && _Data.BluePrintPrice > 0)
                {
                    this.DonateBlueprint();
                    return;
                }

                if (_Data.StatueEnum != StatueEnum.None && _Data.StatuePrice > 0)
                {
                    this.DonateStatueBlueprint();
                    return;
                }


                this._flyInterval += Time.deltaTime;
                if (this._flyInterval >= this._flyTime)
                {
                    Fly2Hero();
                    this._flyInterval = 0;
                }

                //移动停了
                //if (_isStart && !this.mHero.Controller.IsIdleNoAni())
                if (_isStart && !this.mHero.Controller.IsIdle())
                {
                    this.StopDonate();
                }
            }
        }


        private async void DonateStatueBlueprint()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_BuildingBase _Data = this.GetData<Data_BuildingBase>();

            if (userData.HasStatueBulePrint(_Data.StatueEnum))
            {
                _Data.StatuePrice = 0;
                Vector3 to = center != null ? center.position : transform.position;

                GameObject go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_DropBlueprint);

                DropBlueprint item = go.GetComponent<DropBlueprint>();
                SpriteUtils.SetStaueBlueprintSprite(item.srIcon);

                Hero hero = PlayManager.I.mHero;
                item.transform.position = hero.center.position;
                item.SetProp(_Data.BluePrint);
                item.BezierToTarget(to);
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);
                // 暂停
                this.GetComponent<BoxCollider2D>().enabled = false;

                Sequence s = DOTween.Sequence();
                s.AppendInterval(1f);
                s.AppendCallback(() =>
                {
                    this.UpdateView();
                });
                s.AppendInterval(1f);
                s.AppendCallback(() =>
                {
                    this.GetComponent<BoxCollider2D>().enabled = true;
                });
            }
        }

        private async void DonateBlueprint()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_BuildingBase _Data = this.GetData<Data_BuildingBase>();

            if (userData.HasBulePrint(_Data.BluePrint))
            {
                _Data.BluePrintPrice = 0;
                Vector3 to = center != null ? center.position : transform.position;

                GameObject go = await ResManager.I.GetInstantiate(AddressbalePathEnum.PREFAB_DropBlueprint);

                DropBlueprint item = go.GetComponent<DropBlueprint>();
                SpriteUtils.SetBlueprintSprite(item.srIcon);

                Hero hero = PlayManager.I.mHero;
                item.transform.position = hero.center.position;
                item.SetProp(_Data.BluePrint);
                item.BezierToTarget(to);
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);
                // 暂停
                this.GetComponent<BoxCollider2D>().enabled = false;

                Sequence s = DOTween.Sequence();
                s.AppendInterval(1f);
                s.AppendCallback(() =>
                {
                    this.UpdateView();
                });
                s.AppendInterval(1f);
                s.AppendCallback(() =>
                {
                    this.GetComponent<BoxCollider2D>().enabled = true;
                });
            }
        }

        private void Fly2Hero()
        {
            if (mHero == null) return;
            if (this.Data == null) return;

            bool isUnlock = true;

            Data_BuildingBase _Data = this.GetData<Data_BuildingBase>();

            List<Data_GameAsset> Price = _Data.GetPrice();

            for (int i = 0; i < Price.Count; i++)
            {
                Data_GameAsset asset = Price[i];

                if (asset.assetNum > 0)
                {
                    // 更新资产
                    UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                    int num = this._flyNumTool.numPerTime(asset.assetNum, userData.GetAssetNum(asset.assetType));
                    if (Config.IsDebug())
                    {
                        num = asset.assetNum;
                    }

                    bool added = userData.AddAsset(asset.assetType, -num);
                    if (added)
                    {
                        asset.assetNum = asset.assetNum - num;
                        this.UpdatePrice(asset);

                        Vector3 to = center != null ? center.position : transform.position;
                        DealUtils.NewDropPropBezierToTarget(asset.assetType, num, mHero.center.position, to);
                        SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);
                    }
                }

                if (asset.assetNum > 0)
                {
                    isUnlock = false;
                }

            }

            if (isUnlock == true)
            {
                this.Unclock();
            }
        }

        /// <summary>
        /// 解锁
        /// </summary>
        public virtual void Unclock()
        {
            this.StopDonate();
        }

        //void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.gameObject.tag == "Player")
        //    {
        //        mHero = collision.gameObject.GetComponent<Hero>();
        //        this._flyInterval = 0;
        //    }
        //}

        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    if (collision.gameObject.tag == "Player")
        //    {
        //        mHero = null;
        //    }
        //}
    }

}
