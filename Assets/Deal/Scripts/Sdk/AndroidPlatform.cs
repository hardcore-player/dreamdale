using System.Collections;
using System.Collections.Generic;
using Deal;
using Deal.Data;
using Druid;
using UnityEngine;

public class AndroidPlatform : BasePlatform
{
    //#if UNITY_ANDROID


    public override bool IsDebug()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.Native");
        return jc.CallStatic<bool>("IsDebug");
    }

    public override string GetVersionName()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.Native");
        return jc.CallStatic<string>("GetVersionName");
    }

    #region AD
    public override void InitVideoAD(string mid)
    {

    }

    public override bool HasVideoAd()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.ADInterface");
        return jc.CallStatic<bool>("IsVideoAdReady");
    }

    public override void LoadVideoAd()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.ADInterface");
        jc.CallStatic("LoadVideoAd");
    }

    public override void ShowVideoAd()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.ADInterface");
        jc.CallStatic("ShowVideoAd");
    }

    public override string GetIMEI()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.Native");
        return jc.CallStatic<string>("GetIMEI");
    }

    #endregion AD

    #region SD
    public override string GetSdUserUUID()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.SDInterface");
        return jc.CallStatic<string>("getSourceDataUUID");
    }

    public override void LoginSd()
    {
        UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

        Data_Userinfo _Userinfo = userData.Data.Userinfo;
        string uid = _Userinfo.UserId + "";
        string openId = _Userinfo.OpenId;

        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.SDInterface");
        jc.CallStatic("setUserId", uid);
    }

    #endregion SD

    #region TAP

    public override void TagLogin()
    {
        UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

        Data_Userinfo _Userinfo = userData.Data.Userinfo;
        string uid = _Userinfo.UserId + "";
        string openId = _Userinfo.OpenId;

        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.TapInterface");
        jc.CallStatic("Login", uid);
    }

    public override void TagEnterGame()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.TapInterface");
        jc.CallStatic("EnterGame");
    }

    public override void TagLeaveGame()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.TapInterface");
        jc.CallStatic("LeaveGame");
    }

    public override int TagRemainingTime()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.TapInterface");
        return jc.CallStatic<int>("GetRemainingTime");
    }

    //public virtual void TagLogin()
    //{
    //}

    #endregion TAP

    #region JULIANG
    public override void SetJLUserId()
    {
        UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

        Data_Userinfo _Userinfo = userData.Data.Userinfo;
        string uid = _Userinfo.UserId + "";
        string openId = _Userinfo.OpenId;

        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.JLInterface");
        jc.CallStatic("SetUserId", uid);
    }

    public override string GetChannel()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.JLInterface");
        return jc.CallStatic<string>("GetChannel");

    }
    public override void OnJLEvent1(string evetName, string key, string value)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.JLInterface");
        jc.CallStatic("onEventKV1", evetName, key, value);
    }

    public override void OnJLEvent2(string evetName, string key1, string value1, string key2, string value2)
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.JLInterface");
        jc.CallStatic("onEventKV2", evetName, key1, value1, key2, value2);
    }

    #endregion JULIANG

    public override void InitNative()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.ldtgames.smallisland.Native");
        jc.CallStatic("InitNative");
    }

    //   public AndroidJavaClass GetJavaClass(string className)
    //   {
    //       AndroidJavaClass jc = new AndroidJavaClass(className);
    //       return jc;
    //   }

    //   public AndroidJavaObject GetJavaStaticObject(AndroidJavaClass jc, string objName)
    //   {
    //       return jc.GetStatic<AndroidJavaObject>(objName);
    //   }

    //   public AndroidJavaObject GetJavaObject(AndroidJavaClass jc, string objName)
    //   {
    //       return jc.Get<AndroidJavaObject>(objName);
    //   }

    //   //调用java类静态方法，无返回值
    //   public void CallVoidApi(string  jcName, string funName, params object[] args)
    //   {
    //       AndroidJavaClass jc = new AndroidJavaClass(jcName);
    //       jc.CallStatic(funName, args);
    //   }

    //   //调用java类静态方法，无返回值
    //   public string CallStringApi(string jcName, string funName,params object[] args)
    //   {
    //       AndroidJavaClass jc = new AndroidJavaClass(jcName);
    //       return jc.CallStatic<string>(funName, args);
    //   }


    //   //新添加
    //   public AndroidJavaObject GetCurrentJavaObject()
    //   {
    //       AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    //       return jc.GetStatic<AndroidJavaObject>("currentActivity");
    //   }



    //   public void CallSdkApi(string apiName, params object[] args)  //没有返回值的Call  
    //   {
    //       AndroidJavaObject jo = GetCurrentJavaObject();
    //       jo.Call(apiName, args);
    //   }

    ////uniWebView
    //public bool HasUniWebView(){
    //	return false;
    //}

    //   //wxdraw
    //   public bool CanWXDraw(){
    //       return true;
    //   }

    //   public int WXInstalled(){
    //       return 0;
    //   }
    //#endif
}
