using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Druid.Utils;

#if UNITY_EDITOR


namespace Druid
{
    public class ScriptableManager : Singleton<ScriptableManager>
    {
        private Dictionary<string, ScriptableBase> cache = new Dictionary<string, ScriptableBase>();

        public override void Init()
        {
            cache.Clear();
        }

        /// <summary>
        /// 加载并实例化一个so
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">addressable 路径</param>
        /// <returns></returns>
        public T GetInstantiateScriptable<T>(string path) where T : ScriptableBase
        {
            // var s = OnlyLoadScriptable<T>(path);
            T s = AssetDatabase.LoadAssetAtPath<T>(path);
            if (s == null)
            {
                Debug.LogError("so is null ," + path);
                return null;
            }

            var so = GameObject.Instantiate(s);
            GameUtils.DeepCopyScriptableFileds(so);
            so.OnCreate();
            return so;
        }

        /// <summary>
        /// 只加载一个so，不实例化；会指向同一个实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">addressable 路径</param>
        /// <returns></returns>
        public T OnlyLoadScriptable<T>(string path) where T : ScriptableBase
        {
            ScriptableBase s = null;
            if (cache.ContainsKey(path))
            {
                s = cache[path];
            }
            else
            {
                s = ResManager.I.GetResourcesSync<ScriptableBase>(path);
                cache.Add(path, s);
            }

            if (s == null)
            {
                Debug.LogError("GetInstantiateScriptable is null");
                return null;
            }

            return s as T;
        }

        /// <summary>
        /// 拷贝一个so
        /// </summary>
        /// <param name="so"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Copy<T>(T so) where T : ScriptableBase
        {
            var newSO = GameObject.Instantiate(so);
            //这里是不是应全copy呀？todo zj
            GameUtils.DeepCopyScriptableFileds(so);
            newSO.OnCreate();
            return newSO;
        }
    }
}

#endif