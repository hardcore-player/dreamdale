using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.AddressableAssets;
using Deal.UI;
using Deal.Data;
using Druid.Utils;

namespace Deal.Env
{
    /// <summary>
    /// 马车
    /// </summary>
    public class Building_Wagon : BuildingBase
    {
        public GameObject showGo;

        public SpriteRenderer srAsset;

        public Sprite spAssetApple;
        public Sprite spAssetPumpkin;
        public Sprite spAssetStone;
        public Sprite spAssetWood;
        public Sprite spAssetFish;
        public Sprite spAssetWool;
        public Sprite spAssetGrain;
        public Sprite spAssetIron;
        public Sprite spAssetCactus;
        public Sprite spAssetCarrot;
        public Sprite spAssetPotion;
        public Sprite spAssetWinterWood;
        public Sprite spAssetCone;
        public Sprite spAssetDeadWood;
        public Sprite spAssetBamboo;
        public Sprite spAssetOrange;
        public Sprite spAssetDeadWoodPlank;

        public CmpAssetItem cmpAsset;

        public BuildingStateEnum StateEnum = BuildingStateEnum.None;

        /// <summary>
        /// 建筑的广告配置
        /// </summary>
        public override BuildingAdConfig GetAdConfig()
        {
            Data_Wagon _Data = this.GetData<Data_Wagon>();

            if (_Data == null) return null;

            BuildingAdConfig config = new BuildingAdConfig();
            config.refreshSecond = 60;
            config.rewardAsset = _Data.AssetId;
            config.assetNum = _Data.AssetTotal;
            config.onAdCallback = () =>
            {
                _Data.StateEnum = BuildingStateEnum.Building;
                _Data.CDAt = TimeUtils.TimeNowMilliseconds();
                this.UpdateView();
                DataManager.I.Save(DataDefine.MapData);
                DataManager.I.Save(DataDefine.UserData);
            };
            config.onAdRestore = null;
            return config;
        }

        public override void UpdateView()
        {
            //Data_Storage data_ = this.GetData<Data_>();
            //this.assetList.SetAssets(data_.Assets);

            Data_Wagon _Data = this.GetData<Data_Wagon>();

            if (_Data.AssetId == AssetEnum.Wood)
            {
                this.srAsset.sprite = this.spAssetWood;
            }
            else if (_Data.AssetId == AssetEnum.Stone)
            {
                this.srAsset.sprite = this.spAssetStone;
            }
            else if (_Data.AssetId == AssetEnum.Apple)
            {
                this.srAsset.sprite = this.spAssetApple;
            }
            else if (_Data.AssetId == AssetEnum.Pumpkin)
            {
                this.srAsset.sprite = this.spAssetPumpkin;
            }
            else if (_Data.AssetId == AssetEnum.Iron)
            {
                this.srAsset.sprite = this.spAssetIron;
            }
            else if (_Data.AssetId == AssetEnum.Fish)
            {
                this.srAsset.sprite = this.spAssetFish;
            }
            else if (_Data.AssetId == AssetEnum.Grain)
            {
                this.srAsset.sprite = this.spAssetGrain;
            }
            else if (_Data.AssetId == AssetEnum.Cactus)
            {
                this.srAsset.sprite = this.spAssetCactus;
            }
            else if (_Data.AssetId == AssetEnum.Potion)
            {
                this.srAsset.sprite = this.spAssetPotion;
            }
            else if (_Data.AssetId == AssetEnum.Cone)
            {
                this.srAsset.sprite = this.spAssetCone;
            }
            else if (_Data.AssetId == AssetEnum.WinterWood)
            {
                this.srAsset.sprite = this.spAssetWinterWood;
            }
            else if (_Data.AssetId == AssetEnum.DeadWood)
            {
                this.srAsset.sprite = this.spAssetDeadWood;
            }
            else if (_Data.AssetId == AssetEnum.Bamboo)
            {
                this.srAsset.sprite = this.spAssetBamboo;
            }
            else if (_Data.AssetId == AssetEnum.Orange)
            {
                this.srAsset.sprite = this.spAssetOrange;
            }
            else if (_Data.AssetId == AssetEnum.DeadWoodPlank)
            {
                this.srAsset.sprite = this.spAssetDeadWoodPlank;
            }
            else if (_Data.AssetId == AssetEnum.Carrot)
            {
                this.srAsset.sprite = this.spAssetCarrot;
            }
            else if (_Data.AssetId == AssetEnum.Wool)
            {
                this.srAsset.sprite = this.spAssetWool;
            }

            cmpAsset.SetAsset(_Data.AssetId, "+" + _Data.AssetTotal);


            if (_Data.StateEnum == BuildingStateEnum.Building)
            {
                this.showGo.SetActive(false);
            }
            else if (_Data.StateEnum == BuildingStateEnum.Open)
            {
                this.showGo.SetActive(true);
            }
        }


        public override void OnUpdate()
        {
            base.OnUpdate();

            if (this.Data != null)
            {
                this.Data.Update();

                Data_Wagon _Data = this.GetData<Data_Wagon>();

                if (this.StateEnum != _Data.StateEnum)
                {
                    this.StateEnum = _Data.StateEnum;
                    this.UpdateView();
                }
            }
        }

    }
}

