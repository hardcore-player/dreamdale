using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Deal.Tools
{
    /// <summary>
    /// 根据名字自动播放动画
    /// </summary>
    public class PlayAnimtionOnAwake : MonoBehaviour
    {
        public Animator Animator;
        public string DefaultName;

        private void Start()
        {
            if (this.DefaultName != null)
            {
                Animator.Play(this.DefaultName, 0, 0);
            }
        }
    }

}
