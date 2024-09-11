using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Deal;
using Deal.Data;
using Druid;
using UnityEngine;

public class SourcedataUtils
{

    public static void InitSdk()
    {
        PlatformManager.I.PlatformSdk.IntSdSdk();
    }

    public static void Login()
    {
        PlatformManager.I.PlatformSdk.LoginSd();
    }

    public static string GetSaUserUUID()
    {
        return PlatformManager.I.PlatformSdk.GetSdUserUUID();
    }
}