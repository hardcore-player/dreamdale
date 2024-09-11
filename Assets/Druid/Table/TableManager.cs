using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Druid
{
    public class TableManager : Singleton<TableManager>
    {
        private static Dictionary<string, ScriptableObject> datas = new Dictionary<string, ScriptableObject>();

        public void Register<T>(string tableName, string addressbalePath)
            where T : ScriptableObject
        {
            if (!datas.ContainsKey(tableName))
            {
                T data = ResManager.I.GetResourcesSync<T>(addressbalePath);
                //data.Init();
                datas.Add(tableName, data);
            }
        }

        public T Get<T>(string tableName) where T : ScriptableObject
        {
            if (datas.ContainsKey(tableName))
            {
                return datas[tableName] as T;
            }

            return null;
        }
    }
}