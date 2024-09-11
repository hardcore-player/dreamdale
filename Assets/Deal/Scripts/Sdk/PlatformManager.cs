using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using System;
using Deal;
using Druid.Utils;

public delegate void DelegateVideoCacheChange();


public class PlatformManager : SingletonClass<PlatformManager>
{
    public string MSG_CALLBACK_VIDEO_NOAD = "onVideoNoAD";
    public string MSG_CALLBACK_VIDEO_REWARD = "onVideoAdReward";
    public string MSG_CALLBACK_VIDEO_SHOW = "onVideoShow";
    public string MSG_CALLBACK_VIDEO_CACHED = "onVideoCacheFinish";
    public string MSG_CALLBACK_VIDEO_COMPLETED = "onVideoCompleted";
    public string MSG_CALLBACK_VIDEO_CLOSE = "onVideoClose";
    public string MSG_CALLBACK_VIDEO_BARCLICK = "onVideoBarClick";
    public string MSG_CALLBACK_VIDEO_CACHE_FAILED = "onVideoLoadFailed";
    public string MSG_CALLBACK_VIDEO_ECPM = "onVideoEcpm";

    public string MSG_CALLBACK_OADI_COMPLETED = "onOAIDValid";

    public string ANTI_ADDICTION_CALLBACK_CODE = "ANTI_ADDICTION_CALLBACK_CODE";

    public BasePlatform PlatformSdk = null;

    public string OAID = null;

    // 广告刷新时间
    public long VideoCtAt = 0;

    public DelegateVideoCacheChange OnVideoCacheChange;


    public override void Setup()
    {
#if UNITY_WEBGL
        this.PlatformSdk = new MinigamePlatform();
#elif UNITY_ANDROID
        this.PlatformSdk = new AndroidPlatform();
#else
        this.PlatformSdk = new BasePlatform();
#endif
    }

    // 广告回调
    private Action<bool> _onAdComplete;
    private bool _videoReward = false;

    private Action<int> _onTapComplete;

    public void PlayVideoAd(Action<bool> onAdComplete)
    {
        this._onAdComplete = onAdComplete;
        this._videoReward = false;


#if UNITY_EDITOR
        this._videoReward = true;
        this.OnVideoCallback();
#else
        NetUtils.postStats(new { type = "adrequest", client = GetClient() });
        if (this.PlatformSdk.HasVideoAd())
        {
            this.PlatformSdk.ShowVideoAd();
        }
        else
        {
            this._videoReward = false;
            this.OnVideoCallback();
            UIManager.I.Toast("视频还没准备好");
        }
#endif
    }

    // tap tap 登录
    public void TapStartup(Action<int> onAdComplete)
    {
        this._onTapComplete = onAdComplete;

        UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

        //是否开启
        if (userData.openIdentity == true)
        {
            this.PlatformSdk.TagLogin();
        }
        else
        {
            if (this._onTapComplete != null)
            {
                this._onTapComplete(500);
            }
        }

    }

    public string GetClient()
    {
#if UNITY_WEBGL
        return "minigame";
#elif UNITY_ANDROID
        return "android";
#elif UNITY_IOS
        return "ios";
#else
        return "unknow";
#endif
    }

    /// <summary>
    /// 回调消息
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="message"></param>
    public void OnMessage(string eventName, string message)
    {
        Debug.Log($"OnMessage eventName {eventName}");
        Debug.Log("OnMessage  message " + message);

        if (eventName == MSG_CALLBACK_VIDEO_NOAD)
        {
            OnVideoCallback();
        }
        else if (eventName == MSG_CALLBACK_VIDEO_CACHED)
        {
            NetUtils.postStats(new { type = "adfull", client = GetClient() });
        }
        else if (eventName == MSG_CALLBACK_VIDEO_COMPLETED)
        {
            this._videoReward = true;
        }
        else if (eventName == MSG_CALLBACK_VIDEO_REWARD)
        {
            this._videoReward = true;
        }
        else if (eventName == MSG_CALLBACK_VIDEO_BARCLICK)
        {
            NetUtils.postStats(new { type = "adclick", client = GetClient() });
        }
        else if (eventName == MSG_CALLBACK_VIDEO_CLOSE)
        {
            if (this._videoReward == true)
            {
                NetUtils.postStats(new { type = "ad", ad_info = message, client = GetClient() });
            }
            OnVideoCallback();
        }
        else if (eventName == MSG_CALLBACK_OADI_COMPLETED)
        {
            this.OAID = message;
        }
        else if (eventName == ANTI_ADDICTION_CALLBACK_CODE)
        {
            if (this._onTapComplete != null) this._onTapComplete(int.Parse(message));

        }
    }

    /// <summary>
    /// 广告回调
    /// </summary>
    public void OnVideoCallback()
    {
        if (this._videoReward == true)
        {
            if (this._onAdComplete != null) this._onAdComplete(true);
        }
        else
        {
            if (this._onAdComplete != null) this._onAdComplete(false);
        }

        this._videoReward = false;
        this._onAdComplete = null;

        this.VideoCtAt = TimeUtils.TimeNowMilliseconds();

        if (this.OnVideoCacheChange != null)
        {
            this.OnVideoCacheChange();
        }

    }

}
