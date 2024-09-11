using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Msg;

namespace Deal.UI
{
    public class UIMail : UIBase
    {

        public CmpMailItem pfbMailItem;

        private List<CmpMailItem> _cmpMails = new List<CmpMailItem>();


        public override void OnUIStart()
        {
            NetUtils.doReqMailList(this.OnResMailData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void OnResMailData(Msg_Data_Mailbox[] data)
        {
            if (data == null || data.Length == 0)
            {
                // 
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    CmpMailItem item = Instantiate(this.pfbMailItem, this.pfbMailItem.transform.parent);
                    item.gameObject.SetActive(true);
                    item.SetData(data[i]);

                    this._cmpMails.Add(item);
                }
            }
        }
    }

}

