using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.UI;
using Deal.UI;
using Deal.Data;
using Deal.Tools;
using Druid.Utils;

namespace Deal.Env
{
    /// <summary>
    /// 木板厂
    /// </summary>
    public class BuildingChangeFactory : BuildingBase
    {

        protected Hero mHero;
        private float _flyInterval;
        private float _flyTime = 0.1f;
        private bool _isStart = false;
        private AssetFlyNumTool _flyNumTool;

        private List<DropPropPlank> plankList = new List<DropPropPlank>();

        public AssetList assetList;
        public Slider slider;
        public DropPropPlank pfbProp;

        public Animator houseAnimator;

        public override void SetData(Data_SaveBase data)
        {
            base.SetData(data);
        }

        public override void UpdateView()
        {
            base.UpdateView();
            this.UpdateSlider();
            this.UpdateWood();
            this.UpdatePlank();
        }

        private void UpdateSlider()
        {
            DataChangeFactory data_ = this.GetData<DataChangeFactory>();

            if (data_.IsStop == true)
            {
                this.slider.gameObject.SetActive(false);
            }
            else
            {
                this.slider.gameObject.SetActive(true);
                long timePassed = TimeUtils.TimeNowMilliseconds() - data_.CDAt;
                int RefreshNeed = (int)(data_.RefreshNeed * (1 - data_.RefreshNeedBuff));
                float progress = timePassed / 1f / (RefreshNeed * 1000);
                this.slider.value = progress;
            }
        }

        /// <summary>
        /// 捡起来一个
        /// </summary>
        /// <returns></returns>
        public DropPropPlank OnHeroPick()
        {
            DataChangeFactory data_ = this.GetData<DataChangeFactory>();
            data_.ToNum--;

            DropPropPlank item = this.plankList[0];
            this.plankList.RemoveAt(0);
            this.UpdatePlank();

            return item;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            // 生产
            if (this.Data != null)
            {
                this.Data.Update();
                this.UpdateView();

                this.checkAnimation();
            }

            if (mHero)
            {
                this._flyInterval += Time.deltaTime;
                if (this._flyInterval >= this._flyTime)
                {
                    PutWood2Factory();
                    this._flyInterval = 0;
                }

            }
        }

        protected void UpdateWood()
        {
            DataChangeFactory data_ = this.GetData<DataChangeFactory>();

            CmpAssetItem item = this.assetList.GetAssetItem(data_.FromAsset);
            item.gameObject.SetActive(true);
            item.UpdateNum(data_.FromNum + "/" + data_.GetFromTotal());
        }

        private void UpdatePlank()
        {
            DataChangeFactory data_ = this.GetData<DataChangeFactory>();
            int plankNum = data_.ToNum;

            if (this.plankList.Count < plankNum)
            {
                for (int i = 0; i < plankNum - this.plankList.Count; i++)
                {
                    DropPropPlank item = Instantiate(this.pfbProp, this.pfbProp.transform.parent);
                    item.gameObject.SetActive(true);
                    this.plankList.Add(item);

                    item.SetProp(data_.ToAsset, 1);
                    item.OnFall2Ground();

                    //item.onPick = () =>
                    //{

                    //};
                }
            }

            for (int i = 0; i < this.plankList.Count; i++)
            {
                this.plankList[i].transform.localPosition = new Vector3(0, 0.1f * i, 0);
            }
        }

        public void checkAnimation()
        {
            DataChangeFactory data_ = this.GetData<DataChangeFactory>();

            if (data_.IsStop == true)
            {
                this.PlayIdle();
            }
            else
            {
                this.PlayWork();
            }

        }

        public void PlayWork()
        {
            if (houseAnimator != null)
                houseAnimator.Play("work", 0);
        }

        public void PlayIdle()
        {
            if (houseAnimator != null)
                houseAnimator.Play("idle", 0);
        }

        public virtual void PutWood2Factory()
        {
            if (mHero == null) return;
            if (this.Data == null) return;

            DataChangeFactory _Data = this.GetData<DataChangeFactory>();

            int req = _Data.GetFromTotal() - _Data.FromNum;
            if (req > 0)
            {
                // 更新资产
                UserData userData = Druid.DataManager.I.Get<UserData>(DataDefine.UserData);
                int num = this._flyNumTool.numPerTime(req, userData.GetAssetNum(_Data.FromAsset));
                bool added = userData.AddAsset(_Data.FromAsset, -num);
                if (added)
                {
                    _Data.FromNum += num;

                    this.UpdateWood();

                    Vector3 to = center != null ? center.position : transform.position;
                    DealUtils.NewDropPropBezierToTarget(_Data.FromAsset, 1, mHero.center.position, to);
                    SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);

                }
            }
        }

        public override void OnHeroEnter(Hero mHero)
        {
            this.mHero = mHero;
            if (this._flyNumTool == null)
            {
                this._flyNumTool = new AssetFlyNumTool();
            }
            this._flyNumTool.stop();
        }


        public override void OnHeroExit(Hero mHero)
        {
            this.mHero = null;
        }

    }
}

