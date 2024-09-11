using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Druid;
using System;
using Deal.Data;

namespace Deal
{
    public class UIEquipItem : MonoBehaviour
    {
        public GameObject content;
        public Image imgBg;
        public Image imgIcon;
        public TextMeshProUGUI txtLv;
        public GameObject goArrow;


        public GameObject goEmpty;
        public GameObject goItem;

        private Data_Equip _equip;

        private void Awake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "item", this.OnEquipClick);
        }


        public void SetEquip(Data_Equip equip)
        {
            this._equip = equip;

            if (equip == null)
            {
                this.SetEmpty();

                return;
            }

            ExcelData.Equip equipCfg = ConfigManger.I.GetEquipCfg(equip.equipId);

            this.txtLv.text = $"Lv.{equip.equipLv}";

            this.goEmpty.SetActive(false);
            this.goItem.SetActive(true);

            SpriteUtils.SetEquipIcon(imgIcon, equip.equipId, () =>
            {
                this.imgIcon.SetNativeSize();

                if (equipCfg.type == EquipPointEnum.weapon.ToString())
                {
                    this.imgIcon.transform.localEulerAngles = new Vector3(0, 0, -45);
                }
                else
                {
                    this.imgIcon.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            });

        }

        public void SetEmpty()
        {
            this.goEmpty.SetActive(true);
            this.goItem.SetActive(false);

            SpriteUtils.SetEquipIcon(imgIcon, 0);
        }


        public void OnEquipClick()
        {
            UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIEquipForge, UILayer.Dialog, new UIParamStruct(this._equip));

            //DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);

            //ExcelData.Equip equipCfg = ConfigManger.I.GetEquipCfg(this._equip.equipId);

            //EquipPointEnum point = (EquipPointEnum)Enum.Parse(typeof(EquipPointEnum), equipCfg.type);

            //dungeonData.Equip(point, this._equip);
        }
    }

}
