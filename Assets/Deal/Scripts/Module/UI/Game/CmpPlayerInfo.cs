using System.Collections;
using System.Collections.Generic;
using Druid;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Deal.Data;

namespace Deal.UI
{
    public class CmpPlayerInfo : MonoBehaviour
    {
        public Text txtUserName;
        public TextMeshProUGUI txtLandLv;
        public TextMeshProUGUI txtPlayerLv;
        public Slider sliderLandExp;
        public Slider sliderPlayerExp;
        public RawImage imgAvatar;
        public GameObject goLand;
        public GameObject goPlayer;

        public GameObject iconVip;

        void Start()
        {
            // 监听资产变化
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            userData.OnLandExpChange += OnLandExpChange;
            userData.OnVipChange += OnVipChange;
            userData.OnUserInfoChange += OnUserInfoChange;

            // 人物经验
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            dungeonData.OnHeroLvChange += OnHeroLvChange;

            this.OnLandExpChange(userData.Data.LandLv, userData.Data.LandExp);
            this.OnHeroLvChange(dungeonData.Data.HeroLv, dungeonData.Data.HeroExp, false);

            if (App.I.CurScene.sceneName == SceneEnum.dungeon)
            {
                this.goPlayer.SetActive(true);
                this.goLand.SetActive(false);
            }
            else
            {
                this.goPlayer.SetActive(false);
                this.goLand.SetActive(true);
            }

            // vip
            this.iconVip.SetActive(userData.Data.IsVip);

            this.OnUserInfoChange();
        }

        private void OnDestroy()
        {
            // 监听资产变化
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            if (userData != null)
            {
                userData.OnLandExpChange -= OnLandExpChange;
                userData.OnVipChange -= OnVipChange;
                userData.OnUserInfoChange -= OnUserInfoChange;
            }


            // 人物经验
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            if (dungeonData != null)
            {
                dungeonData.OnHeroLvChange -= OnHeroLvChange;
            }
        }


        /// <summary>
        /// 小岛经验变化
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="exp"></param>
        public void OnLandExpChange(int lv, int exp)
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            this.txtLandLv.text = "" + lv;
            this.sliderLandExp.value = exp / 1f / userData.Data.LandExpMax;
        }

        public void OnUserInfoChange()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            Data_Userinfo _Userinfo = userData.Data.Userinfo;
            // 头像
            Druid.Utils.GameUtils.SetUrlImage(_Userinfo.AvatarUrl, this.imgAvatar);

            this.txtUserName.text = _Userinfo.NickName;
            WXManager.I.setFont(txtUserName);
        }

        /// <summary>
        /// 人物经验变化
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="exp"></param>
        public void OnHeroLvChange(int lv, int exp, bool lvup)
        {
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            this.txtPlayerLv.text = "" + lv;
            this.sliderPlayerExp.value = exp / 1f / dungeonData.Data.HeroExpMax;
        }

        /// <summary>
        /// vip信息变化
        /// </summary>
        public void OnVipChange()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            // vip
            this.iconVip.SetActive(userData.Data.IsVip);
        }


    }

}
