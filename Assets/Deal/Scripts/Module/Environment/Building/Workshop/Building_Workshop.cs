using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.UI;
using Deal.UI;
using Deal.Data;
using TMPro;
using Druid.Utils;
using System;
using DG.Tweening;

namespace Deal.Env
{
    /// <summary>
    /// 工坊
    /// </summary>
    public class Building_Workshop : BuildingUnclockWithAsset
    {
        public DropTool dropTool;
        public AssetList assetList;
        public BoxCollider2D boxCollider2D;

        public SpriteRenderer srTool;

        public GameObject toolGo;
        public Image toolIcon;
        public Slider sliderTool;

        public GameObject cdGo;
        public TextMeshProUGUI txtCd;


        /// <summary>
        /// 解锁一把工具
        /// </summary>
        public void UnlockTool()
        {
            this.UpdateView();
        }

        public override void UpdateView()
        {
            Data_Workshop data_ = this.GetData<Data_Workshop>();
            if (data_.ToolEnum != WorkshopToolEnum.None)
            {
                this.InitPrice(data_.Price);

                this.srTool.gameObject.SetActive(true);
                SpriteUtils.SetToolSprite(this.srTool, data_.ToolEnum);
                SpriteUtils.SetToolSprite(this.toolIcon, data_.ToolEnum);

                this.UpdateSlider();
            }

            else if (data_.EquipId > 0 && data_.EquipPrice > 0)
            {
                // 武器
                if (data_.InCd == true)
                {
                    this.cdGo.SetActive(true);
                    this.toolGo.SetActive(false);
                    this.assetList.gameObject.SetActive(false);
                }
                else
                {
                    this.InitPrice(data_.EPrice);

                    this.srTool.gameObject.SetActive(true);
                    SpriteUtils.SetEquipIcon(this.srTool, data_.EquipId);
                    SpriteUtils.SetEquipIcon(this.toolIcon, data_.EquipId);

                    this.UpdateSlider();
                }
            }

            else
            {
                this.boxCollider2D.enabled = false;
                this.assetList.gameObject.SetActive(false);
                this.toolGo.SetActive(false);
                this.cdGo.SetActive(false);

                this.srTool.gameObject.SetActive(false);
            }
        }


        private void InitPrice(List<Data_GameAsset> Price)
        {
            this.boxCollider2D.enabled = true;
            this.assetList.gameObject.SetActive(true);
            this.toolGo.SetActive(true);
            this.cdGo.SetActive(false);
            this.assetList.Clear();
            this.assetList.SetAssets(Price);

            foreach (var item in Price)
            {
                if (item.assetNum > 0)
                {
                    this.assetList.UpdateAssets(item);
                }
            }
        }

        private void UpdateSlider()
        {
            Data_Workshop data_ = this.GetData<Data_Workshop>();

            int priceGoldLeft = 0;

            if (data_.ToolEnum != WorkshopToolEnum.None)
            {
                // 工具
                for (int i = 0; i < data_.Price.Count; i++)
                {
                    priceGoldLeft += data_.Price[i].assetNum;
                }
                this.sliderTool.value = (data_.ToolPrice - priceGoldLeft) / 1f / data_.ToolPrice;
            }
            else if (data_.EquipId > 0)
            {
                // 武器
                for (int i = 0; i < data_.EPrice.Count; i++)
                {
                    priceGoldLeft += data_.EPrice[i].assetNum;
                }

                Debug.Log($"data_.EquipPrice ={data_.EquipPrice} priceGoldLeft ={priceGoldLeft} data_.EquipPrice ={data_.EquipPrice}");
                this.sliderTool.value = (data_.EquipPrice - priceGoldLeft) / 1f / data_.EquipPrice;
            }
        }

        /// <summary>
        /// 更新价格
        /// </summary>
        /// <param name="asset"></param>
        public override void UpdatePrice(Data_GameAsset asset)
        {
            this.assetList.UpdateAssets(asset);

            Data_Workshop data_ = this.GetData<Data_Workshop>();
            if (data_.ToolEnum != WorkshopToolEnum.None)
            {
                this.UpdateSlider();
            }
            else if (data_.EquipId > 0)
            {
                this.UpdateSlider();
            }
        }


        public override void Unclock()
        {
            base.Unclock();

            if (this.Data == null)
            {
                return;
            }

            Data_Workshop data_ = this.GetData<Data_Workshop>();
            // 解锁工具
            if (data_.ToolEnum != WorkshopToolEnum.None)
            {
                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
                userData.UnlockWorkshopTool(data_.ToolEnum);

                Hero hero = PlayManager.I.mHero;
                DropTool item = Instantiate(this.dropTool);
                item.transform.position = this.center.position;
                item.SetTool(data_.ToolEnum);

                Vector3 rp = Deal.MathUtils.GetBottomHalfCirclePoint(Deal.NumDefine.FallRadius);
                item.Fall2Ground(hero.center.position + rp);

                data_.ToolEnum = WorkshopToolEnum.None;
                data_.Price.Clear();
                this.assetList.Clear();
                this.UpdateView();
            }
            else if (data_.EquipId > 0)
            {
                DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
                dungeonData.GetEquipToBag(new Data_Equip(data_.EquipId, 1, 1));

                TaskManager.I.OnTaskEquip(data_.EquipId + "");

                Hero hero = PlayManager.I.mHero;
                DropTool item = Instantiate(this.dropTool);
                item.transform.position = this.center.position;
                item.SetEquip(data_.EquipId);

                Vector3 rp = Deal.MathUtils.GetBottomHalfCirclePoint(Deal.NumDefine.FallRadius);
                item.Fall2Ground(hero.center.position + rp);

                this.NewEquip();

                this.boxCollider2D.enabled = false;

                Sequence sequence = DOTween.Sequence();
                sequence.AppendInterval(1);
                sequence.AppendCallback(() =>
                {
                    this.assetList.Clear();
                    this.UpdateView();
                    this.boxCollider2D.enabled = true;
                });

            }
        }

        private void NewEquip()
        {
            Data_Workshop data_ = this.GetData<Data_Workshop>();
            int nextEquip = data_.EquipId + 1;
            data_.EquipId = nextEquip;

            ExcelData.Equip equip = ConfigManger.I.GetEquipCfg(nextEquip);
            if (equip != null)
            {
                data_.EPrice.Clear();
                this.assetList.Clear();
                int priceNum = 0;
                for (int i = 0; i < equip.assets.Length; i++)
                {
                    AssetEnum asset = (AssetEnum)Enum.Parse(typeof(AssetEnum), equip.assets[i]);
                    int assetNum = (int)equip.num[i];
                    priceNum += assetNum;
                    data_.EPrice.Add(new Data_GameAsset(asset, assetNum));

                }
                data_.EquipPrice = priceNum;

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

                data_.EPrice.Clear();
            }

            DataManager.I.Save(DataDefine.UserData);
            DataManager.I.Save(DataDefine.MapData);
        }


        public override void OnUpdate()
        {
            base.OnUpdate();

            Data_Workshop data_ = this.GetData<Data_Workshop>();

            bool cdChangeed = data_.RefreshCommonState();
            if (data_.InCd == true)
            {
                float left = TimeUtils.TimeNowMilliseconds() - data_.CommonCDAt;
                this.txtCd.text = TimeUtils.SecondsFormat(data_.CommonRefreshNeed - (int)left / 1000);
            }

            if (cdChangeed)
            {
                this.UpdateView();
            }

        }

    }
}

