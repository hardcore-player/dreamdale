using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Deal.Data;
using Deal.UI;
using Druid;
using UnityEngine.ResourceManagement.AsyncOperations;
using DG.Tweening;

namespace Deal.Env
{
    /// <summary>
    /// 建造用地
    /// </summary>
    public class Building_DefaultUnlocked : BuildingUnclockWithAsset
    {
        public AssetList assetList;
        public Image imgBlueprint;
        public Image imgStatueprint;

        public override void SetData(Data_SaveBase data)
        {
            base.SetData(data);

            Data_BuildingBase _Data = this.GetData<Data_BuildingBase>();

            this.UnlockTask = _Data.UnlockTask;
            this.buildingEnum = _Data.BuildingEnum;
        }

        public override void UpdateView()
        {
            Data_BuildingBase _Data = this.GetData<Data_BuildingBase>();

            if (_Data.BluePrint != BluePrintEnum.None && _Data.BluePrintPrice > 0)
            {
                this.imgBlueprint.gameObject.SetActive(true);
                this.imgStatueprint.gameObject.SetActive(false);
                this.assetList.gameObject.SetActive(false);

                //SpriteUtils.SetBlueprintSprite(this.imgBlueprint, _Data.BluePrint);
            }
            else if (_Data.StatueEnum != StatueEnum.None && _Data.StatuePrice > 0)
            {
                this.imgStatueprint.gameObject.SetActive(true);
                this.imgBlueprint.gameObject.SetActive(false);
                this.assetList.gameObject.SetActive(false);

                //SpriteUtils.SetBlueprintSprite(this.imgBlueprint, _Data.BluePrint);
            }
            else
            {
                this.imgBlueprint.gameObject.SetActive(false);
                this.assetList.gameObject.SetActive(true);
                this.assetList.SetAssets(_Data.Price);
            }

        }

        public override void Unclock()
        {
            base.Unclock();

            if (this.Data == null)
            {
                return;
            }

            Data_BuildingBase _Data = this.GetData<Data_BuildingBase>();

            if (_Data.BuildingEnum == BuildingEnum.None)
            {
                return;
            }

            if (_Data.StateEnum == BuildingStateEnum.Open)
            {
                return;
            }


            Hero hero = PlayManager.I.mHero;
            hero.Controller.SetStatePause();

            _Data.Open();

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            userData.AddLandExp(2);

            Sequence s = DOTween.Sequence();
            s.AppendCallback(() =>
            {
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_Build_01);
            });
            s.AppendInterval(0.15f);
            s.AppendCallback(() =>
            {
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_Build_01);
            });
            s.AppendInterval(0.15f);
            s.AppendCallback(() =>
            {
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_Build_01);
            });
            s.AppendInterval(0.15f);
            s.AppendCallback(() =>
            {
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_B_builded);
            });


            ResManager.I.InstantiateAsync(AddressbalePathEnum.PREFAB_SmokeCompleted, (AsyncOperationHandle<GameObject> obj) =>
             {
                 MapRender mapRender = MapManager.I.mapRender;
                 GameObject go = obj.Result;
                 go.transform.parent = mapRender.Builds.transform;
                 go.transform.position = this.transform.position;

                 Sequence s = DOTween.Sequence();
                 s.AppendInterval(0.2f);
                 s.AppendCallback(() =>
                 {
                     _Data.Load();
                 });
                 s.AppendInterval(0.4f);
                 s.AppendCallback(() =>
                 {
                     Destroy(go);
                     hero.Controller.SetStateIdle();
                 });
             });

            // 任务
            TaskManager.I.OnTaskBuild(_Data.BuildingEnum);

            // 保存
            DataManager.I.Save(DataDefine.UserData);
            DataManager.I.Save(DataDefine.MapData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asset"></param>
        public override void UpdatePrice(Data_GameAsset asset)
        {
            assetList.UpdateAssets(asset);
        }
    }

}

