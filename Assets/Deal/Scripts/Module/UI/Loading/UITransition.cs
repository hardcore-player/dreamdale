using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;

namespace Deal.UI
{
    public class UITransition : LoadingBase
    {
        public Animator animator;

        private TransitionState _state = TransitionState.None;

        public TransitionState State { get => _state; set => _state = value; }

        public void RunInAnimation()
        {
            this.State = TransitionState.Ining;
            this.animator.Play("ani_transition0", 0, 0);
        }

        public void RunOutAnimation()
        {
            this.State = TransitionState.Ining;
            this.animator.Play("ani_transition2", 0, 0);
        }

        /// <summary>
        /// 回调
        /// </summary>
        public void InAnimationEnd()
        {
            this.State = TransitionState.Loading;
        }

        /// <summary>
        /// 回调
        /// </summary>
        public void OutAnimationEnd()
        {
            this.State = TransitionState.None;
        }


        public override void OnProgress(object param)
        {
            base.OnProgress(param);

            if (this.MaxProgress == 100)
            {
                this.RunOutAnimation();
            }
        }


        private void Update()
        {
            //Debug.Log("this.State" + this.State);
        }
    }
}

