using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;

namespace Deal.UI
{
    public enum FishState
    {
        IDLE,
        FISHING,
        HASFISH,
    }

    public class UIPierPop : UIBase
    {
        public FishState fishState = FishState.IDLE;
        public bool _inAni = false;

        private float _aniInterval = 0;
        private float _fishInterval = 0;
        private float _fishTime = 0;

        public Button btnStart;
        public Button btnStop;
        public Button btnFish;


        public override void OnUIAwake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/BtnFish", this.OnFishClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/BtnStartFish", this.OnStartFishClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/BtnStopFish", this.OnStopFishClick);
        }


        public override void OnUIStart()
        {
            this.UpdateBtnStates();
        }


        public void OnStartFishClick()
        {
            Hero hero = PlayManager.I.mHero;
            hero.Controller.SetStatePause();
            hero.LookDir(new Vector3(1, 0, 0));
            hero.PlayStartFishingRod();

            this._inAni = true;
            this._aniInterval = 0;
            this._fishInterval = 0;
            this._fishTime = Druid.Utils.MathUtils.RandomInt(5, 10);
            this.fishState = FishState.FISHING;
            this.UpdateBtnStates();
            //hero.PlayWeapon(WorkshopToolEnum.FishingRod);
        }

        public void OnStopFishClick()
        {
            Hero hero = PlayManager.I.mHero;
            hero.Controller.SetStateIdle();
            hero.PlayWeapon(WorkshopToolEnum.FishingRod);
            SoundManager.I.playEffect(AddressbalePathEnum.WAV_fish_finish);


            WeaponFishingRod weaponFishing = hero.GetWeapon(WorkshopToolEnum.FishingRod) as WeaponFishingRod;

            weaponFishing.OnAttack = null;

            this._inAni = true;
            this._aniInterval = 0;
            this.fishState = FishState.IDLE;
            this.UpdateBtnStates();

        }

        public void OnFishClick()
        {
            Hero hero = PlayManager.I.mHero;
            hero.Controller.SetStateIdle();
            hero.PlayWeapon(WorkshopToolEnum.FishingRod);

            WeaponFishingRod weaponFishing = hero.GetWeapon(WorkshopToolEnum.FishingRod) as WeaponFishingRod;

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            int fallNum = userData.GetCollectRes(AssetEnum.Fish);

            weaponFishing.OnAttack = null;
            weaponFishing.OnAttack = () =>
            {
                DealUtils.newDropItem(AssetEnum.Fish, fallNum, hero.transform.position);
            };

            this._inAni = true;
            this._aniInterval = 0;
            this.fishState = FishState.IDLE;
            this.UpdateBtnStates();
        }


        private void Update()
        {
            if (this._inAni == true)
            {
                this._aniInterval += Time.deltaTime;
                if (this._aniInterval >= 0.5f)
                {
                    this._inAni = false;
                    this.UpdateBtnStates();
                }
            }

            if (this.fishState == FishState.FISHING)
            {
                this._fishInterval += Time.deltaTime;
                if (this._fishInterval >= this._fishTime)
                {
                    this.fishState = FishState.HASFISH;

                    Hero hero = PlayManager.I.mHero;
                    WeaponFishingRod weaponFishing = hero.GetWeapon(WorkshopToolEnum.FishingRod) as WeaponFishingRod;
                    weaponFishing.playAttackStrong();
                    this.UpdateBtnStates();
                }
            }
        }

        private void UpdateBtnStates()
        {


            if (fishState == FishState.IDLE)
            {
                this.btnStart.interactable = true;
                this.btnStop.interactable = false;
                this.btnFish.interactable = false;

                this.btnFish.gameObject.SetActive(true);
                this.btnStop.gameObject.SetActive(false);
                this.btnFish.gameObject.SetActive(false);

                Hero hero = PlayManager.I.mHero;
                hero.ChatCharacter.HideMark();
            }
            else if (fishState == FishState.FISHING)
            {
                this.btnStart.interactable = false;
                this.btnStop.interactable = true;
                this.btnFish.interactable = false;

                this.btnFish.gameObject.SetActive(false);
                this.btnStop.gameObject.SetActive(true);
                this.btnFish.gameObject.SetActive(false);

                Hero hero = PlayManager.I.mHero;
                hero.ChatCharacter.HideMark();
            }
            else if (fishState == FishState.HASFISH)
            {
                this.btnStart.interactable = false;
                this.btnStop.interactable = true;
                this.btnFish.interactable = true;

                this.btnFish.gameObject.SetActive(false);
                this.btnStop.gameObject.SetActive(false);
                this.btnFish.gameObject.SetActive(true);

                Hero hero = PlayManager.I.mHero;
                hero.ChatCharacter.ShowMark();
            }

            if (this._inAni == true)
            {
                this.btnStart.interactable = false;
                this.btnStop.interactable = false;
                this.btnFish.interactable = false;

                return;
            }
        }
    }

}

