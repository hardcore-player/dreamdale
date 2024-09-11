using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using UnityEngine.AddressableAssets;
using Deal.Data;

namespace Deal.Env
{
    /// <summary>
    /// 房屋
    /// </summary>
    public class Building_House : BuildingBase
    {
        public Image imgWorkIcon;
        public Image imgNoWorkIcon;

        public void OnUIClick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIHousePop, UILayer.Dialog, new UIParamStruct(this.Data));
        }


        public override void UpdateView()
        {
            Data_House _House = this.GetData<Data_House>();
            if (_House.WorkerType == AssetEnum.None)
            {
                this.imgNoWorkIcon.gameObject.SetActive(true);
                this.imgWorkIcon.gameObject.SetActive(false);
            }
            else
            {
                this.imgNoWorkIcon.gameObject.SetActive(false);
                this.imgWorkIcon.gameObject.SetActive(true);

                SpriteUtils.SetAssetSprite(this.imgWorkIcon, _House.WorkerType);
            }
        }
    }
}

