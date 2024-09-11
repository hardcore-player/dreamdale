using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Msg;
using DG.Tweening;
using TMPro;

namespace Deal.UI
{
    public class UIAreneResult : UIBase
    {
        public GameObject goLose;
        public GameObject goWin;

        public CmpAvatar avatarLeft;
        public CmpAvatar avatarRight;

        public TextMeshProUGUI txtCombatLeft;
        public TextMeshProUGUI txtCombatRight;
        public TextMeshProUGUI txtRank;
        public GameObject goRankUp;

        public CmpRewardItem pfbItem;

        public override void OnInit(UIParamStruct param)
        {
            bool isWin = (bool)param.param;
            this.goLose.SetActive(!isWin);
            this.goWin.SetActive(isWin);
        }


        public void OnClose1Click()
        {
            UIAreneBattle uIArene = UIManager.I.Get(AddressbalePathEnum.PREFAB_UIBattle) as UIAreneBattle;
            uIArene.OnClose1Click();
            this.CloseSelf();
        }

        /// <summary>
        /// 对手信息
        /// </summary>
        /// <param name="data"></param>
        public void SetTarget(Msg_Data_Arena_Playerinfo data1, Msg_Data_Arena_Playerinfo data2)
        {
            this.avatarLeft.SetInfo(data1.nickname, data1.avatar_url);
            this.avatarRight.SetInfo(data2.nickname, data2.avatar_url);

            this.txtCombatLeft.text = "战斗力:" + MathUtils.ToKBM((long)data1.combats);
            this.txtCombatRight.text = "战斗力:" + MathUtils.ToKBM((long)data2.combats);
        }


        public void SetRankInfo(int beforerank, int newrank)
        {
            this.txtRank.text = "排行榜排名:" + newrank;
            this.goRankUp.SetActive(newrank > beforerank);

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            int myRank = userData.rankInfo.data.current;

            //this.txtRank.text = "" + myRank;

            ExcelData.Arena[] arenas = ConfigManger.I.configS.arenas;

            ExcelData.Arena mydata = null;

            for (int i = 0; i < arenas.Length; i++)
            {
                int fromRank = arenas[i].price[0];
                int toRank = arenas[i].price[1];

                if (myRank >= fromRank && myRank <= toRank)
                {
                    mydata = arenas[i];
                    break;
                }
            }

            if (mydata != null)
            {
                for (int i = 0; i < mydata.goods.Length; i++)
                {
                    int num = mydata.num[i];
                    CmpRewardItem item = Instantiate(this.pfbItem, this.pfbItem.transform.parent);
                    item.gameObject.SetActive(true);

                    item.SetAsset(DealUtils.toAssetEnum(mydata.goods[i]), num);
                }
            }


        }

    }

}
