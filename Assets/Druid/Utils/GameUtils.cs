using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Reflection;
using System.Linq;

namespace Druid.Utils
{
    public static class GameUtils
    {
        //把scriptable的scriptable成员复制
        public static void DeepCopyScriptableFileds(ScriptableBase so)
        {
            try
            {
                Type t = so.GetType();
                var fileds = t.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic |
                                         System.Reflection.BindingFlags.Instance);
                foreach (var filed in fileds)
                {
                    //单属性
                    if (filed.FieldType.IsSubclassOf(typeof(ScriptableBase)))
                    {
                        ScriptableBase singleSO = filed.GetValue(so) as ScriptableBase;
                        if (singleSO != null)
                        {
                            ScriptableBase copy = GameObject.Instantiate(singleSO);
                            filed.SetValue(so, copy);
                            DeepCopyScriptableFileds(copy);
                            copy.OnCreate();
                        }

                        continue;
                    }

                    //list
                    if (!filed.FieldType.IsGenericType)
                    {
                        continue;
                    }

                    Type lisItemType = filed.FieldType.GetGenericArguments()[0];
                    if (!lisItemType.IsSubclassOf(typeof(ScriptableBase)))
                    {
                        continue;
                    }

                    object subObj = filed.GetValue(so);
                    if (subObj == null)
                    {
                        continue;
                    }

                    int count = Convert.ToInt32(subObj.GetType().GetProperty("Count").GetValue(subObj, null));
                    for (int i = 0; i < count; i++)
                    {
                        ScriptableBase item =
                            subObj.GetType().GetProperty("Item").GetValue(subObj, new object[] { i }) as ScriptableBase;
                        if (item != null)
                        {
                            ScriptableBase copy = GameObject.Instantiate(item);
                            subObj.GetType().GetProperty("Item").SetValue(subObj, copy, new object[] { i });
                            DeepCopyScriptableFileds(copy);
                            copy.OnCreate();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        public static bool FloatEqual(float a, float b)
        {
            return Mathf.Abs(a - b) < 0.0001f;
        }


        public static TChild CopyPublicToSubClass<TParent, TChild>(TParent parent) where TChild : TParent, new()
        {
            TChild child = new TChild();
            var ParentType = typeof(TParent);

            var fileds = ParentType.GetFields(System.Reflection.BindingFlags.Public |
                                              System.Reflection.BindingFlags.Instance);
            foreach (var filed in fileds)
            {
                filed.SetValue(child, filed.GetValue(parent));
            }

            //属性
            var Properties = ParentType.GetProperties();
            foreach (var Propertie in Properties)
            {
                //循环遍历属性
                if (Propertie.CanRead && Propertie.CanWrite)
                {
                    //进行属性拷贝
                    Propertie.SetValue(child, Propertie.GetValue(parent, null), null);
                }
            }

            return child;
        }

        /// <summary>
        /// 从后先前遍历list，方便删除
        /// </summary>
        /// <param name="list"></param>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        public static void ListSafeIterate<T>(List<T> list, Action<T> action)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                action.Invoke(list[i]);
            }
        }

        /// <summary>
        /// List的深拷贝
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> DeepCopy<T>(this List<T> list)
        {
            List<T> temp = new List<T>();
            foreach (var l in list)
            {
                temp.Add(l);
            }

            return temp;
        }



        public static void SetUrlImage(string url, RawImage rawImage)
        {
            if (url == null || !url.Contains("http")) return;

            HttpManager.HttpGet(url, null, (res) =>
             {
                 if (res.IsSuccess)
                 {
                     rawImage.texture = res.DataAsTexture2D;
                 }
             });
        }

        public static bool ContainsSpecialChars(string value)
        {
            var list = new[] { "~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "+", "=", "\"" };
            return list.Any(value.Contains);
        }

    }
}