using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.Tilemaps;
using UnityEngine.AddressableAssets;
using Deal.Data;
using Deal.Env;
using Deal.UI;

namespace Deal
{
    public enum TransitionState
    {
        None,
        Ining, // 进入中
        Loading, // loading
        Outing, // 退场
    }

    // 转场管理器
    public class TransitionManager : PersistentSingleton<TransitionManager>
    {

        public UILoading uiloading;
        public UITransition uiTransiton;

        public void ShowTransition()
        {
            if (this.uiTransiton != null)
            {
                this.uiTransiton.gameObject.SetActive(true);
                this.uiTransiton.RunInAnimation();
            }

        }

        public void RemoveTransition()
        {

            if (this.uiTransiton != null)
            {
                this.uiTransiton.RunOutAnimation();
            }

        }


        /// <summary>
        /// 入场动画完成
        /// </summary>
        /// <returns></returns>
        public bool IsTransionInOver()
        {
            if (this.uiTransiton != null)
            {
                Debug.Log("this.uiTransiton.State" + TransitionState.Loading);
                return this.uiTransiton.State == TransitionState.Loading;
            }

            return false;
        }




        public void RemoveLoading()
        {
            if (uiloading != null)
            {
                Destroy(uiloading.gameObject);
            }
        }
    }
}
