using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.UI;
using Deal.UI;
using Deal.Data;
using Deal.Tools;
using ExcelData;

namespace Deal.Env
{
    /// <summary>
    /// 图书馆
    /// </summary>
    public class Building_Library : BuildingBase
    {
        public async void OnUIClick()
        {
            UILibrary uILibrary = await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UILibrary, UILayer.Dialog) as UILibrary;
            uILibrary.SetData(this.GetData<Data_Library>());
        }
    }
}

