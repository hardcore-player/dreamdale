using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using UnityEngine.UI;
using Deal.UI;
using Deal.Data;
using Deal.Tools;
using ExcelData;
using DG.Tweening;

namespace Deal.Env
{
    /// <summary>
    /// 雕塑
    /// </summary>
    public class Building_Statue : BuildingBase
    {
        public SpriteRenderer srBody;

        public override void UpdateView()
        {
            Data_BuildingBase data = this.GetData<Data_BuildingBase>();
            SpriteUtils.SetStatueSprite(this.srBody, data.StatueEnum);
        }

        public async void OnUIClick()
        {
            Data_Statue data_Statue = this.Data as Data_Statue;

            UIStatue uIStatue = await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIStatue, UILayer.Dialog) as UIStatue;
            uIStatue.SetData(data_Statue.StatueEnum);
        }
    }
}

