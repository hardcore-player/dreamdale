using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Druid.Utils;
using DG.Tweening;

namespace Druid
{
    public class UIBase : UIInterface
    {
        private Dictionary<string, Action<object>> events = new Dictionary<string, Action<object>>();
        private Dictionary<DataTable, Action<DataTable>> datas = new Dictionary<DataTable, Action<DataTable>>();
        private string _name;

        private int _tag;

        //传入的参数
        private UIParamStruct _param;
        private Animator aniController;

        //是否有进场退场动画
        private bool _enterAni = false;
        private bool _exitAni = false;

        public string Name => _name;

        public UIParamStruct Param
        {
            get { return _param; }
            set { _param = value; }
        }

        public int Tag => _tag;

        public void Init(string uiName, UIParamStruct param = null)
        {
            //Debug.Log("Init");
            _name = uiName;
            _tag = tagen.get();
            this.Param = param;
            register();

            OnInit(param);
        }

        void Start()
        {
            //Debug.Log("Start");
            if (!_enterAni) OnEnter();

            OnUIStart();
        }

        void Awake()
        {
            //Debug.Log("Awake");
            chekInOutAnimation();
            OnUIAwake();
        }

        void OnDestroy()
        {
            onCleanup();
            OnUIDestroy();
        }


        private void OnEnter()
        {
            OnUIEnter();
        }

        private void OnExit()
        {
            OnUIExit();
            unregister();
            Destroy(gameObject);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnUIShow()
        {
            //Debug.Log("UIBase OnUIShow ");
            gameObject.SetActive(true);
            Animator ani = GetComponent<Animator>();
            if (ani)
            {
                ani.Play("UIShowAni", 0, 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnUIHide()
        {
            //Debug.Log("UIBase OnUIHide ");
            Animator ani = GetComponent<Animator>();
            if (ani)
            {
                UIManager.I.Get(AddressbalePathEnum.PREFAB_UIBlock).OnUIShow();
                ani.Play("UIHideAni", 0, 0);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }


        public void OnCloseClick()
        {
            CloseSelf();
        }


        public void CloseSelf()
        {
            if (_exitAni)
            {
                aniController.Play("UIHideAni", 0, 0);
            }
            else
            {
                OnExit();
            }
        }

        private void register()
        {
            UIManager.I.RegisterView(this);
        }

        private void unregister()
        {
            UIManager.I.RemoveView(this);
        }

        private void chekInOutAnimation()
        {
            //GameObject content = GameObject.Find("Content");
            //if (content)
            //{
            Animator ani = this.GetComponent<Animator>();

            this.aniController = ani;
            if (ani != null)
            {
                foreach (var animationClip in ani.runtimeAnimatorController.animationClips)
                {
                    string functionName = null;
                    if (animationClip.name == "UIShowAni")
                    {
                        functionName = "OnEnterAnimation";
                        _enterAni = true;
                    }
                    else if (animationClip.name == "ui_common_pop_out")
                    {
                        functionName = "OnExitAnimation";
                        _exitAni = true;
                    }
                    else if (animationClip.name == "UIHideAni")
                    {
                        functionName = "OnExitAnimation";
                        _exitAni = true;
                    }

                    if (functionName != null)
                    {
                        //创建动画事件
                        AnimationEvent animationEvent = new AnimationEvent();
                        //设置事件回掉函数名字
                        animationEvent.functionName = functionName;
                        //传入参数
                        animationEvent.intParameter = 12;
                        //设置触发帧
                        animationEvent.time = animationClip.events[0].time;
                        //注册事件
                        animationClip.AddEvent(animationEvent);
                    }
                }
                //}
            }
        }

        public void OnEnterAnimation()
        {
            OnEnter();
        }

        public void OnExitAnimation()
        {
            OnExit();
        }

        public void OnHideAnimation()
        {
            //UIManager.I.Get(AddressbalePathEnum.PREFAB_UIBlock).OnUIHide();
            //gameObject.SetActive(false);

        }




        public void AddEventListener(string eventName, Action<object> listener)
        {
            EventManager.I.AddEventListener(eventName, listener);
        }

        public void AddSelfEventListener(string eventName, Action<object> listener)
        {
            if (!events.ContainsKey(eventName))
            {
                events[eventName] = listener;
                AddEventListener(eventName, listener);
            }
        }

        public void AddDatatableWatch(DataTable dataTable, Action<DataTable> listener)
        {
            if (dataTable == null)
            {
                return;
            }
            if (!datas.ContainsKey(dataTable))
            {
                datas[dataTable] = listener;
                dataTable.AddDataListener(listener);
                //先执行一遍
                listener(dataTable);
            }
        }

        private void onCleanup()
        {
            foreach (var event_ in events)
            {
                EventManager.I.RemoveEventListener(event_.Key, event_.Value);
            }

            foreach (var data_ in datas)
            {
                data_.Key.RemoveEventListener(data_.Value);
            }

            events.Clear();
            datas.Clear();
        }
    }
}