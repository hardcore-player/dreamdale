using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.UI;
using Deal.UI;
using Deal.Data;

namespace Deal.Env
{
    /// <summary>
    /// 宝石塔
    /// </summary>
    public class Building_GemTower : BuildingUnclockWithAsset
    {
        public AssetList assetList;
        public BoxCollider2D boxCollider2D;

        public GameObject toolGo;
        public Image toolIcon;
        public Slider sliderTool;

        /// <summary>
        /// 解锁一把工具
        /// </summary>
        public void UnlockTool()
        {
            this.UpdateView();
        }

        public override void UpdateView()
        {
            Data_GemTower data_ = this.GetData<Data_GemTower>();

            if (data_.TowerAsset != AssetEnum.None)
            {
                this.boxCollider2D.enabled = true;
                this.assetList.gameObject.SetActive(true);
                this.toolGo.SetActive(true);
                this.assetList.SetAssets(data_.Price);

                SpriteUtils.SetAssetSprite(this.toolIcon, data_.TowerAsset);

                this.UpdateSlider();
            }
            else
            {
                this.boxCollider2D.enabled = false;
                this.assetList.gameObject.SetActive(false);
                this.toolGo.SetActive(false);
            }
        }

        private void UpdateSlider()
        {
            Data_GemTower data_ = this.GetData<Data_GemTower>();

            int priceGoldLeft = 0;

            for (int i = 0; i < data_.Price.Count; i++)
            {
                if (data_.Price[i].assetType == data_.TowerAsset)
                {
                    priceGoldLeft = data_.Price[i].assetNum;
                    break;
                }
            }
            this.sliderTool.value = (data_.TowerPrice - priceGoldLeft) / 1f / data_.TowerPrice;

            //TaskManager.I.OnTaskTower(data_.TowerAsset.ToString(), data_.TowerPrice - priceGoldLeft);
        }

        /// <summary>
        /// 更新价格
        /// </summary>
        /// <param name="asset"></param>
        public override void UpdatePrice(Data_GameAsset asset)
        {
            this.assetList.UpdateAssets(asset);

            Data_GemTower data_ = this.GetData<Data_GemTower>();
            if (data_.TowerAsset != AssetEnum.None)
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

            Data_GemTower data_ = this.GetData<Data_GemTower>();
            // 解锁工具
            if (data_.TowerAsset != AssetEnum.None)
            {

                TaskManager.I.OnTaskTower(data_.TowerAsset.ToString(), data_.TowerPrice);

                data_.TowerAsset = AssetEnum.None;
                data_.TowerPrice = 0;
                data_.Price.Clear();
                this.UpdateView();
            }
        }
    }
}

