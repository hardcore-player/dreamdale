using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Druid;

#if UNITY_WEBGL
using WeChatWASM;
#endif

using Deal;


public class WXManager : SingletonClass<WXManager>
{
#if UNITY_WEBGL
    private WeChatWASM.WXRewardedVideoAd videoAd;
#endif
    private Action adSuccessFun;
    private Action adFailedFun;

    public override void Setup()
    {
        if (!isWechet()) return;

#if UNITY_WEBGL
        WX.OnShow((res) =>
        {
            Debug.Log("[WXManager] OnShow");

            App.I.OnWxShow();
        });

        WX.OnHide((res) =>
        {
            Debug.Log("[WXManager] OnHide");
        });

        // 监听右上角分享
        WXShareAppMessageParam messageOption = new WXShareAppMessageParam();
        messageOption.title = "来和我一起玩啊，这个游戏太好玩了！"; // 转发标题
        messageOption.query = "id=";
        messageOption.imageUrl = "https://cdn.ldtgames.com/games/dreamdale-res/share/n1.jpg";

        WX.OnShareAppMessage(messageOption);
#endif
    }

    public void InitSDK(Action action)
    {
        if (!isWechet())
        {
            if (action != null) action();
            return;
        }
#if UNITY_WEBGL
        WX.InitSDK((int code) =>
        {
            Debug.Log("wx InitSDK " + code);
            this.initVideo();
            if (action != null) action();
        });
#endif
    }

    public void initVideo()
    {
        if (!isWechet()) return;
#if UNITY_WEBGL
        this.videoAd = WeChatWASM.WX.CreateRewardedVideoAd(new WeChatWASM.WXCreateRewardedVideoAdParam()
        {
            adUnitId = "adunit-810ee9fddd45c479",
            multiton = true,
        });

        this.videoAd.OnError((res) =>
        {
            if (this.adFailedFun != null)
            {
                this.adFailedFun();
                this.adFailedFun = null;
            }
        });

        this.videoAd.OnClose((res) =>
        {
            if ((res != null && res.isEnded) || res == null)
            {
                Debug.Log("正常播放结束，可以下发游戏奖励");
                //打点
                if (this.adSuccessFun != null)
                {
                    this.adSuccessFun();
                    this.adSuccessFun = null;
                }
            }
            else
            {
                Debug.Log("播放中途退出，不下发游戏奖励");

                if (this.adFailedFun != null)
                {
                    this.adFailedFun();
                    this.adFailedFun = null;
                }
            }
        });
#endif
    }


    public void showVideo(Action success, Action fail)
    {
        if (!isWechet())
        {
            success();
            return;
        }
        this.adSuccessFun = success;
        this.adFailedFun = fail;

#if UNITY_WEBGL
        this.videoAd.Show((res) =>
        {
        },
        (res) => { });
#endif
    }


    /// <summary>
    /// 检查用户权限，如果没有授权创建授权按钮
    /// </summary>
    /// <param name="xx"></param>
    /// <param name="yy"></param>
    /// <param name="bw"></param>
    /// <param name="bh"></param>
    /// <param name="callback"></param>
    public void checkUserInfo(Action<bool> callback)
    {
        if (!isWechet()) return;
#if UNITY_WEBGL
        WeChatWASM.GetSettingOption setTingOp = new WeChatWASM.GetSettingOption();
        setTingOp.success = (e) =>
        {
            Debug.Log("[WXManager] 检查用户权限 :" + "成功");
            Debug.Log("[WXManager] 检查用户权限 :" + e.ToString());
            WeChatWASM.AuthSetting authSetting = e.authSetting;

            if (!authSetting.ContainsKey("scope.userInfo"))
            {
                //未询问过用户授权，调用相关 API 或者 wx.authorize 会弹窗询问用户
                Debug.Log("[WXManager] 检查用户权限 :" + "no scope.userInfo");
                //this.createLoginBtn(callback);
                callback(false);
            }

            else if (authSetting["scope.userInfo"] == true)
            {
                Debug.Log("[WXManager] 检查用户权限 :" + "已经授权");
                //this.getUserInfo(callback);
                callback(true);
            }
            else if (authSetting["scope.userInfo"] == false)
            {
                Debug.Log("[WXManager] 检查用户权限 :" + "拒绝授权");
                // 用户已拒绝授权，再调用相关 API 或者 wx.authorize 会失败，需要引导用户到设置页面打开授权开关
                //console.log('授权：请点击右上角菜单->关于（零下记忆）->右上角菜单->设置');
                //this.createLoginBtn(callback);
                callback(false);
            }

        };
        setTingOp.fail = (e) =>
        {
            callback(false);
        };
        WeChatWASM.WX.GetSetting(setTingOp);
#endif
    }

#if UNITY_WEBGL

