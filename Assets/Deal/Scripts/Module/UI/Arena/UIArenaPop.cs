using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Msg;

namespace Deal.UI
{
    public class UIArenaPop : UIBase
    {
        public CmpArenaRankItem pfnItem;

        public List<CmpArenaRankItem> items = new List<CmpArenaRankItem>();


        public override void OnUIStart()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Btn/Button", this.OnRankRewardClick);

            // 请求排行版信息
            NetUtils.reqArenaRankList(this.OnResRankList);
        }

        public void OnRankRewardClick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIRankDescription, UILayer.Dialog);
        }

        /// <summary>
        /// 排行版消息返回
        /// </summary>
        /// <param name="data"></param>
        private void OnResRankList(Msg_ArenaRankInfo data)
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            userData.rankInfo = data;

            if (data != null && data.data != null && data.data.rank != null && data.data.rank.Length > 0)
            {
                this._renderList(data.data.rank);
            }
            else
            {
                this._renderList(null);
            }

        }



        private void _renderList(Msg_Data_ArenaRankPlayerinfo[] datas)
        {
            foreach (var item in this.items)
            {
                item.gameObject.SetActive(false);
            }

            for (int i = 0; i < datas.Length; i++)
            {
                CmpArenaRankItem item;
                if (this.items.Count > i)
                {
                    item = this.items[i];
                }
                else
                {
                    item = Instantiate(this.pfnItem, this.pfnItem.transform.parent);

                }

                item.gameObject.SetActive(true);
                item.SetData(datas[i]);
            }


        }

    }

}
