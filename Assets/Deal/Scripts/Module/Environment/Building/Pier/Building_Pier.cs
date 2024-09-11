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
    /// 码头
    /// </summary>
    public class Building_Pier : BuildingBase
    {
        public override void OnHeroEnter(Hero mHero)
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            if (userData.HasWorkshopTool(WorkshopToolEnum.FishingRod))
            {
                base.OnHeroEnter(mHero);
            }
            else
            {
                mHero.ShowLackToolBubble(WorkshopToolEnum.FishingRod);
            }

        }

        public override void OnHeroExit(Hero mHero)
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            if (userData.HasWorkshopTool(WorkshopToolEnum.FishingRod))
            {
                base.OnHeroExit(mHero);
            }
            else
            {
                mHero.HideToolBubble(WorkshopToolEnum.FishingRod);
            }
        }
    }
}