    /// <summary>
    /// 创建授权登录按钮
    /// </summary>
    public void createLoginBtn(Action<int, WXUserInfo> callback)
    {

        // 打印屏幕信息
        var systemInfo = WX.GetSystemInfoSync();
        Debug.Log($"{systemInfo.screenWidth}:{systemInfo.screenHeight}, {systemInfo.windowWidth}:{systemInfo.windowHeight}, {systemInfo.pixelRatio}");

        // 创建用户信息获取按钮，在底部区域创建一个300高度的透明区域
        // 首次获取会弹出用户授权窗口, 可通过右上角-设置-权限管理用户的授权记录
        var canvasWith = (int)(systemInfo.screenWidth * systemInfo.pixelRatio);
        var canvasHeight = (int)(systemInfo.screenHeight * systemInfo.pixelRatio);
        var buttonHeight = (int)(canvasWith / 1080f * 1800f);
        WXUserInfoButton infoButton = WX.CreateUserInfoButton(0, canvasHeight - buttonHeight, canvasWith, buttonHeight, "zh_CN", true);
        infoButton.OnTap((res) =>
        {
            Debug.Log("[WXManager] OnTap");
            Debug.Log("[WXManager] errCode" + res.errCode);
            Debug.Log("[WXManager] userInfo" + res.userInfo);
            Debug.Log("[WXManager]" + JsonUtility.ToJson(res.userInfo));
            callback(res.errCode, res.userInfo);
            infoButton.Destroy();
        });
        Debug.Log("infoButton Created");

        //WXUserInfoButton wXUserInfoButton = WX.CreateUserInfoButton(0, 0, 750, 1334, "zh_CN", false);

        //wXUserInfoButton.OnTap((res) =>
        //{
        //    Debug.Log("[WXManager] UserInfoButton点击后errCode :" + res.errCode);
        //    Debug.Log("[WXManager] UserInfoButton点击后userInfo :" + res.userInfo);
        //    if (res.errCode == 0)
        //    {

        //    }
        //    callback(res.userInfo);

        //    wXUserInfoButton.Destroy();
        //});

        //wXUserInfoButton.Show();
    }


    /// <summary>
    /// 获取用户信息，需要提前判断是否有权限
    /// </summary>
    /// <param name="xx"></param>
    /// <param name="yy"></param>
    /// <param name="bw"></param>
    /// <param name="bh"></param>
    /// <param name="callback"></param>
    public void getUserInfo(Action<WXUserInfo> callback)
    {
        GetUserInfoOption getUserInfoOption = new GetUserInfoOption();
        getUserInfoOption.withCredentials = true;
        //getUserInfoOption.lang = "";
        getUserInfoOption.success = (res) =>
        {
            Debug.Log("[WXManager] getUserInfo success:" + res.ToString());

            WXUserInfo userInfo = new WXUserInfo();
            userInfo.nickName = res.userInfo.nickName;
            userInfo.avatarUrl = res.userInfo.avatarUrl;
            userInfo.country = res.userInfo.country;
            userInfo.province = res.userInfo.province;
            userInfo.city = res.userInfo.city;
            userInfo.language = res.userInfo.language;
            userInfo.gender = (int)res.userInfo.gender;
            callback(userInfo);
        };
        getUserInfoOption.fail = (res) =>
        {
            Debug.Log("[WXManager] getUserInfo fail :" + res.ToString());
            callback(new WXUserInfo());
        };


        WX.GetUserInfo(getUserInfoOption);

    }
#endif


    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="success"></param>
    /// <param name="failed"></param>
    public void login(Action<string> success, Action<string> failed)
    {
        if (!isWechet()) return;

#if UNITY_WEBGL

        LoginOption loginOption = new LoginOption();
        loginOption.success = (result) =>
        {
            if (result.code != null)
            {
                success(result.code);
            }
            else
            {
                failed("");
            }
        };

        WX.Login(loginOption);
#endif
    }

    public void pay(int price)
    {
        if (!isWechet()) return;

        this.Log("[WXManager] pay开始");

#if UNITY_WEBGL

        string payOfferId = "";
        string payZoneId = "";
        string payEnv = "";

        WX.RequestMidasPayment(new RequestMidasPaymentOption()
        {
            mode = "game",
            env = 0, // 环境配置 0正式环境、1沙箱环境
            offerId = payOfferId, //在米大师侧申请的应用 id
            currencyType = "CNY",
            zoneId = "",
            buyQuantity = price * 10, // 购买数量。mode=game 时必填。购买数量。详见 buyQuantity 限制说明。
            success = (res) =>
            {
                Debug.Log("pay success!");
            },
            fail = (res) =>
            {
                Debug.Log("pay fail:" + res.errMsg);
            }
        });
#endif
    }

    /// <summary>
    /// 跳转客服
    /// https://developers.weixin.qq.com/minigame/dev/api/open-api/customer-message/wx.openCustomerServiceConversation.html
    /// </summary>
    /// <param name="price"></param>
    /// <param name="orderSign"></param>
    public void openService(int price, string orderSign)
    {
#if UNITY_WEBGL

        WX.OpenCustomerServiceConversation(new OpenCustomerServiceConversationOption
        {
            sessionFrom = "sessionFrom_test", //会话来源
            showMessageCard = true,  //是否显示会话内消息卡片，设置此参数为
            sendMessageTitle = $"充值{price}元",
            sendMessagePath = orderSign, // 会话内消息卡片路径
            sendMessageImg = "https://cdn.ldtgames.com/games/dreamdale-res/share/pay.jpg", // 会话内消息卡片图片路径

            success = (res) =>
            {
                this.Log("[WXManager] openService success" + res);
            },

            fail = (res) =>
            {
                this.Log("[WXManager] openService fail" + res);
            },

            complete = (res) =>
            {
                this.Log("[WXManager] openService complete" + res);
            },
        });
#endif

    }

