using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Deal;
using Deal.Data;
using Druid;
using UnityEngine;

public class MinigamePlatform : BasePlatform
{
#if UNITY_WEBGL

    [DllImport("__Internal")]
    private static extern void IntSdk(int appid, string appName, string server, bool debug);

    [DllImport("__Internal")]
    private static extern void Login(string uid, string openid);

    [DllImport("__Internal")]
    private static extern void Track(string type, string openid, string param, string custom);

    [DllImport("__Internal")]
    private static extern string GetUserUUID();

    private static int appId = 69002;  // 应用/游戏ID;  
    private static string appName = "dreamdale";
    private static string server_url_release = "https://mp-sd.shouxinplay.cn/sd";
    private static string server_url_debug = "https://mp-sd.shouxinplay.cn/sd?debug=1";

    public override void IntSdSdk()
    {
        string _server_url = Config.IsDebug() ? server_url_debug : server_url_release;

        bool isDeubg = Config.IsDebug();

        Debug.Log("[InitSdk] _server_url:" + _server_url);
        Debug.Log("[InitSdk] appName:" + appName);
        Debug.Log("[InitSdk] isDeubg:" + isDeubg);

        IntSdk(appId, appName, _server_url, isDeubg);//调用定义方法
    }

    public override void LoginSd()
    {
        UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

        Data_Userinfo _Userinfo = userData.Data.Userinfo;
        string uid = _Userinfo.UserId + "";
        string openId = _Userinfo.OpenId;

        Debug.Log("[Login] uid:" + uid);
        Debug.Log("[Login] openId:" + openId);
        Login(uid, openId);

        TrackSdCustomAppShow();
    }

    public override string GetSdUserUUID()
    {
        return "";
    }

    public override void TrackSdCustomAppShow()
    {
        WeChatWASM.LaunchOptionsGame options = WeChatWASM.WX.GetLaunchOptionsSync();

        Debug.Log("_trackCustomAppShow" + options);
        var clue_token = "";
        if (options.query.ContainsKey("clue_token"))
        {
            clue_token = options.query["clue_token"];
        }
        //var req_id = options.query["req_id"] ?? "";
        //var xl_ad_id = options.query["ad_id"] ?? "";
        //var clue_token = options.query["clue_token"] ?? "";
        //var xl_creative_id = options.query["creative_id"] ?? "";
        //var xl_advertiser_id = options.query["advertiser_id"] ?? "";

        if (clue_token == "")
        {
            return;
        }

        string url_query = "";

        foreach (var item in options.query)
        {
            string key = item.Key;
            string value = item.Value;

            Debug.Log("_trackCustomAppShow options.query key" + key);
            Debug.Log("_trackCustomAppShow options.query value" + value);

            url_query = url_query + $"{key}={value}&";
        }

        UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

        Data_Userinfo _Userinfo = userData.Data.Userinfo;
        string uid = _Userinfo.UserId + "";
        string openId = _Userinfo.OpenId;

        // sdk上报
        Track("ATTRIBUTION", openId, url_query, "");
        // 服务上报

        if (url_query.Length > 0)
        {
            Debug.Log("url_query" + url_query);
            NetUtils.postQueryStats(url_query);
        }
    }


    public override void ShowVideoAd()
    {
        WXManager.I.showVideo(() =>
        {
            PlatformManager.I.OnMessage("onVideoAdReward", "");
            PlatformManager.I.OnMessage("onVideoClose", "");
        }, () =>
        {
            PlatformManager.I.OnMessage("onVideoClose", "");
        });
    }

#endif
}
