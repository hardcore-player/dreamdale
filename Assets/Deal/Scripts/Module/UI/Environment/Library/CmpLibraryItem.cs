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
    /// 图书馆界面
    /// </summary>
    public class CmpLibraryItem : MonoBehaviour
    {
        //public TextMeshProUGUI txtTitle;
        public TextMeshProUGUI txtCNum;
        public GameObject txtToNum;
        public TextMeshProUGUI txtTNum;

        public TextMeshProUGUI txtPrice;
        public Image imgPrice;
        public GameObject goPrice;


        public void SetData(int lv, int[] priceValue, float[] cValue)
        {
            // 容量
            if (lv < 4)
            {
                int capacityPrice = priceValue[lv - 1];
                string cNum = cValue[lv - 1] + "";
                string tNum = cValue[lv] + "";

                this.SetProgress(lv, cNum, tNum, capacityPrice);
            }
            else
            {
                string cNum = cValue[lv - 1] + "";
                this.SetFull(lv, cNum);
            }
        }

        public void SetData(int lv, int[] priceValue, int[] cValue)
        {
            // 容量
            if (lv < 4)
            {
                int capacityPrice = priceValue[lv - 1];
                string cNum = cValue[lv - 1] + "";
                string tNum = cValue[lv] + "";

                this.SetProgress(lv, cNum, tNum, capacityPrice);
            }
            else
            {
                string cNum = cValue[lv - 1] + "";
                this.SetFull(lv, cNum);
            }
        }

        private void SetProgress(int lv, string fromnum, string tonum, int price)
        {
            this.txtCNum.text = fromnum + "";
            this.txtTNum.text = tonum + "";

            this.txtToNum.gameObject.SetActive(true);
            this.txtTNum.gameObject.SetActive(true);

            for (int i = 0; i < 4; i++)
            {
                GameObject ok = Druid.Utils.UIUtils.Find(this.transform, "Lv/Lv" + (i + 1) + "/ok");
                ok.SetActive((i + 1) <= lv);
            }

            if (lv < 4)
            {
                this.goPrice.SetActive(true);
                this.txtPrice.text = price + "";
            }
            else
            {
                this.goPrice.SetActive(false);
            }

        }

        private void SetFull(int lv, string cNum)
        {
            this.txtCNum.text = cNum + "";
            this.goPrice.SetActive(false);

            this.txtToNum.gameObject.SetActive(false);
            this.txtTNum.gameObject.SetActive(false);

            for (int i = 0; i < 4; i++)
            {
                GameObject ok = Druid.Utils.UIUtils.Find(this.transform, "Lv/Lv" + (i + 1) + "/ok");
                ok.SetActive(true);
            }
        }
    }
}

