using System;
using System.Collections.Generic;
using UnityEngine;

namespace Druid
{
    public class EventManager : SingletonClass<EventManager>
    {
        //事件中心的容器
        private Dictionary<string, Action<object>> eventDic = new Dictionary<string, Action<object>>();

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="name">事件名字</param>
        /// <param name="action">准备用来处理事件的委托函数</param>
        public void AddEventListener(string name, Action<object> action)
        {
            if (eventDic.ContainsKey(name))
            {
                if (eventDic[name] != action)
                {
                    eventDic[name] += action;
                }
                else
                {
                    Debug.LogError("event on duplicate:" + name);
                }
            }
            else
            {
                eventDic.Add(name, action);
            }
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="name">事件的名字</param>
        public void Emit(string name, object info)
        {
            if (eventDic.ContainsKey(name))
            {
                eventDic[name](info);
            }
        }


        /// <summary>
        /// 移除对应的事件监听
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public void RemoveEventListener(string name, Action<object> action)
        {
            if (eventDic.ContainsKey(name))
            {
                eventDic[name] -= action;
            }
        }
    }
}