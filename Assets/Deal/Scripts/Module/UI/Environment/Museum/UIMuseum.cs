using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using ExcelData;
using Deal;
using Deal.Env;
using Deal.Data;
using DG.Tweening;
using TMPro;

namespace Deal.UI
{

    /// <summary>
    /// 博物馆界面
    /// </summary>
    public class UIMuseum : UIBase
    {
        public GameObject goLock;
        public GameObject goOpen;

        public Image imgPrice;
        public TextMeshProUGUI txtPrice;
        public TextMeshProUGUI txtInfo;
        public TextMeshProUGUI txtTitle;
        public Image imgIcon;

        public GameObject btnRight;
        public GameObject btnLeft;

        private int _pageId = 0;
        private List<StatueEnum> statueEnums = new List<StatueEnum>();

        public override void OnUIStart()
        {
            Druid.Utils.UIUtils.AddBtnClick(transform, "Content/BtnClose", this.OnCloseClick);
            Druid.Utils.UIUtils.AddBtnClick(transform, "Content/BtnRight", this.OnRightClick);
            Druid.Utils.UIUtils.AddBtnClick(transform, "Content/BtnLeft", this.OnLeftClick);
            Druid.Utils.UIUtils.AddBtnClick(transform, "Content/pnlLock/btnUnlock", this.OnLockClick);
            Druid.Utils.UIUtils.AddBtnClick(transform, "Content/pnlOpen/btnLvUp", this.OnLvUpClick);

            foreach (StatueEnum item in Enum.GetValues(typeof(StatueEnum)))
            {
                if (item != StatueEnum.None)
                {
                    statueEnums.Add(item);
                }
            }

            this.RenderPage(this._pageId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        private void RenderPage(int pageId)
        {
            StatueEnum statueEnum = statueEnums[pageId];

            this.btnLeft.SetActive(pageId != 0);
            this.btnRight.SetActive(pageId != this.statueEnums.Count - 1);

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            ExcelData.Statue statueCfg = ConfigManger.I.GetStatueCfg(statueEnum.ToString());

            int xlv = userData.GetStatueLv(statueEnum);
            if (!userData.HasStatueBulePrint(statueEnum))
            {
                xlv = 0;
            }

            int lv = xlv / 100;
            int explv = xlv % 100;

            this.txtTitle.text = $"{statueCfg.chinese}";
            this.txtInfo.text = $"{statueCfg.function} +{MathUtils.GetStatueBuff(statueEnum) * 100}%";

            SpriteUtils.SetStatueSprite(this.imgIcon, statueEnum);

            bool hasOpen = userData.HasStatueBulePrint(statueEnum);
            if (hasOpen)
            {
                // 解锁了
                this.goLock.SetActive(false);
                this.goOpen.SetActive(true);
            }
            else
            {
                // 没解锁
                this.goLock.SetActive(true);
                this.goOpen.SetActive(false);

                AssetEnum asset = DealUtils.toAssetEnum(statueCfg.unlock);
                int needNum = asset == AssetEnum.Gem ? 50 : 30;

                SpriteUtils.SetAssetSprite(this.imgPrice, asset);
                this.txtPrice.text = needNum + "";
            }
        }


        /// <summary>
        /// 解锁
        /// </summary>
        public void OnLockClick()
        {
            StatueEnum statueEnum = statueEnums[this._pageId];
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            ExcelData.Statue statueCfg = ConfigManger.I.GetStatueCfg(statueEnum.ToString());

            AssetEnum asset = DealUtils.toAssetEnum(statueCfg.unlock);

            int needNum = asset == AssetEnum.Gem ? 50 : 30;

            if (userData.CostAsset(asset, needNum))
            {
                userData.UnlockStatueBulePrint(statueEnum);
                userData.Save();

                this.RenderPage(this._pageId);
            }
        }

        /// <summary>
        /// 升级
        /// </summary>
        public void OnLvUpClick()
        {
            this.UpdateTaskGuide();

            this.CloseSelf();
        }

        public void OnRightClick()
        {
            if (this._pageId < this.statueEnums.Count - 1)
            {
                this._pageId++;
                this.RenderPage(this._pageId);
            }
        }

        public void OnLeftClick()
        {
            if (this._pageId > 0)
            {
                this._pageId--;
                this.RenderPage(this._pageId);
            }
        }


        private void UpdateTaskGuide()
        {
            if (App.I.CurScene.sceneName == SceneEnum.dungeon)
            {
                return;
            }

            StatueEnum statueEnum = statueEnums[this._pageId];
            MapRender mapRender = MapManager.I.mapRender;

            // 目标雕塑
            Data_BuildingBase target = null;
            for (int i = 0; i < mapRender.SO_MapData.Builds.Count; i++)
            {
                Data_BuildingBase builds = mapRender.SO_MapData.Builds[i];

                if (builds.BuildingEnum == BuildingEnum.Statue && builds.StatueEnum == statueEnum)
                {
                    target = builds;
                    break;
                }
            }

            DealUtils.Guide2BuildingData(target);

        }

    }
}

