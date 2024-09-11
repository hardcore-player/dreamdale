using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deal.Data;


namespace Deal.Env
{


    public class BindingSaveData : MonoBehaviour
    {
        [SerializeField]
        public Data_SaveBase Data;
        // 中心点
        public Transform center;
        // 任务点
        public Transform guide;

        /// <summary>
        /// 更新表现
        /// </summary>
        public virtual void UpdateView()
        {
        }

        //public virtual void OnDataChage(string param0, int param1)
        //{
        //}

        public T GetData<T>() where T : Data_SaveBase
        {
            return this.Data as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetData(Data_SaveBase data)
        {
            this.Data = data;
            this.UpdateView();

            // 更新完界面和数据在开启碰撞
            Collider2D collider2D = this.GetComponent<Collider2D>();
            if (collider2D) collider2D.enabled = true;
        }

    }
}

