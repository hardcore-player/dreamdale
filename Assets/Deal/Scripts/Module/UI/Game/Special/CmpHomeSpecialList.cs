using System.Collections;
using System.Collections.Generic;
using Druid;
using Druid.Utils;
using UnityEngine;


namespace Deal.UI
{
    /// <summary>
    /// 限时礼包侧边栏
    /// </summary>
    public class CmpHomeSpecialList : MonoBehaviour
    {

        public CmpHomeSpecialItem pfbItem;

        private UserData _userData;

        private List<CmpHomeSpecialItem> items = new List<CmpHomeSpecialItem>();

        private void Start()
        {
            this._userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            // 监听变化
            this._userData.OnSpecialOfferChange += OnSpecialOfferChange;

            // 数据清理
            List<Data_SpecialOffer> SpecialOffers = this._userData.Data.SpecialOffers;

            double nowSeconds = TimeUtils.TimeNowSeconds();
            for (int i = SpecialOffers.Count - 1; i >= 0; i--)
            {
                if (nowSeconds >= SpecialOffers[i].EndSeconds)
                {
                    SpecialOffers.RemoveAt(i);
                }
            }


            this._renderList();
        }

        private void OnDestroy()
        {
            if (this._userData != null)
            {
                this._userData.OnSpecialOfferChange -= OnSpecialOfferChange;
            }
        }


        public void OnSpecialOfferChange()
        {
            this._renderList();
        }

        /// <summary>
        /// 
        /// </summary>
        private void _renderList()
        {
            for (int i = 0; i < this.items.Count; i++)
            {
                this.items[i].gameObject.SetActive(false);
            }

            List<Data_SpecialOffer> SpecialOffers = this._userData.Data.SpecialOffers;

            // 初始化列表
            for (int i = 0; i < SpecialOffers.Count; i++)
            {
                CmpHomeSpecialItem item;
                if (this.items.Count > i)
                {
                    item = this.items[i];

                }
                else
                {
                    item = Instantiate(this.pfbItem, this.pfbItem.transform.parent);
                    items.Add(item);
                }

                item.gameObject.SetActive(true);
                item.SetData(SpecialOffers[i]);
            }
        }
    }
}

