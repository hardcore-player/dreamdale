using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using ExcelData;
using Deal;
using Deal.Data;
using TMPro;
using System;

namespace Deal.UI
{
    /// <summary>
    /// 市场的弹出界面
    /// </summary>
    public class CmpHallLvUpem : MonoBehaviour
    {
        public TextMeshProUGUI txtInfo;
        public TextMeshProUGUI txtVal;
        public TextMeshProUGUI txtLv;
        public TextMeshProUGUI txtGold;
        public CanvasGroup canvasGroup;
        public GameObject btnUnlock;
        public Image imgIcon;

        // 当前任务
        public GameObject bg0;
        public GameObject bg1;

        private HallAbilityEnum _abilityEnum;

        private void Start()
        {
            Druid.Utils.UIUtils.AddBtnClick(transform, "Unlock/btnSell", this.OnUpgradeClick);
        }

        public void SetData(HallAbilityEnum abilityEnum)
        {
            this._abilityEnum = abilityEnum;
            this.UpdateUI();
        }


        void UpdateUI()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            int cLv = userData.GetHallAbilityLv(this._abilityEnum);
            HallUpgrade hallUpgrade = ConfigManger.I.GetHallAbilityCfg(this._abilityEnum);

            SpriteUtils.SetHallAbilitySprite(this.imgIcon, this._abilityEnum);

            //float cVal = hallUpgrade.baseVal + hallUpgrade.upgrade * cLv;
            //float nVal = hallUpgrade.baseVal + hallUpgrade.upgrade * (cLv + 1);

            float cVal = MathUtils.GetHallabilityValue(this._abilityEnum, cLv, hallUpgrade.baseVal, hallUpgrade.upgrade);
            float nVal = MathUtils.GetHallabilityValue(this._abilityEnum, cLv + 1, hallUpgrade.baseVal, hallUpgrade.upgrade);

            this.txtLv.text = $"Lv{cLv + 1}";
            this.txtInfo.text = hallUpgrade.chineseName;
            this.txtVal.text = $"<color=#8E5313>由</color>  <color=#DB3423>{cVal}</color> <color=#8E5313>提升至</color> <color=#44DB22>{nVal}</color>";

            int gooldLv = cLv > hallUpgrade.priceArr.Length - 1 ? hallUpgrade.priceArr.Length - 1 : cLv;
            //if (hallUpgrade.priceArr.Length > cLv)
            //{
            int needGold = hallUpgrade.priceArr[gooldLv];
            this.txtGold.text = needGold + "";
            //}
            //else
            //{
            //    // 满级
            //    this.txtGold.text = "--";
            //}

            Data_Task _Task = TaskManager.I.GetTask();

            if (_Task.TaskId >= hallUpgrade.unlock - 1)
            {
                Debug.Log("Open");
                this.canvasGroup.alpha = 1;
                this.btnUnlock.SetActive(false);
            }
            else
            {
                Debug.Log("Close");
                this.canvasGroup.alpha = 0.2f;
                this.btnUnlock.SetActive(true);
            }

            if (_Task.TargetType == this._abilityEnum.ToString())
            {
                this.bg0.SetActive(true);
            }

        }

        /// <summary>
        /// 点击升级
        /// </summary>
        void OnUpgradeClick()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            int cLv = userData.Data.GetHallAbilityLv(this._abilityEnum);
            HallUpgrade hallUpgrade = ConfigManger.I.GetHallAbilityCfg(this._abilityEnum);

            //if (hallUpgrade.priceArr.Length > cLv)
            //{
            int gooldLv = cLv > hallUpgrade.priceArr.Length - 1 ? hallUpgrade.priceArr.Length - 1 : cLv;
            int needGold = hallUpgrade.priceArr[gooldLv];

            if (userData.CostAsset(AssetEnum.Gold, (int)needGold))
            {
                userData.UpHallAbilityLv(this._abilityEnum);

                this.UpdateUI();
                // 任务
                TaskManager.I.OnTaskUpgrade(this._abilityEnum, cLv + 1);
                // 保存
                DataManager.I.Save(DataDefine.UserData);
                DataManager.I.Save(DataDefine.MapData);
            }
            else
            {
                ShopUtils.pushShop();
            }
            //}

        }

    }
}

