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
    /// 实验室
    /// </summary>
    public class Building_ResearchLab : BuildingBase
    {
        public void OnUIClick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIResearchLabPop, UILayer.Dialog);
        }
    }
}

