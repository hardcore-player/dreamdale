using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Deal.Data;

namespace Deal.Env
{
    /// <summary>
    /// 谷物
    /// </summary>
    public class Res_Grain : CollectableRes
    {
        public SpriteRenderer grain0;
        public SpriteRenderer grain1;
        public SpriteRenderer grain2;

        /// <summary>
        /// 更新表现
        /// </summary>
        public override void UpdateView()
        {
            Data_CollectableRes _Data = this.GetData<Data_CollectableRes>();

            if (_Data.AssetLeft <= 0)
            {
                this.grain0.gameObject.SetActive(true);
                this.grain1.gameObject.SetActive(false);
                this.grain2.gameObject.SetActive(false);

            }
            else if (_Data.AssetLeft <= 2)
            {
                this.grain0.gameObject.SetActive(false);
                this.grain1.gameObject.SetActive(true);
                this.grain2.gameObject.SetActive(false);
            }
            else
            {
                this.grain0.gameObject.SetActive(false);
                this.grain1.gameObject.SetActive(false);
                this.grain2.gameObject.SetActive(true);
            }


        }

        /// <summary>
        /// 掉落触发
        /// </summary>
        public override void OnFallAsset(int fallNum, RoleBase role)
        {
            //this.effet.gameObject.SetActive(true);

            //animatorTree.Play("ani_tree_roll", 0, 0);
            //animatorEffect.Play("ani_choptree", 0, 0);
            Data_CollectableRes _Data = this.GetData<Data_CollectableRes>();

            if (fallNum > 0)
            {
                DealUtils.newDropItem(_Data.AssetId, fallNum, transform.position, false, role);
            }

            Sequence s = DOTween.Sequence();
            //s.AppendInterval(0.25f);
            s.Append(transform.DOScale(new Vector3(1.1f, 0.9f, 1), 0.1f));
            s.Append(transform.DOScale(new Vector3(1.0f, 1.0f, 1), 0.05f));
            s.AppendCallback(() =>
            {
                this.UpdateView();
                //this.effet.gameObject.SetActive(false);
            });
        }
    }
}


