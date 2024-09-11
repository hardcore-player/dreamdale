using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Data;
using TMPro;
using Druid.Utils;
using System;

namespace Deal.UI
{
    public class UILogin : UIBase
    {
        public TextMeshProUGUI txtStart;

        public GameObject goDebugLogin;
        public TMP_InputField tMP_InputFieldId;
        public TMP_InputField tMP_InputFieldName;
        public TMP_InputField tMP_InputFieldAvatar;

        private bool hasLocalSlot = false;

        public void Login()
        {
            this.txtStart.gameObject.SetActive(false);
            this.goDebugLogin.SetActive(false);

            int dev = 1;
            if (WXManager.I.isWechet())
            {
                dev = 0;
            }
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

#if UNITY_EDITOR
            // 编辑器
            this.goDebugLogin.SetActive(true);


#elif UNITY_ANDROID || UNITY_IOS
            //string code = SystemInfo.deviceUniqueIdentifier;
            //this._doDeviceLogin(code);

            StartCoroutine(this.DoDeviceLogin());

#elif UNITY_WEBGL
            if (WXManager.I.isWechet())
            {
                // 微信每次都要登录
                WXManager.I.login((code) =>
                {
                    this.Log("WXManager.I.login code =:" + code);
                    this._doWxLogin(code, "", "", 0);
                }, (code) => { });
            }
            else
            {
                this.goDebugLogin.SetActive(true);
            }
#endif



            Debug.Log("UILogin 本地记录" + _Userinfo.UserId);
            if (_Userinfo != null && (_Userinfo.UserId > 0))
            {
                this.hasLocalSlot = true;
                //// 有本地记录
                Debug.Log("UILogin 有本地记录" + _Userinfo.UserCode);
                //this._doWxLogin(_Userinfo.UserCode, _Userinfo.NickName, _Userinfo.AvatarUrl, dev);

                this.tMP_InputFieldId.text = _Userinfo.UserCode;
                this.tMP_InputFieldName.text = _Userinfo.NickName;
                this.tMP_InputFieldAvatar.text = _Userinfo.AvatarUrl;
            }
            else
            {
                //this.txtStart.gameObject.SetActive(true);
                this.hasLocalSlot = false;
                // 没有本地记录
                Debug.Log("UILogin 没有本地记录");

            }
        }


        IEnumerator DoDeviceLogin()
        {
            Debug.Log("PlatformManager.I.OAID2 = " + PlatformManager.I.OAID);
            while (PlatformManager.I.OAID == null)
            {
                // 在此可使用handle.PercentComplete进行进度展示
                yield return null;
            }
            Debug.Log("PlatformManager.I.OAID1 = " + PlatformManager.I.OAID);

            string code = SystemInfo.deviceUniqueIdentifier;
            this._doDeviceLogin(code, PlatformManager.I.OAID);
            //this._doDeviceLogin(code, "");
        }


        public void OnLoginClick()
        {
            // 新建账号
            //string UserCode = Druid.Utils.MathUtils.RandomDouble(10000, 99999) + "" + Druid.Utils.MathUtils.RandomDouble(10000, 99999);
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            _Userinfo.UserCode = this.tMP_InputFieldId.text;
            _Userinfo.NickName = this.tMP_InputFieldName.text;
            _Userinfo.AvatarUrl = this.tMP_InputFieldAvatar.text;

            this._doWxLogin(_Userinfo.UserCode, _Userinfo.NickName, _Userinfo.AvatarUrl, 1);
        }


        private void _doDeviceLogin(string deviceId, string oaid)
        {
            this.txtStart.gameObject.SetActive(false);

            //string uuid = SourcedataUtils.GetSaUserUUID();
            string uuid = "";
            string client = PlatformManager.I.GetClient();
            string channel = PlatformManager.I.PlatformSdk.GetChannel();
            string imei = PlatformManager.I.PlatformSdk.GetIMEI();

            var postData = new { device_id = deviceId, uuid = uuid, client = client, oaid = oaid, imei = imei, channel = channel };

            Debug.Log($"GetSaUserUUID uuid={uuid} client = {client} device_id = {deviceId} oaid = {oaid} imei = {imei}");

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            //this.Log("_doWxLogin 10054");

            NetUtils.doDeviceLogin(postData, (res) =>
            {
                Debug.Log("doWxLogin res.code" + res.code);
                if (res != null && res.code == 0)
                {
                    this.Log("登录成功");

                    this._reqServerConfig(() =>
                    {
                        PlatformManager.I.TapStartup((code) =>
                        {
                            if (code == 500)
                            {
                                PlatformManager.I.PlatformSdk.TagEnterGame();

                                //玩家登录后判断当前玩家可以进行游戏
                                //SourcedataUtils.Login();
                                PlatformManager.I.PlatformSdk.SetJLUserId();

                                UIManager.I.Toast("登录成功");
                                if (this.hasLocalSlot)
                                {
                                    this.Log("使用本地存档");
                                    this._onSlotsEnd();
                                }
                                else
                                {
                                    this.Log("拉取服务器存档");
                                    this._loadServerSlots();
                                }
                            }
                            else if (code == 1030)
                            {
                                UIManager.I.Toast("未成年玩家当前无法进行游戏");
                            }
                            else if (code == 1050)
                            {
                                UIManager.I.Toast("时长限制");
                            }
                            else if (code == 9002)
                            {
                                UIManager.I.Toast("实名过程中点击了关闭实名窗");
                            }

                        });

                    });
                }
                else
                {
                    this.Log("登录失败");

                    UIManager.I.Toast("登录失败");
                    //this.Login();
                }
            });
        }

