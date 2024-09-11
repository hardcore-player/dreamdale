using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Druid;
using UnityEngine;


namespace Deal
{
    /// <summary>
    /// 小岛等场景
    /// </summary>
    public class DealScene : MonoBehaviour
    {
        // 场景名字
        public SceneEnum sceneName = SceneEnum.none;


        private int _curProgress = 0;
        public int CurProgress { get => _curProgress; set => _curProgress = value; }

        private async void Start()
        {
            App.I.CurScene = this;
            await this.LoadScene();
        }

        /// <summary>
        /// 设置加载进度
        /// </summary>
        /// <param name="progress"></param>
        protected virtual void SetProgress(int progress)
        {
            this.CurProgress += progress;
            EventManager.I.Emit(EventDefine.EVENT_SCENE_LOAD_PROGRESS, this.CurProgress);
        }


        public virtual async UniTask LoadScene()
        {
        }

    }

}
