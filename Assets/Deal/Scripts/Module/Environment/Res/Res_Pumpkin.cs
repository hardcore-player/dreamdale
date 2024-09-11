using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Deal.Env
{
    /// <summary>
    /// 采集南瓜
    /// </summary>
    public class Res_Pumpkin : CollectableRes
    {

        public SpriteRenderer pumpkin;

        public Image cd;
        public GameObject ui;

        /// <summary>
        /// 更新表现
        /// </summary>
        public override void UpdateView()
        {
            Data_CollectableRes _Data = this.GetData<Data_CollectableRes>();

            if (_Data.AssetLeft <= 0)
            {
                this.pumpkin.gameObject.SetActive(false);
            }
            else
            {
                this.pumpkin.gameObject.SetActive(true);
            }

            if (_Data.AssetLeft <= 0)
            {
                this.ui.gameObject.SetActive(true);
            }
            else
            {
                this.ui.gameObject.SetActive(false);
            }
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