        private void _doWxLogin(string wxCode, string nickName, string avatarUrl, int dev)
        {
            this.txtStart.gameObject.SetActive(false);

            string uuid = SourcedataUtils.GetSaUserUUID();

            string client = PlatformManager.I.GetClient();
            var postData = new { code = wxCode, nickname = nickName, avatar_url = avatarUrl, dev = dev, uuid = uuid, client = client };

            Debug.Log($"GetSaUserUUID uuid={uuid} client = {client} code = {wxCode} dev = {dev} ");

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            //this.Log("_doWxLogin 10054");

            NetUtils.doWxLogin(postData, (res) =>
            {

                Debug.Log("doWxLogin res.code" + res.code);
                if (res != null && res.code == 0)
                {
                    this.Log("登录成功");

                    SourcedataUtils.Login();

                    UIManager.I.Toast("登录成功");
                    if (this.hasLocalSlot)
                    {
                        this.Log("使用本地存档");
                        this._onSlotsEnd();
                    }
                    else
                    {
                        this.Log("拉取服务器存档");
                        this._loadServerSlots();
                    }
                }
                else
                {
                    this.Log("登录失败");

                    UIManager.I.Toast("登录失败");
                    //this.Login();
                }
            });
        }


        /// <summary>
        /// 获取服务器存档
        /// </summary>
        private void _loadServerSlots()
        {
            long t1 = TimeUtils.TimeNowMilliseconds();

            // 更新服务器存档
            this.Log("更新服务器存档");
            NetUtils.postSlotLoad((res) =>
            {
                if (res != null && res.userData != null && res.userData.Length > 0)
                {

                    UserDataSlot userDataSlot = LitJsonEx.JsonMapper.ToObject<UserDataSlot>(res.userData);
                    MapDataSlot mapDataSlot = LitJsonEx.JsonMapper.ToObject<MapDataSlot>(res.mapData);
                    DungeonDataSlot dungeonDataSlot = LitJsonEx.JsonMapper.ToObject<DungeonDataSlot>(res.dungeonData);

                    UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                    MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);
                    mapDataSlot.Load(res.mapData);
                    DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
                    userData.Data = userDataSlot;
                    mapData.Data = mapDataSlot;
                    dungeonData.Data = dungeonDataSlot;

                    userData.Save();
                    mapData.Save();
                    dungeonData.Save();

                    long t2 = TimeUtils.TimeNowMilliseconds();
                    this.Log("替换服务器存档 time" + (t2 - t1));

                }
                else
                {
                    this.Log("没有服务器存档");
                }

                this._onSlotsEnd();

            });
        }

        /// <summary>
        /// 存档验证结束
        /// </summary>
        private void _onSlotsEnd()
        {
            this._checkUserInfo();
        }

        /// <summary>
        /// 检查用户名称等信息
        /// </summary>
        private void _checkUserInfo()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            // 是否有名字，需要授权
            bool hasUserName = _Userinfo.IsAuth == 1;

            // 默认有名字，不用授权
            hasUserName = true;
            if (hasUserName)
            {
                this.OnServerEnd();

                return;
            }

            if (!WXManager.I.isWechet())
            {
                this.OnServerEnd();
                return;
            }
            else
            {
#if UNITY_WEBGL
                WXManager.I.checkUserInfo((yes) =>
                {
                    if (yes)
                    {
                        WXManager.I.getUserInfo((data) =>
                        {
                            this.Log("UILogin getUserInfo " + data);
                            string nickName = data.nickName;
                            string avatarUrl = data.avatarUrl;

                            this._postUserInfo(nickName, avatarUrl);
                            this.OnServerEnd();
                        });
                    }
                    else
                    {
                        // 点击授权
                        this.Log("UILogin getUserInfo 点击授权");
                        this.txtStart.gameObject.SetActive(true);
                        WXManager.I.createLoginBtn((err, data) =>
                        {
                            this.Log("UILogin getUserInfo " + data);
                            if (err == 0)
                            {
                                string nickName = data.nickName;
                                string avatarUrl = data.avatarUrl;
                                this._postUserInfo(nickName, avatarUrl);
                            }

                            this.OnServerEnd();
                        });
                    }
                });
#endif
            }
        }


        private void _postUserInfo(string nickName, string aratar)
        {
            this.Log("_postUserInfo");

            NetUtils.postUserInfo(nickName, aratar);
        }

        private void _reqServerConfig(Action call)
        {
            this.Log("获取服务器配置");
            NetUtils.reqShopConfig((yes) =>
            {
                this.Log("获取服务器配置");
                if (call != null)
                {
                    call();
                }
            });
        }


        /// <summary>
        /// 拉配置
        /// </summary>
        private void OnServerEnd()
        {
            this.EnterGame();
        }

        public void EnterGame()
        {
            App.I.EndLogin();
        }

    }

}

