using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using TMPro;
using Deal.Env;
using Deal.Data;

namespace Deal.UI
{
    public class CmpResearchLabItem : MonoBehaviour
    {
        public GameObject btnSell;
        public GameObject btnUnlock;
        public TextMeshProUGUI txtInfo;
        public TextMeshProUGUI txtPrice;
        public Image imgIcon;

        public GameObject goLeftB;
        public GameObject goLeft;
        public GameObject goRight;
        public GameObject goTop;
        public GameObject goCenter;
        public GameObject goCenterB;

        private ExcelData.Research data;

        private void Awake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "btnSell", this.OnSellClick);
        }

        public void SetData(ExcelData.Research data)
        {
            this.data = data;

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            BluePrintEnum bluePrint = (BluePrintEnum)Enum.Parse(typeof(BluePrintEnum), data.name);

            this.txtInfo.text = data.chineseName;
            this.txtPrice.text = data.gem + "";

            SpriteUtils.SetSprite(AddressbalePathEnum.SPRITEATLAS_AtlasLab, this.imgIcon, this.getIcon()); ;

            bool hasBluePrint = userData.HasBulePrint(bluePrint);
            if (hasBluePrint)
            {
                // 解锁了
                this.btnSell.SetActive(false);
                this.btnUnlock.SetActive(false);

                this.goLeftB.SetActive(false);
                this.goCenterB.SetActive(true);
                this.goLeft.SetActive(false);
                this.goCenter.SetActive(true);
                this.goTop.SetActive(true);
                this.goRight.SetActive(true);
            }
            else
            {
                // 未解锁
                Data_Task _Task = TaskManager.I.GetTask();
                if (_Task != null && _Task.TaskId >= data.unlock)
                {
                    // 可以卖
                    this.btnSell.SetActive(true);
                    this.btnUnlock.SetActive(false);

                    this.goLeftB.SetActive(true);
                    this.goCenterB.SetActive(true);
                    this.goLeft.SetActive(true);
                    this.goCenter.SetActive(true);
                    this.goTop.SetActive(true);
                    this.goRight.SetActive(true);
                }
                else
                {
                    this.btnSell.SetActive(false);
                    this.btnUnlock.SetActive(true);

                    this.goLeftB.SetActive(true);
                    this.goCenterB.SetActive(true);
                    this.goLeft.SetActive(false);
                    this.goCenter.SetActive(false);
                    this.goTop.SetActive(false);
                    this.goRight.SetActive(false);
                }

            }
        }


        public void OnSellClick()
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            BluePrintEnum bluePrint = (BluePrintEnum)Enum.Parse(typeof(BluePrintEnum), data.name);

            bool hasBluePrint = userData.HasBulePrint(bluePrint);
            if (!hasBluePrint)
            {
                bool ok = userData.CostAsset(AssetEnum.Gem, this.data.gem);
                if (ok)
                {
                    userData.UnlockBulePrint(bluePrint);
                    this.SetData(this.data);

                    TaskManager.I.OnTaskResearch(data.name);
                    userData.Save();
                }
            }
        }

        private string getIcon()
        {
            if (this.data.name == BuildingEnum.Museum.ToString())
            {
                return "img_lab_buildicon1";
            }
            else if (this.data.name == BuildingEnum.FarmerHouse.ToString())
            {
                return "img_lab_buildicon2";
            }
            else if (this.data.name == BuildingEnum.Ship.ToString())
            {
                return "img_lab_buildicon3";
            }
            else if (this.data.name == BuildingEnum.Lighthouse.ToString())
            {
                return "img_lab_buildicon4";
            }
            else if (this.data.name == BuildingEnum.GemTower.ToString())
            {
                return "img_lab_buildicon5";
            }
            else if (this.data.name == BuildingEnum.Library.ToString())
            {
                return "img_lab_buildicon6";
            }
            else if (this.data.name == BuildingEnum.Academy.ToString())
            {
                return "img_lab_buildicon7";
            }
            else if (this.data.name == BuildingEnum.ChickenFarm.ToString())
            {
                return "img_lab_buildicon8";
            }
            else
            {
                return "img_lab_buildicon1";
            }

        }

    }

}
