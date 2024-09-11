using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Deal.Data;

namespace Deal.UI
{
    public class UIGuideShop : UIBase
    {

        public void OnDungeonClick()
        {
            //ShopUtils.pushShop();
            MapRender mapRender = MapManager.I.mapRender;

            // 目标雕塑
            Data_BuildingBase target = null;
            for (int i = 0; i < mapRender.SO_MapData.Builds.Count; i++)
            {
                Data_BuildingBase builds = mapRender.SO_MapData.Builds[i];

                if (builds.BuildingEnum == BuildingEnum.Portal)
                {
                    target = builds;
                    break;
                }
            }

            DealUtils.Guide2BuildingData(target);

            this.CloseSelf();
        }

        public void OnShopClick()
        {
            ShopUtils.pushShop();
            this.CloseSelf();
        }
    }
}

