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
    /// 邮件详情
    /// </summary>
    public class UIMailDetail : UIBase
    {

        public Text txtTitle;
        public Text txtContent;
        public TextMeshProUGUI txtGet;
        public CmpRewardItem pfbReward;
        public Button btnGet;

        private Msg_Data_Mailbox data;

        public override void OnUIAwake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/BtnClose", this.OnClose1Click);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/BtnGet", this.OnRewardClick);
        }


        public override void OnInit(UIParamStruct param)
        {
            Msg_Data_Mailbox data = param.param as Msg_Data_Mailbox;

            this.SetData(data);
        }


        public void SetData(Msg_Data_Mailbox _data)
        {
            this.data = _data;

            this.txtTitle.text = this.data.title;
            this.txtContent.text = this.data.content;

            WXManager.I.setFont(this.txtContent);
            WXManager.I.setFont(this.txtTitle);

            if (this.data.rewards != null && this.data.rewards.Length >= 0)
            {
                //奖励
                for (int i = 0; i < this.data.rewards.Length; i++)
                {
                    string key = this.data.rewards[i].key;
                    int num = this.data.rewards[i].num;
                    CmpRewardItem item = Instantiate(this.pfbReward, this.pfbReward.transform.parent);
                    item.gameObject.SetActive(true);
                    item.SetAsset(DealUtils.toAssetEnum(key), num); ;
                }
            }

            this.btnGet.interactable = this.data.is_receive == 0;
            this.txtGet.text = this.data.is_receive == 0 ? "领取" : "已领取";
        }

        public void OnClose1Click()
        {
            if (data == null || data.rewards == null || data.rewards.Length == 0)
            {
                // 没有奖励，标记已读
                if (data.is_receive == 0)
                {
                    this._doRead();
                }
            }

            this.CloseSelf();
        }

        public void OnRewardClick()
        {

            // 主动领取奖励
            if (data == null || data.rewards == null || data.rewards.Length == 0) return;
            this._doRead();
        }



        private void _doRead()
        {
            NetUtils.doReqMailReceive(data.id, (res) =>
            {
                if (res == true)
                {
                    if (data == null || data.rewards == null || data.rewards.Length == 0) return;
                    // 发奖励
                    List<Data_GameAsset> rewards = new List<Data_GameAsset>();

                    for (int i = 0; i < this.data.rewards.Length; i++)
                    {
                        Msg_Data_Mailbox_Rewards item = this.data.rewards[i];

                        AssetEnum assetEnum = DealUtils.toAssetEnum(item.key);
                        int num = item.num;
                        rewards.Add(new Data_GameAsset(assetEnum, num));
                    }

                    ShopUtils.showRewardAndToUser(rewards);

                    this.data.is_receive = 1;
                    this.btnGet.interactable = this.data.is_receive == 0;
                    this.txtGet.text = this.data.is_receive == 0 ? "领取" : "已领取";
                }
            });
        }

    }
}