    public void openService()
    {
#if UNITY_WEBGL

        WX.OpenCustomerServiceConversation(new OpenCustomerServiceConversationOption
        {
            sessionFrom = "sessionFrom_test", //会话来源
            showMessageCard = false,  //是否显示会话内消息卡片，设置此参数为
            sendMessageTitle = "客服咨询",
            //sendMessagePath = orderSign, // 会话内消息卡片路径
            sendMessageImg = "", // 会话内消息卡片图片路径

            success = (res) =>
            {
                this.Log("[WXManager] openService success" + res);
            },

            fail = (res) =>
            {
                this.Log("[WXManager] openService fail" + res);
            },

            complete = (res) =>
            {
                this.Log("[WXManager] openService complete" + res);
            },
        });
#endif
    }

    //public void openService()
    //{
    //    WX.GameCi(new OpenCustomerServiceConversationOption
    //    {
    //        sessionFrom = "sessionFrom_test", //会话来源
    //        showMessageCard = false,  //是否显示会话内消息卡片，设置此参数为
    //        sendMessageTitle = "客服",
    //        //sendMessagePath = orderSign, // 会话内消息卡片路径
    //        sendMessageImg = "", // 会话内消息卡片图片路径

    //        success = (res) =>
    //        {
    //            this.Log("[WXManager] openService success" + res);
    //        },

    //        fail = (res) =>
    //        {
    //            this.Log("[WXManager] openService fail" + res);
    //        },

    //        complete = (res) =>
    //        {
    //            this.Log("[WXManager] openService complete" + res);
    //        },
    //    });
    //}

    public void showPayAlert()
    {
        if (!isWechet()) return;

#if UNITY_WEBGL

        ShowModalOption modalOption = new ShowModalOption();
        modalOption.title = "";
        modalOption.content = "即将进入客服会话。在客服会话中点击右下角图片即可开始充值";
        modalOption.showCancel = true;
        modalOption.confirmText = "去充值";

        modalOption.success = (res) =>
        {
            this.Log("用户点击确定");
        };
        modalOption.fail = (res) =>
        {
            this.Log("用户点击取消");
        };


        WX.ShowModal(modalOption);
#endif
    }

    public void showToast(string msg)
    {
        if (!isWechet()) return;
#if UNITY_WEBGL

        ShowToastOption modalOption = new ShowToastOption();
        modalOption.title = msg;

        modalOption.success = (res) =>
        {
        };
        modalOption.fail = (res) =>
        {
        };


        WX.ShowToast(modalOption);
#endif

    }


    /// <summary>
    /// 小程序的版本号
    /// </summary>
    /// <returns></returns>
    public string getVersion()
    {
        if (!isWechet())
        {

            return "1.0.0";
        };

#if UNITY_WEBGL

        AccountInfo accountInfo = WX.GetAccountInfoSync();

        if (accountInfo.miniProgram.version != null && accountInfo.miniProgram.version.Length > 0)
        {
            return accountInfo.miniProgram.version;
        }

        return "1.0.0";
#endif
        return "1.0.0";

    }

    public string getRunPlatform()
    {
        if (!isWechet())
        {

            return "unkonw";
        };

#if UNITY_WEBGL

        DeviceInfo deviceInfo = WX.GetDeviceInfo();

        return deviceInfo.platform;
#endif
        return "unkonw";
    }


    private Font wxFont;
    public void setFont(Text text)
    {
        if (!isWechet()) return;

        Debug.Log("GetWXFont ==  setFont" + text.text);

#if UNITY_WEBGL

        if (wxFont != null)
        {
            text.font = wxFont;
        }
        else
        {
            var fallbackFont = Application.streamingAssetsPath + "/Fz.ttf";
            WeChatWASM.WX.GetWXFont(fallbackFont, (font) =>
            {
                Debug.Log("GetWXFont == " + font.name);
                if (font != null)
                {
                    this.wxFont = font;
                    text.font = font;
                }
            });
        }
#endif
    }

    public void shareAppMessage()
    {
        if (!isWechet()) return;
#if UNITY_WEBGL

        ShareAppMessageOption messageOption = new ShareAppMessageOption();
        messageOption.title = "来和我一起玩啊，这个游戏太好玩了！"; // 转发标题
        messageOption.query = "id=";
        //messageOption.path = "https://cdn.ldtgames.com/games/dreamdale-res/share/n1.jpg";
        messageOption.imageUrl = "https://cdn.ldtgames.com/games/dreamdale-res/share/n1.jpg";

        WX.ShareAppMessage(messageOption);
#endif
    }


    public bool isWechet()
    {
#if UNITY_WEBGL
        return true;
#else
        return false;
#endif
    }

}
