using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using ExcelData;
using TMPro;
using System;

namespace Deal.UI
{
    /// <summary>
    /// 市场的弹出界面
    /// </summary>
    public class UIMarketPop : UIBase
    {
        public TextMeshProUGUI txtGoldAll;

        public CmpMarketSellItem pfbItem;

        private List<CmpMarketSellItem> listSell = new List<CmpMarketSellItem>();

        public override void OnUIStart()
        {

            Druid.Utils.UIUtils.AddBtnClick(transform, "Content/BtnAll", this.OnSellAllClick);
            Druid.Utils.UIUtils.AddBtnClick(transform, "Content/BtnClose", this.OnCloseClick);
            Druid.Utils.UIUtils.AddBtnClick(transform, "", this.OnCloseClick);

            this.InitSellList();
        }


        public void InitSellList()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            Market[] markets = ConfigManger.I.configS.markets;

            int goldAll = 0;
            for (int i = 0; i < markets.Length; i++)
            {
                Market data = markets[i];
                AssetEnum assetEnum = this.GetAssetEnum(data.name);
                int assetNum = userData.GetAssetNum(assetEnum);
                if (assetNum > 0)
                {
                    int sellGold = Mathf.FloorToInt(assetNum * data.unitPrice);
                    int trueCount = (int)(sellGold / data.unitPrice);

                    CmpMarketSellItem item = Instantiate<CmpMarketSellItem>(this.pfbItem, this.pfbItem.transform.parent);
                    item.gameObject.SetActive(true);
                    item.SetData(new DataMarketSellItem(assetEnum, trueCount, data.unitPrice));
                    listSell.Add(item);

                    goldAll += sellGold;
                }
            }

            //this.txtGoldAll.text = $"x{goldAll}";
        }


        public void OnSellAllClick()
        {
            SoundManager.I.playEffect(AddressbalePathEnum.WAV_click_shop);

            for (int i = 0; i < this.listSell.Count; i++)
            {
                this.listSell[i].DoSell();
            }

            //保存
            DataManager.I.Save(DataDefine.UserData);

            //int goldAll = 0;
            //for (int i = 0; i < this.listSell.Count; i++)
            //{
            //    goldAll += this.listSell[i].GetLeftGold();
            //}
            //this.txtGoldAll.text = $"x{goldAll}";
        }


        public AssetEnum GetAssetEnum(string strName)
        {
            return (AssetEnum)Enum.Parse(typeof(AssetEnum), strName);
        }
    }
}

