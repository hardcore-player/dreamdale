using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Druid
{
    public class DataTable
    {
        private string tableName;
        public string TableName => tableName;

        public DataTable(string tableName)
        {
            this.tableName = tableName;
            Load();
        }

        public virtual void Load()
        {
        }

        public virtual void Save()
        {
        }

        public void FireUpdate()
        {
            Emit(TableName, this);
        }

        public void AddDataListener(Action<DataTable> listener)
        {
            On(TableName, listener);
        }

        public void RemoveEventListener(Action<DataTable> listener)
        {
            Off(TableName, listener);
        }

        //事件中心的容器
        private Dictionary<string, Action<DataTable>> eventDic = new Dictionary<string, Action<DataTable>>();

        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="name">事件名字</param>
        /// <param name="action">准备用来处理事件的委托函数</param>
        public void On(string name, Action<DataTable> action)
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
        public void Emit(string name, DataTable info)
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
        public void Off(string name, Action<DataTable> action)
        {
            if (eventDic.ContainsKey(name))
            {
                eventDic[name] -= action;
            }
        }
    }
}