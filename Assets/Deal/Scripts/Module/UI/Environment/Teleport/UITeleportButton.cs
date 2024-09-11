using System;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Env;
using Deal.Data;
using DG.Tweening;

namespace Deal.UI
{
    public class UITeleportButton : UIBase
    {
        public GameObject Point;
        public GameObject MyPoint;

        public long UniqueId = 0;

        private Data_BuildingBase _data;

        private bool isMyPoint = false;

        public override void OnUIAwake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Btn", this.OnClick);

        }

        public void SetData(Data_BuildingBase data)
        {
            this._data = data;

            this.Point.SetActive(true);
            this.MyPoint.SetActive(false);
        }

        public void SetMyPos()
        {
            this.Point.SetActive(false);
            this.MyPoint.SetActive(true);

            this.isMyPoint = true;
        }

        public void OnClick()
        {
            if (this._data != null && this.isMyPoint == false)
            {
                this.GotoTeleport(this._data.WorldPos + new Vector3(0f, 0f, 0));

                UIManager.I.Pop(AddressbalePathEnum.PREFAB_UITeleportMap);
            }
        }

        private void GotoTeleport(Vector3 pos)
        {
            Hero hero = PlayManager.I.mHero;
            hero.Controller.SetStatePause();
            hero.PlayBorn();

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(0.3f);
            sequence.AppendCallback(() =>
            {
                hero.transform.position = pos;
                hero.PlayBorn();
            });
            sequence.AppendInterval(0.3f);
            sequence.AppendCallback(() =>
            {
                hero.Controller.SetStateIdle();
            });
        }
    }



}


