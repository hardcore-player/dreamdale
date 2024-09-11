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
    public class Building_SpaceLand : BuildingUnclockWithAsset
    {
        public Image imgBgBorder;
        public AssetList assetList;

        public Transform ui;


        public override void OnUIDisplayShow()
        {
            Data_SpaceCost data_ = this.GetData<Data_SpaceCost>();
            int xx = data_.Size.x / 3;
            int yy = data_.Size.y / 3;

            if (xx == 1 && yy == 1) return;

            bool heroLeft = true;
            bool heroTop = true;

            Hero hero = PlayManager.I.mHero;

            heroLeft = hero.transform.position.x < this.transform.position.x;
            heroTop = hero.transform.position.y > this.transform.position.y;

            float uix = 0;
            float uiy = 0;
            if (heroLeft)
            {
                uix = -(xx - 1) * 1.5f;
            }
            else
            {
                uix = (xx - 1) * 1.5f;
            }

            if (heroTop)
            {
                uiy = (yy - 1) * 1.5f;
            }
            else
            {
                uiy = -(yy - 1) * 1.5f;
            }

            this.ui.localPosition = new Vector3(uix, uiy, 0);
            this.center.localPosition = new Vector3(uix, uiy, 0);
        }

        public override void UpdateView()
        {
            Data_SpaceCost data_ = this.GetData<Data_SpaceCost>();
            this.assetList.SetAssets(data_.Price);

            int xx = data_.Size.x / 3;
            int yy = data_.Size.y / 3;

            RectTransform rect = imgBgBorder.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(xx * 100, yy * 100);

            BoxCollider2D collider2D = this.GetComponent<BoxCollider2D>();
            collider2D.size = new Vector2(3 * xx + 1, 3 * yy + 1);
        }

        /// <summary>
        /// 更新价格
        /// </summary>
        /// <param name="asset"></param>
        public override void UpdatePrice(Data_GameAsset asset)
        {
            this.assetList.UpdateAssets(asset);
        }


        public override void Unclock()
        {
            base.Unclock();

            if (this.Data == null)
            {
                return;
            }

            Data_BuildingBase _Data = this.GetData<Data_BuildingBase>();

            if (_Data.StateEnum == BuildingStateEnum.Open)
            {
                return;
            }


            _Data.Open();

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            userData.AddLandExp(2);

            Sequence s = DOTween.Sequence();
            s.AppendCallback(() =>
            {
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_Build_01);
            });
            s.AppendInterval(0.1f);
            s.AppendCallback(() =>
            {
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_Build_01);
            });
            s.AppendInterval(0.1f);
            s.AppendCallback(() =>
            {
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_Build_01);
            });
            s.AppendInterval(0.1f);
            s.AppendCallback(() =>
            {
                SoundManager.I.playEffect(AddressbalePathEnum.WAV_B_builded);
            });

            Data_Point grid = _Data.StartGrid();

            ResManager.I.InstantiateAsync(AddressbalePathEnum.PREFAB_SmokeCompleted, (AsyncOperationHandle<GameObject> obj) =>
            {
                MapRender mapRender = MapManager.I.mapRender;
                GameObject go = obj.Result;
                go.transform.parent = mapRender.Builds.transform;
                go.transform.position = this.transform.position;

                MapManager.I.mapRender.DeleteSpaceLand(this.Data as Data_SpaceCost);

                Sequence s = DOTween.Sequence();
                s.AppendInterval(0.2f);
                s.AppendCallback(() =>
                {
                });
                s.AppendInterval(0.4f);
                s.AppendCallback(() =>
                {
                    Destroy(go);
                });
            });

            // 开放新土地
            MapManager.I.OpenTiles(grid.x, grid.y, _Data.Size.x, _Data.Size.y);

            // 任务
            TaskManager.I.OnTaskLand(this.Data as Data_SpaceCost);
            ActivityUtils.DoDailyTask(DailyTaskTypeEnum.land, 1);

            // 保存
            DataManager.I.Save(DataDefine.UserData);
            DataManager.I.Save(DataDefine.MapData);

        }
    }

}

