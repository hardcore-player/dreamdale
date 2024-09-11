using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Druid
{
    public class UIInterface : MonoBehaviour
    {
        /// <summary>
        /// 加载就调用，并且在Init之前 1
        /// </summary>
        public virtual void OnUIAwake()
        {
        }

        /// <summary>
        /// 加载就调用，并且在Init之前 2
        /// </summary>
        public virtual void OnInit(UIParamStruct param)
        {
            Debug.Log("UIInterface OnInit");
        }

        /// <summary>
        /// 唤醒就调用，并且在Init之后，SetActive(false)不调用 3
        /// </summary>
        public virtual void OnUIStart()
        {
        }

        /// <summary>
        /// 唤醒就调用，并且在Init之后，SetActive(false)不调用
        /// </summary>
        public virtual void OnUIDestroy()
        {
        }

        /// <summary>
        /// 动画结束
        /// </summary>
        public virtual void OnUIEnter()
        {
        }


        /// <summary>
        /// 动画结束
        /// </summary>
        public virtual void OnUIExit()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnUIShow()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnUIHide()
        {
        }

    }
}