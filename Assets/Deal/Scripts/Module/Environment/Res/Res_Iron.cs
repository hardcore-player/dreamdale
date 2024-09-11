using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Deal.Env
{
    /// <summary>
    /// 采集钢铁
    /// </summary>
    public class Res_Iron : CollectableRes
    {

        public SpriteRenderer stone0;
        public SpriteRenderer stone1;
        public SpriteRenderer stone2;

        public Image cd;
        public GameObject ui;

        public GameObject block;
        //public SpriteRenderer effet;

        //public Animator animatorTree;
        //public Animator animatorEffect;


        /// <summary>
        /// 更新表现
        /// </summary>
        public override void UpdateView()
        {
            Data_CollectableRes _Data = this.GetData<Data_CollectableRes>();

            if (_Data.AssetLeft <= 0)
            {
                this.stone0.gameObject.SetActive(true);
                this.stone1.gameObject.SetActive(false);
                this.stone2.gameObject.SetActive(false);
            }
            else if (_Data.AssetLeft <= 3)
            {
                this.stone0.gameObject.SetActive(true);
                this.stone1.gameObject.SetActive(true);
                this.stone2.gameObject.SetActive(false);
            }
            else
            {
                this.stone0.gameObject.SetActive(true);
                this.stone1.gameObject.SetActive(false);
                this.stone2.gameObject.SetActive(true);
            }

            if (_Data.AssetLeft <= 0)
            {
                this.ui.gameObject.SetActive(true);
            }
            else
            {
                this.ui.gameObject.SetActive(false);
            }

            this.block.SetActive(_Data.AssetLeft > 0);
        }

        public override void UpdateCD()
        {
            Data_CollectableRes _Data = this.GetData<Data_CollectableRes>();

            if (_Data.AssetLeft <= 0)
            {
                this.ui.gameObject.SetActive(true);
                this.cd.fillAmount = _Data.GetCdProgress();
            }
            else
            {
                this.ui.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 掉落触发
        /// </summary>
        public override void OnFallAsset(int fallNum, RoleBase role)
        {

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
            });

        }
    }
}


