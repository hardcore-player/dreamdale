using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
#if UNITY_WEBGL && !UNITY_EDITOR
using WeChatWASM;
#endif
using TMPro;
using Deal.Data;
using Deal.UI;

namespace Deal
{
    public class UISetting : UIBase
    {
        public TextMeshProUGUI txtVersion;
        public TextMeshProUGUI txtId;
        public Text txtName;

        public AdTicketPrice btnAdPice;

        public GameObject BtnGameCircle;

        public GameObject BtnFreeChange;
        public GameObject BtnVideoChange;

#if UNITY_WEBGL && !UNITY_EDITOR
        public WXGameClubButton GameClubButton { get; private set; }
#endif

        public Toggle tgMusic;
        public Toggle tgEffect;
        public Toggle tgVibrate;

        public override void OnUIAwake()
        {
            this.CreateGameClubButton();
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/BtnClose", this.OnCloseClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Group1/BtnService", this.OnServiceClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Group1/BtnGameCircle", this.OnGameCircleClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Group1/BtnSave", this.OnSaveClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/BtnPrivate", this.OnPrivateClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Name/BtnChange", this.OnChangeNameClick);

            // 
            this.tgMusic.isOn = !SoundManager.I.musicIsOff();
            this.tgEffect.isOn = !SoundManager.I.effectIsOff();
            this.tgVibrate.isOn = !SoundManager.I.vibrateIsOff();

            this.tgMusic.onValueChanged.AddListener(this.onSwitchMusic);
            this.tgEffect.onValueChanged.AddListener(this.onSwitchEffect);
            this.tgVibrate.onValueChanged.AddListener(this.onSwitchVibrate);

            // 视频改名增加次数
            btnAdPice.SetAdComplete(this.OnNameChange);

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            this.BtnFreeChange.SetActive(userData.Data.ChangeNameTimes > 0);
            this.BtnVideoChange.SetActive(userData.Data.ChangeNameTimes <= 0);

            this.txtName.text = _Userinfo.NickName;

            this.txtId.text = "ID:" + _Userinfo.UserId;

            if (Config.IsDebug())
            {
                this.txtVersion.text = "debug Ver." + Config.VersionCode;
            }
            else
            {
                this.txtVersion.text = "Ver." + Config.VersionCode;
            }
        }

        private void onSwitchMusic(bool yes)
        {
            SoundManager.I.switchMusic();
            this.tgMusic.isOn = !SoundManager.I.musicIsOff();
        }

        private void onSwitchEffect(bool yes)
        {
            SoundManager.I.switchEffect();
            this.tgEffect.isOn = !SoundManager.I.effectIsOff();
        }

        private void onSwitchVibrate(bool yes)
        {
            SoundManager.I.switchVibrate();
            this.tgVibrate.isOn = !SoundManager.I.vibrateIsOff();
        }


        public void OnServiceClick()
        {
            WXManager.I.openService();
        }

        public void OnGameCircleClick()
        {

        }

        public void OnPrivateClick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIPersonalPrivacy, UILayer.Dialog);

            this.CloseSelf();
        }



        public void OnChangeNameClick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UINickName, UILayer.Dialog, new UIParamStruct("change"));
            this.CloseSelf();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnNameChange()
        {
            UserData user = DataManager.I.Get<UserData>(DataDefine.UserData);
            user.Data.ChangeNameTimes++;
            user.Save();

            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UINickName, UILayer.Dialog);
            this.CloseSelf();
        }


        public void OnSaveClick()
        {
            NetUtils.postSlotSave((ok) =>
            {
                if (ok)
                {
                    UIManager.I.Toast("上传成功");
                }
                else
                {
                    UIManager.I.Toast("上传失败");
                }
            });
        }

        private void CreateGameClubButton()
        {
            if (!WXManager.I.isWechet()) return;

#if UNITY_WEBGL && !UNITY_EDITOR

            Vector3 btnPos = this.BtnGameCircle.transform.position;

            //左上角横坐标（以屏幕左上角为0
            var systemInfo = WX.GetSystemInfoSync();
            int screenWidth = (int)systemInfo.screenWidth;
            int screenHeight = (int)systemInfo.screenHeight;

            //屏幕比例
            float rwidth = (float)(systemInfo.screenWidth / Screen.width);
            float rhieght = (float)(systemInfo.screenHeight / Screen.height);

            // unity 屏幕上等占比
            float unityLeft = (btnPos.x + Screen.width / 2) / Screen.width;
            float unityTop = (-btnPos.y + Screen.height / 2) / Screen.height;

            GameClubButton = WX.CreateGameClubButton(new WXCreateGameClubButtonParam()
            {
                type = GameClubButtonType.text,
                text = "",
                icon = GameClubButtonIcon.white,
                style = new GameClubButtonStyle()
                {
                    left = (int)(screenWidth * unityLeft) - 40,
                    top = (int)(screenHeight * unityTop) + 20,
                    width = 80,
                    height = 40,
                }
            });
#endif
        }

        private void OnDestroy()
        {
            if (!WXManager.I.isWechet()) return;
#if UNITY_WEBGL && !UNITY_EDITOR
            GameClubButton.Destroy();
#endif
        }
    }

}
