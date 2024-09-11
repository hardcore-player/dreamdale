using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using TMPro;


namespace Deal.UI
{
    public class DataMarketSellItem
    {
        public AssetEnum asset;
        public int count;
        public float unitPrice;
        public float percent;

        public DataMarketSellItem()
        {
        }

        public DataMarketSellItem(AssetEnum asset, int count, float unitPrice)
        {
            this.asset = asset;
            this.count = count;
            this.unitPrice = unitPrice;

            this.percent = 1.0f;
        }
    }

    public class CmpMarketSellItem : MonoBehaviour
    {
        public Image imgSrc;
        public TextMeshProUGUI txtSrc;
        public TextMeshProUGUI txtDst;
        public Slider sliderSell;

        private DataMarketSellItem data;

        private void Start()
        {
            Druid.Utils.UIUtils.AddBtnClick(transform, "Unlock/BtnSell", this.OnSellClick);
            sliderSell.onValueChanged.AddListener(this.OnValueChanged); ;
        }

        public void SetData(DataMarketSellItem data)
        {
            this.data = data;

            this.UpdateUI();
        }

        public void DoSell()
        {
            if (this.data == null) return;

            int trueCount = this.GetTrueSellCount();
            int sellGold = (int)(trueCount * data.unitPrice);

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            userData.AddAsset(AssetEnum.Gold, sellGold);
            userData.AddAsset(this.data.asset, -trueCount);

            this.data.count -= trueCount;

            this.UpdateUI();

            // 任务
            TaskManager.I.OnTaskSell(this.data.asset, trueCount);
        }

        void UpdateUI()
        {
            SpriteUtils.SetAssetSprite(this.imgSrc, data.asset);
            this.txtSrc.text = MathUtils.ToKBM(data.count) + "";
            this.txtDst.text = MathUtils.ToKBM((int)(data.count * data.unitPrice)) + "";

            this.sliderSell.value = 1;
        }

        void OnValueChanged(float value)
        {
            if (this.data == null) return;

            int trueCount = this.GetTrueSellCount();

            this.txtSrc.text = MathUtils.ToKBM(trueCount) + "";
            this.txtDst.text = MathUtils.ToKBM((int)(trueCount * data.unitPrice)) + "";
        }

        void OnSellClick()
        {
            SoundManager.I.playEffect(AddressbalePathEnum.WAV_click_shop);

            this.DoSell();

            //保存
            DataManager.I.Save(DataDefine.UserData);
        }

        public int GetLeftGold()
        {
            return (int)(data.count * data.unitPrice);
        }

        private int GetTrueSellCount()
        {
            int perChange = 1;
            if (data.unitPrice < 1)
            {
                perChange = (int)(1 / data.unitPrice);
            }

            int trueCount = Mathf.FloorToInt(this.sliderSell.value * (this.data.count / perChange)) * perChange;

            return trueCount;
        }
    }

}

