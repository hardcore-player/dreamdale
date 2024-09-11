using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;

namespace Deal.UI
{
    /// <summary>
    /// UI专场的基础类
    /// </summary>
    public class LoadingBase : UIBase
    {

        private float _curProgress = 0;
        private float _maxProgress = 0;

        public float CurProgress { get => _curProgress; set => _curProgress = value; }
        public float MaxProgress { get => _maxProgress; set => _maxProgress = value; }

        public override void OnUIAwake()
        {

            this.AddSelfEventListener(EventDefine.EVENT_SCENE_LOAD_PROGRESS, this.OnProgress);
        }

        public override void OnUIDestroy()
        {

        }


        public virtual void OnProgress(object param)
        {
            int progress = (int)param;

            this.MaxProgress = progress;
        }

    }
}

