using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using TMPro;
using Deal.Data;
#if UNITY_WEBGL && !UNITY_EDITOR
using WeChatWASM;
#endif
using System;
using Druid.Utils;
using Deal.Env;

namespace Deal.UI
{
    public class UIGameMain : UIBase
    {
        public TextMeshProUGUI txtBagNum;
        public CmpHomeSpecialList cmpHomeSpecialList;

        public Button BtnBag;
        public Button BtnShop;
        public Button BtnEquip;

        public GameObject goRedSetting;
        // 授权头像
        public GameObject BtnGameCircle;
#if UNITY_WEBGL && !UNITY_EDITOR
        WXUserInfoButton infoButton;
#endif

        public override void OnUIAwake()
        {
            this._initBtnClick();
            this._initEventListener();
            this._checkUserInfo();


            //相对于左上角的坐标，左下角0，0
            Vector3 btnPos = UIManager.I.UICamera.WorldToScreenPoint(this.BtnGameCircle.transform.position);

            // unity 屏幕上等占比
            float unityLeft = (btnPos.x) / Screen.width;
            float unityTop = 1 - (btnPos.y) / Screen.height;

            Debug.Log($"[WXManager]   {btnPos.x}:{btnPos.y}, {unityLeft}:{unityTop} , {Screen.width}:{Screen.height}");


            if (!PlayerPrefs.HasKey("goRedSetting"))
            {
                this.goRedSetting.SetActive(true);
            }
            else
            {
                this.goRedSetting.SetActive(false);
            }

        }


        private void _checkUserInfo()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            // 是否有名字，需要授权
            bool hasUserName = _Userinfo.IsAuth == 1;
            this.Log("UILogin getUserInfo " + hasUserName);
            //hasUserName = false;
            if (hasUserName == false)
            {
#if UNITY_WEBGL && !UNITY_EDITOR

                WXManager.I.checkUserInfo((yes) =>
                {
                    if (yes)
                    {
                        WXManager.I.getUserInfo((data) =>
                        {
                            this.Log("UILogin getUserInfo " + data);
                            string nickName = data.nickName;
                            string avatarUrl = data.avatarUrl;

                            NetUtils.postUserInfo(nickName, avatarUrl);
                        });
                    }
                    else
                    {
                        // 点击授权
                        this.Log("UILogin getUserInfo 点击授权");
                        this.createLoginBtn((err, data) =>
                        {
                            this.Log("UILogin getUserInfo " + data);
                            if (err == 0)
                            {
                                string nickName = data.nickName;
                                string avatarUrl = data.avatarUrl;
                                NetUtils.postUserInfo(nickName, avatarUrl);
                            }

                        });
                    }
                });
#endif
            }

        }

#if UNITY_WEBGL && !UNITY_EDITOR

        /// <summary>
        /// 创建授权登录按钮
        /// </summary>
        public void createLoginBtn(Action<int, WXUserInfo> callback)
        {

            if (!WXManager.I.isWechet()) return;


            //Vector3 btnPos = this.BtnGameCircle.transform.position;

            //相对于左上角的坐标，左下角0，0
            Vector3 btnPos = UIManager.I.UICamera.WorldToScreenPoint(this.BtnGameCircle.transform.position);


            //左上角横坐标（以屏幕左上角为0
            var systemInfo = WX.GetSystemInfoSync();
            int screenWidth = (int)systemInfo.screenWidth;
            int screenHeight = (int)systemInfo.screenHeight;

            Debug.Log($"[WXManager] {systemInfo.screenWidth}:{systemInfo.screenHeight}, {systemInfo.windowWidth}:{systemInfo.windowHeight}, {systemInfo.pixelRatio}");


            //屏幕比例
            float rwidth = (float)(systemInfo.screenWidth / Screen.width);
            float rhieght = (float)(systemInfo.screenHeight / Screen.height);

            // unity 屏幕上等占比
            float unityLeft = (btnPos.x) / Screen.width;
            float unityTop = 1 - (btnPos.y) / Screen.height;

            Debug.Log($"[WXManager] {btnPos.x}:{btnPos.y}, {unityLeft}:{unityTop}  , {Screen.width}:{Screen.height}");


            int width = 100;
            int height = 100;
            int left = (int)(screenWidth * unityLeft) - width / 2;
            int top = (int)(screenHeight * unityTop) + height / 2;

            Debug.Log("[WXManager] left" + left);
            Debug.Log("[WXManager] top" + top);
            this.infoButton = WX.CreateUserInfoButton(left, top, width, height, "zh_CN", false);
            this.infoButton.OnTap((res) =>
            {
                Debug.Log("[WXManager] errCode" + res.errCode);
                Debug.Log("[WXManager]" + JsonUtility.ToJson(res.userInfo));
                callback(res.errCode, res.userInfo);
                infoButton.Destroy();
            });
            Debug.Log("[WXManager] infoButton Created");
    }

#endif

