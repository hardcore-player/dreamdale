using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Druid;
using Deal.Data;

namespace Deal.Env
{
    /// <summary>
    /// 传送点
    /// </summary>
    public class Building_Teleport : BuildingBase
    {
        public override void OnHeroEnter(Hero mHero)
        {
            //base.OnHeroEnter(mHero);

            Debug.Log("Building_Teleport OnHeroEnter");

            this.FindTeleport();
        }


        public void FindTeleport()
        {

            Data_BuildingBase data = this.GetData<Data_BuildingBase>();

            MapData mapData = DataManager.I.Get<MapData>(DataDefine.MapData);
            List<Data_BuildingBase> buildings = mapData.Data.buildings;

            List<Data_BuildingBase> otherTeleport = new List<Data_BuildingBase>();

            for (int i = 0; i < buildings.Count; i++)
            {
                if (buildings[i].BuildingEnum == BuildingEnum.Teleport)
                {
                    if (buildings[i].Pos != data.Pos)
                    {
                        otherTeleport.Add(buildings[i]);
                    }
                }
            }

            if (otherTeleport.Count == 1)
            {
                this.GotoTeleport(otherTeleport[0].WorldPos + new Vector3(0f, 0f, 0));
            }
            else
            {
                otherTeleport.Add(data);
                UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UITeleportMap, UILayer.Dialog, new UIParamStruct(otherTeleport));
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


