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
    /// 博物馆
    /// </summary>
    public class Building_Museum : BuildingBase
    {
        public async void OnUIClick()
        {
            await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIMuseum, UILayer.Dialog);
        }
    }
}

