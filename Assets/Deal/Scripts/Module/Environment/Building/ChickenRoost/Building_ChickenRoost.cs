using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.UI;
using Deal.UI;
using Deal.Data;
using Deal.Tools;
using ExcelData;
using DG.Tweening;
using Druid.Utils;

namespace Deal.Env
{
    /// <summary>
    /// 鸡舍
    /// </summary>
    public class Building_ChickenRoost : BuildingBase
    {
        //public Transform sheepHome;
        //public Transform sheepEnter;
        //public Transform sheepHide;
        protected Hero mHero;
        public DropPropPlank pfbProp;
        private float _flyInterval;
        private float _flyTime = 0.1f;
        private AssetFlyNumTool _flyNumTool;


        public List<GameObject> eggList = new List<GameObject>();
        public Dictionary<int, FarmerHouseChiken> sheepList = new Dictionary<int, FarmerHouseChiken>();

        private void Awake()
        {
            for (int i = 0; i < 4; i++)
            {
                sheepList.Add(i, null);
            }

        }

        /// <summary>
        /// 建筑的广告配置
        /// </summary>
        public override BuildingAdConfig GetAdConfig()
        {
            Data_ChickenRoost _Data = this.GetData<Data_ChickenRoost>();
            ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.ChickenRoost.ToString());

            BuildingAdConfig config = new BuildingAdConfig();
            config.refreshSecond = 60;
            config.rewardAsset = AssetEnum.Egg;
            config.assetNum = resBuilding.ad;
            config.onAdCallback = null;
            config.onAdRestore = null;

            return config;
        }

        public override void UpdateView()
        {
            //base.UpdateView();
            //this.UpdateSlider();
            //this.UpdateWood();
            //this.UpdatePlank();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            Data_ChickenRoost data_ = this.GetData<Data_ChickenRoost>();

            // 生产
            if (data_ != null)
            {
                data_.Update();
                this.UpdateView();

                //this.checkAnimation();
            }

            if (mHero)
            {
                this._flyInterval += Time.deltaTime;
                if (this._flyInterval >= this._flyTime)
                {
                    PutWood2Factory();
                    this._flyInterval = 0;
                }

            }


            for (int i = 0; i < data_.RoostItem.Count; i++)
            {
                Data_ChickenRoostItem itemData = data_.RoostItem[i];
                FarmerHouseChiken itemChiken = this.sheepList[i];
                if (itemData.State == CollectableResState.None)
                {
                    // 没有
                    // 鸡蛋
                    this.eggList[i].SetActive(false);
                }
                else if (itemData.State == CollectableResState.CD)
                {
                    // cd

                    if (itemChiken == null)
                    {
                        //羊圈的羊帮过来
                        Building_ChickenFarm building_FarmerHouse = MapManager.I.GetSingleBuilding(BuildingEnum.ChickenFarm) as Building_ChickenFarm;
                        if (building_FarmerHouse != null)
                        {
                            FarmerHouseChiken sheep = building_FarmerHouse.GetIdleSheep();
                            if (sheep != null)
                            {
                                sheep.sheepState = FarmerHouseSheepState.NONE;
                                this.putChicken2Egg(sheep, i);
                                itemChiken = sheep;
                            }

                        }
                    }

                    float progress = data_.RefreshStateId(i);
                    if (itemChiken != null)
                    {
                        itemChiken.UpdateSlider(progress);
                    }

                    // 鸡蛋
                    this.eggList[i].SetActive(false);
                    if (itemData.State == CollectableResState.DONE)
                    {
                        if (itemChiken != null)
                        {

                            // cd 结束
                            itemChiken.SetWoolState(false);
                            itemChiken.GoHome();
                            // 显示鸡蛋
                            this.eggList[i].SetActive(true);
                        }

                    }
                }
                else if (itemData.State == CollectableResState.DONE)
                {
                    // done 鸡蛋
                    // 显示鸡蛋
                    this.eggList[i].SetActive(true);
                }
            }
        }


        public void PutWood2Factory()
        {
            if (mHero == null) return;
            if (this.Data == null) return;

            Data_ChickenRoost _Data = this.GetData<Data_ChickenRoost>();

            // 更新资产
            if (mHero.FarmCharacter.HasChiken())
            {
                for (int i = 0; i < _Data.RoostItem.Count; i++)
                {
                    Data_ChickenRoostItem itemData = _Data.RoostItem[i];
                    if (itemData.State == CollectableResState.None)
                    {
                        // 没有
                        FarmerHouseChiken sheep = mHero.FarmCharacter.houseChikens[0];
                        this.putChicken2Egg(sheep, i);
                        // 身上删除一只鸡
                        mHero.FarmCharacter.ChikenPop();
                        break;
                    }
                }
            }

        }

        /// <summary>
        /// 把一只鸡，放到制定的鸡窝
        /// </summary>
        /// <param name="chiken"></param>
        /// <param name="id"></param>
        public void putChicken2Egg(FarmerHouseChiken chiken, int id)
        {
            Data_ChickenRoost _Data = this.GetData<Data_ChickenRoost>();

            Data_ChickenRoostItem itemData = _Data.RoostItem[id];

            chiken.sheepState = FarmerHouseSheepState.NONE;

            itemData.State = CollectableResState.CD;
            itemData.CDAt = TimeUtils.TimeNowMilliseconds();

            chiken.SetWoolState(false);
            chiken.ShowBody();

            this.sheepList[id] = chiken;

            chiken.SetIdle();
            chiken.transform.position = this.eggList[id].transform.position;

        }

        /// <summary>
        /// 玩家捡鸡蛋
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public DropPropPlank OnHeroPick(int i)
        {
            // 隐藏对应的鸡蛋
            this.eggList[i].SetActive(false);

            // 建一个预知体
            DropPropPlank item = Instantiate(this.pfbProp, this.pfbProp.transform.parent);
            item.gameObject.SetActive(true);
            item.transform.position = this.eggList[i].transform.position;

            item.SetProp(AssetEnum.Egg, 1);
            item.OnFall2Ground();

            Data_ChickenRoost _Data = this.GetData<Data_ChickenRoost>();
            Data_ChickenRoostItem itemData = _Data.RoostItem[i];
            itemData.State = CollectableResState.None;

            return item;
        }

        public override void OnHeroEnter(Hero mHero)
        {
            this.mHero = mHero;
            if (this._flyNumTool == null)
            {
                this._flyNumTool = new AssetFlyNumTool();
            }
            this._flyNumTool.stop();
        }


        public override void OnHeroExit(Hero mHero)
        {
            this.mHero = null;
        }

    }
}

