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
    /// 珠宝店
    /// </summary>
    public class Building_JewelryShop : BuildingBase
    {
        public void OnUIClick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIJewelryShopPop, UILayer.Dialog);
        }
    }
}

