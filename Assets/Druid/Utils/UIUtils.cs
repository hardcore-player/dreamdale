using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Druid.Utils
{
    public class UIUtils
    {
        #region UI 查找相关封装

        /// <summary>
        /// 查找组件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static GameObject Find(Transform parent, string path)
        {
            GameObject go = FindChild(parent, path);
            if (go)
                return go;
            Debug.LogWarning("not GameObject Find name:" + parent.name + " on Path " + path);
            return null;
        }

        /// <summary>
        ///  查找组件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindCmp<T>(Transform parent, string path) where T : MonoBehaviour
        {
            GameObject go = FindChild(parent, path);
            if (go)
                return go.GetComponent<T>();
            Debug.LogError("not GameObject Find name:" + parent.name + " on Path " + path);
            return null;
        }

        /// <summary>
        /// 强势查找组件，没有就添加一个
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindCmpForce<T>(Transform parent, string path) where T : MonoBehaviour
        {
            GameObject go = FindChild(parent, path);
            if (go)
            {
                T cmp = go.GetComponent<T>();
                if (cmp == null) cmp = go.AddComponent<T>();
                return cmp;
            }

            Debug.LogError("not GameObject Find name:" + parent.name + " on Path " + path);
            return null;
        }


        public static Image FindImage(Transform parent, string path)
        {
            GameObject go = FindChild(parent, path);
            if (go)
                return go.GetComponent<Image>();
            return null;
        }

        public static Text FindText(Transform parent, string path)
        {
            GameObject go = FindChild(parent, path);
            if (go)
                return go.GetComponent<Text>();
            return null;
        }

        public static TextMeshProUGUI TextMeshProUGUI(Transform parent, string path)
        {
            GameObject go = FindChild(parent, path);
            if (go)
                return go.GetComponent<TextMeshProUGUI>();
            return null;
        }


        public static Button FindBtn(Transform parent, string path)
        {
            GameObject go = FindChild(parent, path);
            if (go)
                return go.GetComponent<Button>();
            return null;
        }

        /// <summary>
        /// 给一个UI按钮增加点击事件
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="path"></param>
        /// <param name="call"></param>
        /// <param name="sfxPath">点击音效，为空时走通用</param>
        public static void AddBtnClick(Transform parent, string path, UnityAction call, string sfxPath = null)
        {
            GameObject go = FindChild(parent, path);
            Button btn = null;
            if (go) btn = go.GetComponent<Button>();
            if (btn)
                btn.onClick.AddListener(() =>
                {
                    if (sfxPath == null)
                        SoundManager.I.playBtnSound();
                    else
                        SoundManager.I.playEffect(sfxPath);
                    call?.Invoke();
                });
        }

        public static void AddBtnClick(Transform parent, string path, UnityAction call)
        {
            GameObject go = FindChild(parent, path);
            Button btn = null;
            if (go) btn = go.GetComponent<Button>();
            if (btn)
                btn.onClick.AddListener(() =>
                {
                    SoundManager.I.playBtnSound();
                    call?.Invoke();
                });
        }

        public static Toggle FindToggle(Transform parent, string path)
        {
            GameObject go = FindChild(parent, path);
            if (go)
                return go.GetComponent<Toggle>();
            return null;
        }


        /// <summary>
        /// 查找子物体（递归查找）  
        /// </summary> 
        /// <param name="trans">父物体</param>
        /// <param name="goName">子物体的名称</param>
        /// <returns>找到的相应子物体</returns>
        public static GameObject FindChild(Transform trans, string goName)
        {
            Transform child = trans.Find(goName);
            if (child != null)
                return child.gameObject;

            GameObject go;
            for (int i = 0; i < trans.childCount; i++)
            {
                child = trans.GetChild(i);
                go = FindChild(child, goName);
                if (go != null)
                    return go;
            }

            return null;
        }

        /// <summary>
        /// 递归查找组件
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="goName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T FindChildComponent<T>(Transform trans, string goName) where T : class
        {
            Transform child = trans.Find(goName);
            if (child != null)
                return child.gameObject.GetComponent<T>();
            T go = null;
            for (int i = 0; i < trans.childCount; i++)
            {
                child = trans.GetChild(i);
                go = FindChildComponent<T>(child, goName);
                if (go != null)
                    return go;
            }

            return go;
        }
    }
    #endregion UI 查找相关封装
}