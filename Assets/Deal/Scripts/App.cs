using System;
using System.Collections;
using System.Collections.Generic;
using Deal;
using Deal.Data;
using Deal.UI;
using Druid;
using Druid.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class App : PersistentSingleton<App>
{
    // 场景
    public DealScene CurScene;

    public UILogin uILogin;

    public bool isLogin = false;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        Application.targetFrameRate = 60;

        Debuger.EnableLog = true;
        Debuger.EnableLogLoop = true;

        WXManager.I.Setup();
        DataManager.I.Setup();
        PlatformManager.I.Setup();

        SourcedataUtils.InitSdk();
#if UNITY_EDITOR


#elif UNITY_ANDROID
        bool pIsDebug = PlatformManager.I.PlatformSdk.IsDebug();
        Debug.Log("pIsDebug" + pIsDebug);
        Config.DebugMode = pIsDebug ? 1 : 0;

        String versionName = PlatformManager.I.PlatformSdk.GetVersionName();
        Config.VersionCode = versionName;

        Debug.Log("Unity Android pIsDebug" + Config.DebugMode + " versionName" + versionName);
#endif

        if (Config.IsDebug())
        {
            Config.HttpUrl = EnvDebug.httpUrl;
        }
        else
        {
            Config.HttpUrl = EnvRelease.httpUrl;
        }

        WXManager.I.InitSDK(() =>
        {
            this.StartLogin();
        });
    }

    private async void StartLogin()
    {
        // 初始化存档
        if (DataManager.I.Get<UserData>(DataDefine.UserData) == null)
        {
            DataManager.I.Register(new UserData(DataDefine.UserData));
        }

        if (DataManager.I.Get<MapData>(DataDefine.MapData) == null)
        {
            DataManager.I.Register(new MapData(DataDefine.MapData));
        }

        if (DataManager.I.Get<DungeonData>(DataDefine.DungeonData) == null)
        {
            DataManager.I.Register(new DungeonData(DataDefine.DungeonData));
        }

        UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

        if (userData.Data.Data_Hero == null)
        {
            Data_Hero data_Hero = DataUtils.NewHero();
            data_Hero.DefaultData();
            userData.Data.Data_Hero = data_Hero;

            userData.Data.Userinfo = new Data_Userinfo();
        }


        //if (!PlayerPrefs.HasKey("PrivacyAgreement_Agree"))
        //{
        //    // 弹出隐私协议
        //    UIPrivacyAgreement uIPrivacy = await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIPrivacyAgreement, UILayer.Dialog) as UIPrivacyAgreement;
        //    uIPrivacy.SetCallback(() =>
        //    {
        //        PlatformManager.I.PlatformSdk.InitNative();
        //        this.uILogin.gameObject.SetActive(true);
        //        this.uILogin.Login();
        //    });
        //}
        //else
        //{
        PlatformManager.I.PlatformSdk.InitNative();
        this.uILogin.gameObject.SetActive(true);
        this.uILogin.Login();
        //}
    }

    public void EndLogin()
    {
        Destroy(this.uILogin.gameObject);

        StartCoroutine(this.LoadMain());
    }


    void OnDestroy()
    {
        WXManager.I.Destroy();
        DataManager.I.Destroy();
    }

    void OnApplicationForcus(bool isForcus)
    {

        if (isForcus)
        {
            Debug.Log("isForcus:true");

            if (Config.AntiAddiction == true)
            {
                PlatformManager.I.PlatformSdk.TagEnterGame();
            }
        }
        else
        {
            Debug.Log("isForcus:false");
            if (Config.AntiAddiction == true)
            {
                PlatformManager.I.PlatformSdk.TagLeaveGame();
            }

        }
    }

    /// <summary>
    /// 微信回前台
    /// </summary>
    public void OnWxShow()
    {
        this._strongCheckOrder();
    }

    IEnumerator LoadMain()
    {
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(AddressbalePathEnum.UNITY_Main, LoadSceneMode.Single, false);
        handle.Completed += (obj) =>
        {
            SceneInstance scene = ((AsyncOperationHandle<SceneInstance>)obj).Result;
            scene.ActivateAsync();

            Debug.LogWarning($"Load async scene complete{obj.Status}");
        };

        while (!handle.IsDone)
        {
            // 在此可使用handle.PercentComplete进行进度展示
            yield return null;
        }
    }

    private UserData _user;

    public void OnLogin()
    {
        this._user = DataManager.I.Get<UserData>(DataDefine.UserData);

        this.isLogin = true;


        if (this._user.Data.LandLv == 1 && this._user.Data.ChangeNameTimes > 0)
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UINickName, UILayer.Dialog, new UIParamStruct("create"));
        }

        ActivityUtils.DoDailyTask(DailyTaskTypeEnum.login, 1);

    }


    #region  Updatae

    private void Update()
    {
        if (this.isLogin == false) return;

        this.UpdateCheckOrder();
        this.UpdateDayLoop();
        this.UpdateVipLoop();
        this.UpdateAutoSave();

        if (this._user != null) this._user.UpdateAssetSave();
    }


    /// <summary>
    /// 订单检查循环
    /// </summary>

    private float _timeCheckOrder = 10;
    private float _intervalCheckOrder = 0;
    private int _checkOrderCnt = 0;

    /// <summary>
    /// 每次会前台前5次，3秒一次，后面20秒一次
    /// </summary>
    private void UpdateCheckOrder()
    {
        this._intervalCheckOrder += Time.deltaTime;

        if (this._intervalCheckOrder >= this._timeCheckOrder)
        {
            ShopUtils.CheckOrder();
            this._intervalCheckOrder = 0;

            this._checkOrderCnt += 1;

            if (this._checkOrderCnt < 5)
            {
                this._timeCheckOrder = 3;
            }
            else
            {
                this._timeCheckOrder = 20;
            }
        }
    }

    private void _strongCheckOrder()
    {
        ShopUtils.CheckOrder();
        this._checkOrderCnt = 0;
        this._timeCheckOrder = 1;
    }

    private void UpdateVipLoop()
    {
        if (this._user == null) return;

        if (this._user.Data.IsVip == true)
        {
            long timestamp = TimeUtils.TimeNowSeconds();
            if (timestamp > this._user.Data.VipEndSecond)
            {
                this._user.FinishVip();
            }
        }

    }

    /// <summary>
    /// 跨天检查
    /// </summary>
    private void UpdateDayLoop()
    {
        if (this._user == null) return;

        if (this._user.Data.LoginSecond <= 0)
        {
            this.NewDayLogin();
        }

        double logTime = this._user.Data.LoginSecond;
        long timestamp = TimeUtils.TimeNowMilliseconds();
        DateTime logDate = TimeUtils.DateTimeFromSeconds(logTime);
        DateTime nowDate = TimeUtils.DateTimeFromSeconds(timestamp / 1000);


        //if (timestamp - logTime * 1000 > 2 * 1000)
        //{
        //    this.NewDayLogin();
        //}

        // 如果不是一天
        if (!(nowDate.Year == logDate.Year && nowDate.DayOfYear == logDate.DayOfYear))
        {
            this.NewDayLogin();
        }

    }



    private float _timeAutoSave = 60;
    private float _intervalAutoSave = 0;

    /// <summary>
    /// 自动保存
    /// </summary>
    private void UpdateAutoSave()
    {
        if (this._user == null) return;

        this._intervalAutoSave += Time.deltaTime;

        if (this._intervalAutoSave >= this._timeAutoSave)
        {
            Hero hero = PlayManager.I.mHero;

            if (hero.Controller.IsIdleNoAni())
            {

                DataManager.I.Save(DataDefine.UserData);
                DataManager.I.Save(DataDefine.MapData);
                DataManager.I.Save(DataDefine.DungeonData);

                this._intervalAutoSave = 0;
                NetUtils.postSlotSave((res) => { });

                GC.Collect();
            }
        }

    }

    #endregion  Updatae




    /// <summary>
    /// 每日登录的接口，跨天的时候这个会重置时间
    /// </summary>
    private void NewDayLogin()
    {
        Debug.Log("NewDayLogin 新的一天");

        //上线时如果当前没有卷轴礼包，则触发一个已经解锁的卷轴礼包（要求的任务ID小于等于当前任务ID）
        ShopUtils.newSpecialOfferScroll();

        ShopUtils.vipFreeTicket();

        // 重置每日古代碎片次数
        this._user.Data.TodayAncientShard = 0;

        this._user.Data.TodayArenaTicketBuy = 0;

        this._user.Data.TodayShareTimes = 0;

        int arenaTicket = this._user.GetAssetNum(AssetEnum.ArenaTicket);
        if (arenaTicket < 10)
        {
            this._user.Data.Assets[AssetEnum.ArenaTicket] = 10;
        }

        this._user.Data.LoginSecond = TimeUtils.TimeNowMilliseconds() / 1000;

        // 七日签到
        ActivityUtils.CheckSignNewDay();
        ActivityUtils.CheckDailyTaskNewDay();

        ActivityUtils.DoDailyTask(DailyTaskTypeEnum.login, 1);

        this._user.Save();

    }


}
