using System.Collections;
using System.Collections.Generic;
using Deal.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Deal.Env
{
    /// <summary>
    /// 采集苹果树
    /// </summary>
    public class Res_AppleTree : CollectableRes
    {
        public SpriteRenderer apple0;
        public SpriteRenderer apple1;
        public SpriteRenderer apple2;

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
                this.apple0.gameObject.SetActive(false);
                this.apple1.gameObject.SetActive(false);
                this.apple2.gameObject.SetActive(false);
            }
            else if (_Data.AssetLeft <= 1)
            {
                this.apple0.gameObject.SetActive(true);
                this.apple1.gameObject.SetActive(false);
                this.apple2.gameObject.SetActive(false);
            }
            else if (_Data.AssetLeft <= 2)
            {
                this.apple0.gameObject.SetActive(true);
                this.apple1.gameObject.SetActive(true);
                this.apple2.gameObject.SetActive(false);
            }
            else
            {
                this.apple0.gameObject.SetActive(true);
                this.apple1.gameObject.SetActive(true);
                this.apple2.gameObject.SetActive(true);
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
            s.Append(transform.DOScale(new Vector3(1.1f, 0.9f, 1), 0.1f));
            s.Append(transform.DOScale(new Vector3(1.0f, 1.0f, 1), 0.05f));
            s.AppendCallback(() =>
            {
                this.UpdateView();
            });
        }
    }
}


