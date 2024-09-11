using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Threading.Tasks;

namespace Druid
{
    public enum UILayer
    {
        Ground,
        Layer,
        Dialog,
        System,
        Toast,
    }

    public class UIManager : Singleton<UIManager>
    {
        public Camera UICamera;
        public Transform GroundLayer;
        public Transform LayerLayer;
        public Transform DialogLayer;
        public Transform SystemLayer;
        public Transform ToastLayer;

        public UISysLoading uILoading;

        private Dictionary<UILayer, Transform> _uiLayerRoots = new Dictionary<UILayer, Transform>();
        private Dictionary<string, UIBase> _views = new Dictionary<string, UIBase>();

        void Awake()
        {
            _uiLayerRoots.Add(UILayer.Ground, GroundLayer);
            _uiLayerRoots.Add(UILayer.Layer, LayerLayer);
            _uiLayerRoots.Add(UILayer.Dialog, DialogLayer);
            _uiLayerRoots.Add(UILayer.System, SystemLayer);
            _uiLayerRoots.Add(UILayer.Toast, ToastLayer);
        }

        #region Syna
        public GameObject LoadUI(string uiPath, UILayer layer = UILayer.Layer)
        {
            var ui = ResManager.I.GetInstantiateSync(uiPath, _uiLayerRoots[layer]);
            if (ui == null)
            {
                return null;
            }

            return ui;
        }

        /// <summary>
        /// 弹出一个界面
        /// </summary>
        /// <param name="uiPath"></param>
        /// <param name="layer"></param>
        public UIBase Push(string uiPath, UILayer layer = UILayer.Layer, UIParamStruct param = null)
        {
            GameObject go = LoadUI(uiPath, layer);
            if (go != null)
            {
                UIBase[] uis = go.GetComponents<UIBase>();
                foreach (var ui in uis)
                {
                    ui.Init(uiPath, param);
                }
            }

            return Get(uiPath);
        }

        public UIBase Push(GameObject go, string uiPath, UILayer layer = UILayer.Layer, UIParamStruct param = null)
        {
            if (go != null)
            {
                UIBase[] uis = go.GetComponents<UIBase>();
                foreach (var ui in uis)
                {
                    ui.Init(uiPath, param);
                }
            }

            return Get(uiPath);
        }

        #endregion Syna

        #region Async 异步
        public async Task<GameObject> LoadUIAsync(string uiPath, UILayer layer = UILayer.Layer)
        {
            GameObject ui = await ResManager.I.GetInstantiate(uiPath, _uiLayerRoots[layer]);
            if (ui == null)
            {
                return null;
            }

            return ui;
        }

        /// <summary>
        /// 弹出一个界面
        /// </summary>
        /// <param name="uiPath"></param>
        /// <param name="layer"></param>
        public async Task<UIBase> PushAsync(string uiPath, UILayer layer = UILayer.Layer, UIParamStruct param = null)
        {
            GameObject go = await LoadUIAsync(uiPath, layer);
            if (go != null)
            {
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_ui_open_01);

                UIBase[] uis = go.GetComponents<UIBase>();
                foreach (var ui in uis)
                {
                    ui.Init(uiPath, param);
                }

                Debug.Log($"PushAsync {uiPath} count={_views.Count}");
            }
            return Get(uiPath);
        }
        #endregion Async 异步

        /// <summary>
        /// 移除一个界面，也可以用 view.CloseSelf()
        /// </summary>
        /// <param name="uiPath"></param>
        /// <param name="layer"></param>
        public void Pop(string uiPath)
        {
            if (_views.ContainsKey(uiPath))
            {
                _views[uiPath].CloseSelf();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uiPath"></param>
        /// <param name="layer"></param>
        public UIBase Get(string uiPath)
        {
            if (_views.ContainsKey(uiPath))
            {
                return _views[uiPath];
            }
            return null;
        }


        public void RegisterView(UIBase ui)
        {
            string uiName = ui.Name;
            if (!_views.ContainsKey(uiName))
            {
                _views.Add(uiName, ui);
            }
        }

        public void RemoveView(UIBase ui)
        {
            string uiName = ui.Name;
            if (_views.ContainsKey(uiName))
            {
                _views.Remove(uiName);
            }
        }


        public void Toast(string msg)
        {


            if (WXManager.I.isWechet())
            {
                WXManager.I.showToast(msg);
            }
            else
            {
                GameObject go = LoadUI(AddressbalePathEnum.PREFAB_ToastPopup, UILayer.Toast);

                UIToast toast = go.GetComponent<UIToast>();
                toast.Show(msg);
            }
        }

        public void MsgBox(string msg, UIMsgBoxEnum msgBoxEnum = UIMsgBoxEnum.Single, Action okFun = null,
            Action canFun = null,
            object param = null)
        {
            UIParamStruct paramStruct = new UIParamStruct();

            paramStruct.message = msg;
            paramStruct.type = msgBoxEnum;
            paramStruct.okFun = okFun;
            paramStruct.canFun = canFun;
            paramStruct.param = param;

            Push(AddressbalePathEnum.PREFAB_UIMsgBox, UILayer.System, paramStruct);
        }

        public void MsgBox(UIParamStruct paramStruct)
        {
            Push(AddressbalePathEnum.PREFAB_UIMsgBox, UILayer.System, paramStruct);
        }

        public void ShowLoading()
        {
            if (this.uILoading != null) this.uILoading.gameObject.SetActive(true);
        }

        public void HideLoading()
        {
            if (this.uILoading != null) this.uILoading.gameObject.SetActive(false);
        }


        //
        // public void ClearAllLayer()
        // {
        //     foreach (var layer in _uiLayerRoots)
        //     {
        //         foreach (var child in layer.Value.FindAllChild())
        //         {
        //             Remove(child);
        //         }
        //     }
        // }
        //
        // public void ClearLayer(UILayer layer)
        // {
        //     foreach (var child in _uiLayerRoots[layer].FindAllChild())
        //     {
        //         Remove(child);
        //     }
        // }
        //
        // public void Remove(GameObject ui)
        // {
        //     ResManager.Instance.Release(ui);
        // }
        //
        // public void ShowToast(string content)
        // {
        //     var toast = Add(AddressbalePathEnum.PREFAB_UIToast, UILayer.Front);
        //     var uiToast = toast.GetComponent<UIToast>();
        //     uiToast.Content = content;
        //     uiToast.Show();
        // }
    }
}