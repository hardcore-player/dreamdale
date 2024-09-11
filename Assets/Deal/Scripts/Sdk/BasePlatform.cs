using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Deal;
using UnityEngine;

public class BasePlatform
{

    public virtual bool IsDebug()
    {
        return Config.IsDebug();
    }

    public virtual string GetVersionName()
    {
        return Config.VersionCode;
    }


    public virtual void InitNative()
    {
    }

    #region Sourcedata

    public virtual void IntSdSdk()
    {

    }

    public virtual void LoginSd()
    {

    }

    public virtual string GetSdUserUUID()
    {
        return "";
    }

    public virtual void TrackSdCustomAppShow()
    {
    }



    #endregion Sourcedata

    #region AD
    public virtual void InitVideoAD(string mid)
    {

    }

    public virtual bool HasVideoAd()
    {
        return false;
    }

    public virtual void LoadVideoAd()
    {

    }

    public virtual void ShowVideoAd()
    {

    }

    public virtual string GetIMEI()
    {
        return "";
    }

    #endregion AD

    #region TAP

    public virtual void TagLogin()
    {
    }

    public virtual void TagEnterGame()
    {
    }

    public virtual void TagLeaveGame()
    {
    }

    public virtual int TagRemainingTime()
    {
        return 0;
    }

    //public virtual void TagLogin()
    //{
    //}

    #endregion TAP

    #region JULIANG


    public virtual void SetJLUserId()
    {
    }

    public virtual string GetChannel()
    {
        return "";
    }

    public virtual void OnJLEvent1(string evetName, string key, string value)
    {
    }

    public virtual void OnJLEvent2(string evetName, string key1, string value1, string key2, string value2)
    {
    }

    #endregion JULIANG
}
