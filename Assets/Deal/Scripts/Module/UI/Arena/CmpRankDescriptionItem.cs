using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Msg;
using DG.Tweening;
using TMPro;

namespace Deal.UI
{
    public class CmpRankDescriptionItem : UIBase
    {

        public TextMeshProUGUI txtRank;
        public CmpRewardItem pfbItem;

        public void SetData(ExcelData.Arena data)
        {
            int fromRank = data.price[0];
            int toRank = data.price[1];

            if (fromRank == toRank)
            {
                this.txtRank.text = "" + fromRank;
            }
            else
            {
                this.txtRank.text = fromRank + "-" + toRank;
            }

            for (int i = 0; i < data.goods.Length; i++)
            {
                int num = data.num[i];
                CmpRewardItem item = Instantiate(this.pfbItem, this.pfbItem.transform.parent);
                item.gameObject.SetActive(true);

                item.SetAsset(DealUtils.toAssetEnum(data.goods[i]), num);
            }

        }

        public void SetMyData()
        {

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            int myRank = userData.rankInfo.data.current;

            this.txtRank.text = "" + myRank;

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
