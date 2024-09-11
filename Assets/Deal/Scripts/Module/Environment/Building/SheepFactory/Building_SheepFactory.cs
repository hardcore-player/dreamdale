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

namespace Deal.Env
{
    /// <summary>
    /// 羊毛厂
    /// </summary>
    public class Building_SheepFactory : BuildingChangeFactory
    {
        public Transform sheepHome;
        public Transform sheepEnter;
        public Transform sheepHide;

        public List<FarmerHouseSheep> sheepList = new List<FarmerHouseSheep>();

        /// <summary>
        /// 建筑的广告配置
        /// </summary>
        public override BuildingAdConfig GetAdConfig()
        {
            Data_SheepFactory _Data = this.GetData<Data_SheepFactory>();
            ResBuilding resBuilding = ConfigManger.I.GetResBuildingCfg(BuildingEnum.SheepFactory.ToString());

            BuildingAdConfig config = new BuildingAdConfig();
            config.refreshSecond = 60;
            config.rewardAsset = _Data.ToAsset;
            config.assetNum = resBuilding.ad;
            config.onAdCallback = null;
            config.onAdRestore = null;
            return config;
        }


        public override void OnUpdate()
        {
            base.OnUpdate();

            DataChangeFactory data_ = this.GetData<DataChangeFactory>();

            if (data_.FromNum < this.sheepList.Count)
            {
                // 羊回家
                this.ProdutNew();
            }
            else if (data_.FromNum > this.sheepList.Count)
            {
                //羊圈的羊帮过来
                Building_FarmerHouse building_FarmerHouse = MapManager.I.GetSingleBuilding(BuildingEnum.FarmerHouse) as Building_FarmerHouse;
                if (building_FarmerHouse != null)
                {
                    FarmerHouseSheep sheep = building_FarmerHouse.GetIdleSheep();
                    sheep.sheepState = FarmerHouseSheepState.NONE;

                    this.sheepList.Add(sheep);
                    sheep.SetWoolState(false);
                }

            }
        }



        public void ProdutNew()
        {
            if (this.sheepList.Count > 0)
            {
                FarmerHouseSheep sheep = this.sheepList[0];
                this.sheepList.RemoveAt(0);

                Sequence sequence = DOTween.Sequence();
                sequence.Append(sheep.transform.DOMove(this.sheepHome.position, 0.25f));
                sequence.AppendCallback(() =>
                {
                    sheep.SetWoolState(false);
                    sheep.GoHome();
                    sheep.transform.position = this.sheepHome.position;

                    mHero.Controller.SetStateIdle();
                });
            }


        }

        public override void PutWood2Factory()
        {
            if (mHero == null) return;
            if (this.Data == null) return;

            DataChangeFactory _Data = this.GetData<DataChangeFactory>();

            if (_Data.FromNum < _Data.GetFromTotal())
            {
                // 更新资产
                if (mHero.FarmCharacter.HasSheep())
                {
                    mHero.Controller.SetStatePause();

                    FarmerHouseSheep sheep = mHero.FarmCharacter.houseSheep;
                    sheep.sheepState = FarmerHouseSheepState.NONE;


                    Sequence sequence = DOTween.Sequence();
                    sequence.Append(sheep.transform.DOMove(this.sheepEnter.position, 0.5f));
                    sequence.Append(sheep.transform.DOMove(this.sheepHide.position, 0.25f));

                    sequence.AppendCallback(() =>
                    {
                        _Data.FromNum += 1;
                        this.sheepList.Add(sheep);
                        this.UpdateWood();
                        //sheep.SetWoolState(false);
                        mHero.Controller.SetStateIdle();
                    });
                    //sequence.Append(sheep.transform.DOMove(this.sheepHome.position, 0.25f));
                    //sequence.AppendCallback(() =>
                    //{
                    //    sheep.GoHome();
                    //    sheep.transform.position = this.sheepHome.position;

                    //    mHero.Controller.SetStateIdle();
                    //});


                    //Vector3 to = center != null ? center.position : transform.position;
                    //DealUtils.NewDropPropBezierToTarget(_Data.FromAsset, 1, mHero.center.position, to);
                    //SoundManager.I.playEffect(AddressbalePathEnum.WAV_pop_02);



                }

            }
        }

    }
}

