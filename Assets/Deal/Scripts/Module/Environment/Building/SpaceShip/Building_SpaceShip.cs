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
    /// 太空飞船
    /// </summary>
    public class Building_SpaceShip : BuildingUnclockWithAsset
    {
        public AssetList assetList;
        public BoxCollider2D boxCollider2D;

        public GameObject ship;


        public override void Unclock()
        {
            base.Unclock();

            Data_SpaceShip data_ = this.GetData<Data_SpaceShip>();

            data_.Price.Clear();
            this.assetList.Clear();

            MapRender mapRender = MapManager.I.mapRender;
            PrefabsUtils.NewUnlockSmoke(mapRender.Builds.transform, this.transform.position);

            this.boxCollider2D.enabled = false;



            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(1);
            sequence.AppendCallback(() =>
            {
                data_.NewTool();
                this.UpdateView();
                this.boxCollider2D.enabled = true;
            });
        }

        public override void UpdateView()
        {
            Data_SpaceShip data_ = this.GetData<Data_SpaceShip>();
            Debug.Log("SetAssets====== UpdateView" + data_.Price.Count);

            this.assetList.SetAssets(data_.Price);

            //float ss = 0.5f + (data_.ProcessId * 0.04f);
            //this.ship.transform.localScale = new Vector3(ss, ss, ss);

            //for (int i = 0; i < data_.Price.Count; i++)
            //{
            //    this.assetList.UpdateAssets(data_.Price[i]);
            //}


        }
    }
}

