using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using ExcelData;
using Deal;
using TMPro;

namespace Deal.UI
{
    /// <summary>
    /// 雕塑界面
    /// </summary>
    public class UIStatue : UIBase
    {
        public TextMeshProUGUI txtTitle;
        public TextMeshProUGUI txtLvPgs;
        public TextMeshProUGUI txtAtts;
        public TextMeshProUGUI txtPrice;
        public Slider sliderLv;
        public GameObject btnLvup;

        private StatueEnum _statueEnum;

        public override void OnUIAwake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/BtnClose", this.OnCloseClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/pnlOpen/BtnUp/btnUnlock", this.OnUpClick);
        }

        public void SetData(StatueEnum statueEnum)
        {
            this._statueEnum = statueEnum;
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            ExcelData.Statue statueCfg = ConfigManger.I.GetStatueCfg(statueEnum.ToString());

            if (statueCfg == null) return;

            for (int i = 0; i <= 15; i++)
            {
                GameObject item = Druid.Utils.UIUtils.Find(this.transform, "Content/inner/list/item" + i);
                if (item != null)
                {
                    TextMeshProUGUI txtInfo = Druid.Utils.UIUtils.FindCmp<TextMeshProUGUI>(item.transform, "Ok/txtInfo");
                    if (i == 15)
                    {
                        txtInfo.text = $"[满级] 雕像效果+100%";
                    }
                    else
                    {
                        txtInfo.text = $"[{i+1}级] {statueCfg.function}+{this._getLvNum(i)}%";
                    }
                }
            }

            this.RenderItem();
        }

        public void RenderItem()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            ExcelData.Statue statueCfg = ConfigManger.I.GetStatueCfg(this._statueEnum.ToString());

            if (statueCfg == null) return;


            int statueLv = userData.GetStatueLv(this._statueEnum);
            // 等级
            int lv = statueLv / 100;
            int expLv = statueLv % 100;

            this.txtTitle.text = $"{statueCfg.chinese}({lv}级)";
            this.txtLvPgs.text = $"{lv}级:{expLv}/10";
            this.sliderLv.value = expLv / 10f;

            this.txtPrice.text = this._getPrice(lv) + "";

            int hp = MathUtils.GetStatueHp(lv, expLv);
            int atk = MathUtils.GetStatueAtk(lv, expLv);
            this.txtAtts.text = $"攻击力：{atk}  生命值：{hp}";

            if (lv == 15 && expLv == 10)
            {
                this.btnLvup.SetActive(false);
            }
            else
            {
                this.btnLvup.SetActive(true);
            }


            for (int i = 0; i <= 15; i++)
            {
                GameObject item = Druid.Utils.UIUtils.Find(this.transform, "Content/inner/list/item" + i);
                if (item != null)
                {
                    TextMeshProUGUI txtInfo = Druid.Utils.UIUtils.FindCmp<TextMeshProUGUI>(item.transform, "Ok/txtInfo");
                    GameObject ok = Druid.Utils.UIUtils.Find(item.transform, "Ok/ok");

                    if (lv > i || (lv == i && expLv == 10))
                    {
                        // 可以用
                        txtInfo.color = new Color(36 / 255f, 188 / 255f, 51 / 255f);
                        ok.SetActive(true);
                    }
                    else
                    {
                        txtInfo.color = new Color(0, 0, 0);
                        ok.SetActive(false);
                    }
                }
            }

        }



        public void OnUpClick()
        {

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            ExcelData.Statue statueCfg = ConfigManger.I.GetStatueCfg(this._statueEnum.ToString());

            if (statueCfg == null) return;

            int statueLv = userData.GetStatueLv(this._statueEnum);
            // 等级
            int lv = statueLv / 100;
            int expLv = statueLv % 100;

            if (lv >= 15 && expLv >= 10) return;

            int price = this._getPrice(lv);

            bool cost = userData.CostAsset(AssetEnum.AncientShard, price);
            if (cost)
            {
                expLv++;
                if (lv < 15 && expLv >= 10)
                {
                    lv++;
                    expLv = 0;
                }

                userData.UpgradeStatueLv(this._statueEnum, lv, expLv);

                this.RenderItem();
            }

        }

        private int _getLvNum(int lv)
        {
            if (lv == 0)
            {
                return 2;
            }
            else if (lv == 3)
            {
                return 3;
            }
            else if (lv == 6)
            {
                return 4;
            }
            else if (lv == 9)
            {
                return 5;
            }
            else if (lv == 12)
            {
                return 6;
            }

            else
            {
                return 0;
            }
        }

        private int _getPrice(int lv)
        {
            if (lv <= 3)
            {
                return 5;
            }
            else if (lv <= 6)
            {
                return 10;
            }
            else if (lv <= 9)
            {
                return 15;
            }
            else if (lv <= 12)
            {
                return 20;
            }
            else
            {
                return 25;
            }
        }

    }
}

