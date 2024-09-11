using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using Deal.Data;
using UnityEngine.AddressableAssets;
using Deal.UI;
using System;
using DG.Tweening;
using Druid.Utils;
using TMPro;

namespace Deal.Env
{
    /// <summary>
    /// 武器库
    /// </summary>
    public class Building_Armory : BuildingUnclockWithAsset
    {
        public DropTool dropTool;
        public AssetList assetList;
        public BoxCollider2D boxCollider2D;

        public GameObject toolGo;
        public Image toolIcon;
        public Slider sliderTool;

        public GameObject cdGo;
        public TextMeshProUGUI txtCd;


        public override void OnHeroEnter(Hero mHero)
        {
            NetUtils.doPostArenaCombat((yes) =>
            {
                if (yes)
                {
                    Hero hero = PlayManager.I.mHero;
                    hero.Controller.SetStatePause();
                    PlayManager.I.enterDungeonPos = hero.transform.position;
                    PlayManager.I.LoadPvpScene();
                }
                else
                {
                    Debug.Log("doPostArenaCombat failed");
                }

            });

        }


        //public override void OnUpdate()
        //{
        //    base.OnUpdate();

        //    Data_Armory data_ = this.GetData<Data_Armory>();

        //    bool cdChangeed = data_.RefreshCommonState();
        //    if (data_.InCd == true)
        //    {
        //        float left = TimeUtils.TimeNowMilliseconds() - data_.CommonCDAt;
        //        this.txtCd.text = TimeUtils.SecondsFormat(data_.CommonRefreshNeed - (int)left / 1000);
        //    }

        //    if (cdChangeed)
        //    {
        //        this.UpdateView();
        //    }

        //}
        ///// <summary>
        ///// 解锁一把工具
        ///// </summary>
        //public void UnlockTool()
        //{
        //    this.UpdateView();
        //}


        public override void UpdateView()
        {
            Data_Armory data_ = this.GetData<Data_Armory>();

            //if (data_.InCd == true)
            //{
            //    this.cdGo.SetActive(true);
            //    this.toolGo.SetActive(false);
            //    this.assetList.gameObject.SetActive(false);

            //    return;
            //}

            //this.cdGo.SetActive(false);
            //this.toolGo.SetActive(true);
            //this.assetList.gameObject.SetActive(true);

            //if (data_.EquipId > 0 && data_.ToolPrice > 0)
            //{
            //    this.boxCollider2D.enabled = true;
            //    this.assetList.gameObject.SetActive(true);
            //    this.toolGo.SetActive(true);
            //    this.assetList.SetAssets(data_.Price);

            //    foreach (var item in data_.Price)
            //    {
            //        if (item.assetNum > 0)
            //        {
            //            this.assetList.UpdateAssets(item);
            //        }
            //    }

            //    SpriteUtils.SetEquipIcon(this.toolIcon, data_.EquipId);

            //    this.UpdateSlider();
            //}
            //else
            //{
            //    this.boxCollider2D.enabled = false;
            //    this.assetList.gameObject.SetActive(false);
            //    this.toolGo.SetActive(false);
            //}
        }

        //private void UpdateSlider()
        //{
        //    Data_Armory data_ = this.GetData<Data_Armory>();

        //    int priceGoldLeft = 0;

        //    for (int i = 0; i < data_.Price.Count; i++)
        //    {
        //        priceGoldLeft += data_.Price[i].assetNum;
        //    }

        //    this.sliderTool.value = (data_.ToolPrice - priceGoldLeft) / 1f / data_.ToolPrice;
        //}

        /// <summary>
        /// 更新价格
        /// </summary>
        /// <param name="asset"></param>
        //public override void UpdatePrice(Data_GameAsset asset)
        //{
        //    this.assetList.UpdateAssets(asset);

        //    Data_Armory data_ = this.GetData<Data_Armory>();
        //    if (data_.EquipId > 0)
        //    {
        //        this.UpdateSlider();
        //    }
        //}


        public override void Unclock()
        {
            base.Unclock();

            if (this.Data == null)
            {
                return;
            }

            //Data_Armory data_ = this.GetData<Data_Armory>();
            //// 解锁工具
            //if (data_.EquipId > 0)
            //{
            //    //UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            //    DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            //    //userData.UnlockWorkshopTool(data_.ToolEnum);
            //    dungeonData.GetEquipToBag(new Data_Equip(data_.EquipId, 1, 1));

            //    TaskManager.I.OnTaskEquip(data_.EquipId + "");

            //    Hero hero = PlayManager.I.mHero;
            //    DropTool item = Instantiate(this.dropTool);
            //    item.transform.position = this.center.position;
            //    item.SetEquip(data_.EquipId);

            //    Vector3 rp = Deal.MathUtils.GetBottomHalfCirclePoint(Deal.NumDefine.FallRadius);
            //    item.Fall2Ground(hero.center.position + rp);

            //    this.NewEquip();

            //    this.boxCollider2D.enabled = false;

            //    Sequence sequence = DOTween.Sequence();
            //    sequence.AppendInterval(1);
            //    sequence.AppendCallback(() =>
            //    {
            //        this.UpdateView();
            //        this.boxCollider2D.enabled = true;
            //    });

            //}
        }


        private void NewEquip()
        {
            Data_Armory data_ = this.GetData<Data_Armory>();
            int nextEquip = data_.EquipId + 1;
            data_.EquipId = nextEquip;

            ExcelData.Equip equip = ConfigManger.I.GetEquipCfg(nextEquip);
            if (equip != null)
            {
                data_.Price.Clear();
                this.assetList.Clear();
                int priceNum = 0;
                for (int i = 0; i < equip.assets.Length; i++)
                {
                    AssetEnum asset = (AssetEnum)Enum.Parse(typeof(AssetEnum), equip.assets[i]);
                    int assetNum = (int)equip.num[i];
                    priceNum += assetNum;
                    data_.Price.Add(new Data_GameAsset(asset, assetNum));

                }
                data_.ToolPrice = priceNum;

                if (Config.IsDebug())
                {
                    data_.CommonRefreshNeed = 10;//cd时间
                }
                else
                {
                    data_.CommonRefreshNeed = 5 * 60;//cd时间
                }
                data_.CommonCDAt = TimeUtils.TimeNowMilliseconds();
                data_.InCd = true;
            }
            else
            {

                data_.Price.Clear();
            }

            DataManager.I.Save(DataDefine.UserData);
            DataManager.I.Save(DataDefine.MapData);
        }

    }
}

