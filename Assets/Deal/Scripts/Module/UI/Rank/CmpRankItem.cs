using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Msg;
using TMPro;
using Deal.Data;

namespace Deal.UI
{
    /// <summary>
    /// 排行版玩家信息
    /// </summary>
    public class CmpRankItem : UIBase
    {
        public CmpAvatar cmpAvatar;
        public TextMeshProUGUI txtRank;
        public TextMeshProUGUI txtCombat;

        public GameObject goSelf;

        private Msg_Data_Rankinfo _data;

        public override void OnUIAwake()
        {
        }

        public void SetData(Msg_Data_Rankinfo data, int rType)
        {
            this._data = data;
            this._renderUI(rType);
        }

        private void _renderUI(int rType)
        {
            if (this._data == null) return;

            this.cmpAvatar.SetInfo(this._data.nickname, this._data.avatar_url);
            this.txtRank.text = this._data.ranking + "";

            Debug.Log("this._data.score" + this._data.score);
            if (rType == 1 || rType == 2)
            {
                this.txtCombat.text = MathUtils.ToKBM((long)this._data.score) + "";
            }
            else if (rType == 3 || rType == 4)
            {
                this.txtCombat.text = (int)(this._data.score / 100000) + "级";
            }
            else if (rType == 5)
            {
                this.txtCombat.text = (int)(this._data.score) + "关";
            }

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            this.goSelf.SetActive(_Userinfo.UserId == this._data.uid);
        }

    }

}
