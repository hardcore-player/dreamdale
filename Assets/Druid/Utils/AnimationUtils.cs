using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

namespace Druid.Utils
{
    public class AnimationUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dalayTime"></param>
        /// <param name="callback"></param>
        public static void DelayToCall(float dalayTime, Action callback)
        {
            Sequence s = DOTween.Sequence();
            s.AppendInterval(dalayTime);
            s.AppendCallback(() => { callback(); });
        }


        public static Sequence Sequence()
        {
            return DOTween.Sequence();
        }
    }
}