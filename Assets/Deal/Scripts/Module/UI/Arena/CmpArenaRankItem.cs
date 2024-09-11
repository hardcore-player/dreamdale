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
    public class CmpArenaRankItem : UIBase
    {
        public CmpAvatar cmpAvatar;
        public TextMeshProUGUI txtRank;
        public TextMeshProUGUI txtCombat;

        public GameObject goSelf;
        public GameObject btnChallenge;

        private Msg_Data_ArenaRankPlayerinfo _data;


        public override void OnUIAwake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "btnBattle", this.OnPkClick);
        }

        public void SetData(Msg_Data_ArenaRankPlayerinfo data)
        {
            this._data = data;
            this._renderUI();
        }




        protected void OnPkClick()
        {
            if (this._data == null) return;

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            if (userData.GetAssetNum(AssetEnum.ArenaTicket) < 1)
            {
                return;
            }

            NetUtils.doArenaChallenge(this._data.uid, (data) =>
            {
                if (data != null)
                {
                    userData.CostAsset(AssetEnum.ArenaTicket, 1);
                    UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIBattle, UILayer.Dialog, new UIParamStruct(data));
                    UIManager.I.Pop(AddressbalePathEnum.PREFAB_UIArenaPop);
                }
                else
                {
                    UIManager.I.Toast("挑战错误");
                }
            });



        }


        private void _renderUI()
        {
            if (this._data == null) return;

            this.cmpAvatar.SetInfo(this._data.nickname, this._data.avatar_url);
            this.txtRank.text = this._data.ranking + "";
            this.txtCombat.text = MathUtils.ToKBM((long)this._data.combats) + "";

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            int myRank = userData.rankInfo.data.current;
            Data_Userinfo _Userinfo = userData.Data.Userinfo;

            this.goSelf.SetActive(_Userinfo.UserId == this._data.uid);

            Debug.Log("this._data.ranking" + this._data.ranking + " " + myRank);

            // 只能挑战我前面四个
            if (this._data.ranking >= myRank - 4 && this._data.ranking < myRank)
            {
                btnChallenge.SetActive(true);
            }
            else
            {
                btnChallenge.SetActive(false);
            }

        }

    }

}
