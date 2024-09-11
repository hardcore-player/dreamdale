using System;
using UnityEngine;

public class PlatformResponse : MonoBehaviour
{

    private Action<string> m_onMessage;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    //注册platform接收回调
    public void OnReceiveMessage(Action<string> callback)
    {
        m_onMessage = callback;
    }

    /// <summary>
    /// 接收来自android和ios的消息
    /// </summary>
    /// <param name="msg"></param>
    public void OnMessage(string msg)
    {
        Debug.Log("SDKManager:OnMessage=======================");
        Debug.Log("SDKManager:" + msg);


        string eventName = "";
        string eventContent = "";

        string[] arrs = msg.Split(":");

        if (arrs != null && arrs.Length > 1)
        {
            eventName = arrs[0];
            eventContent = msg.Substring(eventName.Length + 1);
        }
        else
        {
            eventName = msg;
        }


        Debug.Log("SDKManager: eventName =" + eventName);
        Debug.Log("SDKManager: eventContent =" + eventContent);

        PlatformManager.I.OnMessage(eventName, eventContent);
    }

}
