using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using Deal.Msg;
using TMPro;

namespace Deal.UI
{
    /// <summary>
    /// 排行版信息
    /// </summary>
    public class UIRank : UIBase
    {
        public TextMeshProUGUI txtInfo;

        public TapPages tapPages;

        public CmpRankItem pfnItem;
        public CmpRankItem cmpMyRank;

        public List<CmpRankItem> items = new List<CmpRankItem>();

        private int m_rankType = 1;

        public override void OnUIAwake()
        {
            tapPages.onPageAction = this.OnPageChange;
        }

        /// <summary>
        /// 页面切换
        /// </summary>
        /// <param name="page"></param>
        public void OnPageChange(int page)
        {
            this.m_rankType = page + 1;

            if (page == 0)
            {
                this.txtInfo.text = "金币数";
                // 财富
                NetUtils.reqRankList(1, this.OnResRankList);
            }
            else if (page == 1)
            {
                this.txtInfo.text = "战力";
                // 战力
                NetUtils.reqRankList(2, this.OnResRankList);
            }
            else if (page == 2)
            {
                this.txtInfo.text = "角色等级";
                // 角色登记
                NetUtils.reqRankList(3, this.OnResRankList);
            }
            else if (page == 3)
            {
                this.txtInfo.text = "家园等级";
                // 小岛登记
                NetUtils.reqRankList(4, this.OnResRankList);
            }
            else if (page == 4)
            {
                this.txtInfo.text = "副本进度";
                // 副本进度
                NetUtils.reqRankList(5, this.OnResRankList);
            }

        }

        /// <summary>
        /// 排行版消息返回
        /// </summary>
        /// <param name="data"></param>
        private void OnResRankList(Msg_Rank data)
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            if (data != null && data.data != null && data.data.rank != null && data.data.rank.Length > 0)
            {
                this._renderList(data.data.rank, data.data.current);
            }
            else
            {
                this._renderList(null, null);
            }
        }


        private void _renderList(Msg_Data_Rankinfo[] datas, Msg_Data_Rankinfo mydata)
        {
            foreach (var item in this.items)
            {
                item.gameObject.SetActive(false);
            }

            if (datas == null)
            {
                return;
            }

            Debug.Log("current  _renderList" + datas.Length);


            for (int i = 0; i < datas.Length; i++)
            {
                CmpRankItem item;
                if (this.items.Count > i)
                {
                    item = this.items[i];
                }
                else
                {
                    item = Instantiate(this.pfnItem, this.pfnItem.transform.parent);
                    this.items.Add(item);
                }

                item.gameObject.SetActive(true);
                item.SetData(datas[i], this.m_rankType);
            }

            Debug.Log("current  _renderList" + mydata);
            this.cmpMyRank.SetData(mydata, this.m_rankType);

        }
    }
}