        private void OnDestroy()
        {
            this._removeEventListener();
#if UNITY_WEBGL && !UNITY_EDITOR

            if (this.infoButton != null)
            {
                infoButton.Destroy();
            }
#endif
        }

        public override void OnUIStart()
        {
            this._renderUI();
            this._reunderMainButtons();
        }


        private void _renderUI()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);

            this._renderBag(userData.BagToatl(), userData.Data.Data_Hero.BagTotal);

            this.cmpHomeSpecialList.gameObject.SetActive(userData.openShopServer);

        }

        private void _initBtnClick()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "LeftButtons/BtnBag", OnBagCick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "BtnSetting", OnSettingCick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "LeftButtons/BtnEquip", OnEquipCick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "LeftButtons/BtnShop", OnShopCick);
            //Druid.Utils.UIUtils.AddBtnClick(this.transform, "BtnRank", OnRankCick);
        }

        private void _initEventListener()
        {
            // 监听资产变化
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            userData.OnAssetChange += OnDataChange;
            userData.OnAbilityChange += OnAbilityChange;

            // 监听变化
            TaskManager.I.OnDataTaskChange += OnDataTaskChange;
        }


        private void _removeEventListener()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            if (userData != null)
            {
                userData.OnAssetChange -= OnDataChange;
                userData.OnAbilityChange -= OnAbilityChange;
            }

            TaskManager.I.OnDataTaskChange -= OnDataTaskChange;
        }

        /// <summary>
        /// 数据变化
        /// </summary>
        /// <param name="o"></param>
        public void OnDataChange(AssetEnum assetEnum, int assetNum)
        {
            if (DealUtils.isAssetInBagTotal(assetEnum))
            {
                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                this._renderBag(userData.BagToatl(), userData.Data.Data_Hero.BagTotal);
            }
        }

        /// <summary>
        /// 大厅能力变更
        /// </summary>
        /// <param name="abilityEnum"></param>
        /// <param name="num"></param>
        public void OnAbilityChange(HallAbilityEnum abilityEnum, int num)
        {
            if (abilityEnum == HallAbilityEnum.BagLevel)
            {
                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

                this._renderBag(userData.BagToatl(), userData.Data.Data_Hero.BagTotal);
            }
        }


        protected void OnBagCick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIBagList, UILayer.Dialog);
        }

        protected void OnSettingCick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIMenu, UILayer.Dialog);

            this.goRedSetting.SetActive(false);
            //UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UISetting, UILayer.Dialog);
        }

        protected void OnEquipCick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIEquipMain, UILayer.Layer);

            //ShopUtils.newSpecialOffer(1);
        }

        protected void OnShopCick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIShop, UILayer.Dialog);
        }

        protected void OnRankCick()
        {

            //NetUtils.postAttrSave((ok) =>
            //{
            //    if (ok == true)
            //    {
            //        UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIRank, UILayer.Dialog);
            //    }
            //});

        }


        /// <summary>
        /// 
        /// </summary>
        private void _reunderMainButtons()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            Data_Task _Task = TaskManager.I.GetTask();
            // 商店在任务8之后显示
            this.BtnShop.gameObject.SetActive(_Task.TaskId > 8 && userData.openShopServer == true);
            //this.BtnShop.gameObject.SetActive(_Task.TaskId > 8);
            // 解锁装备任务之后开始显示
            this.BtnEquip.gameObject.SetActive(_Task.TaskId > 23);
        }

        /// <summary>
        /// 任务变化
        /// </summary>
        /// <param name="task"></param>
        private void OnDataTaskChange(Data_Task task, bool isCreate)
        {
            this._reunderMainButtons();
        }


        private void _renderBag(int bagNum, int bagMax)
        {
            this.txtBagNum.text = $"{MathUtils.ToKBM(bagNum)}/{MathUtils.ToKBM(bagMax)}";

            if (bagNum >= bagMax)
            {
                this.txtBagNum.color = new Color(236 / 255f, 6 / 255f, 6 / 255f);
            }
            else
            {
                this.txtBagNum.color = new Color(1, 1, 1);
            }

        }
    }

}

