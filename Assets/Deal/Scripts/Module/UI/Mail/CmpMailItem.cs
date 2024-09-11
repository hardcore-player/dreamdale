using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using Deal.Msg;
using TMPro;

namespace Deal.UI
{
    public class CmpMailItem : MonoBehaviour
    {

        public Text txtTitle;
        public TextMeshProUGUI txtDate;
        public TextMeshProUGUI txtExpire;

        public GameObject iconOpen;
        public GameObject iconClose;


        private Msg_Data_Mailbox _data;


        private void Awake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Btn", this.OnEmailClick);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void SetData(Msg_Data_Mailbox data)
        {
            this._data = data;

            this.txtTitle.text = "" + data.title;
            this.txtDate.text = "" + data.created_at;
            this.txtExpire.text = "有效期至:" + data.expire_at;

            WXManager.I.setFont(this.txtTitle);

            this.iconClose.SetActive(data.is_receive == 0);
            this.iconOpen.SetActive(data.is_receive == 1);
        }


        public void OnEmailClick()
        {
            if (this._data == null) return;
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIEmailDetail, UILayer.Dialog, new UIParamStruct(this._data));
            UIManager.I.Pop(AddressbalePathEnum.PREFAB_UIEmail);
        }

    }

}

