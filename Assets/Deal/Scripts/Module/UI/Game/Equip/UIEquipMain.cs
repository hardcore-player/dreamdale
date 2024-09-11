using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using Deal.Data;
using TMPro;

namespace Deal.UI
{
    public class UIEquipMain : UIBase
    {
        public Transform pfbEquipItem;

        public TextMeshProUGUI txtHp;
        public TextMeshProUGUI txtAtk;

        // info
        public TextMeshProUGUI txtUserName;
        public TextMeshProUGUI txtLv;
        public Slider sldLv;


        public Dictionary<EquipPointEnum, UIEquipItem> listEquiped = new Dictionary<EquipPointEnum, UIEquipItem>();
        public List<UIEquipItem> listBag = new List<UIEquipItem>();

        public override void OnUIAwake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Top/BtnClose", this.OnCloseClick);
            //GameObject bag = Druid.Utils.UIUtils.Find(this.transform, "Content/Bag");


            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            dungeonData.OnEquipChange += OnEquipChange;
            dungeonData.OnEquipLvup += OnEquipLvup;

            this._initEquipItems();

        }

        private void OnDestroy()
        {
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            if (dungeonData != null)
            {
                dungeonData.OnEquipChange -= OnEquipChange;
                dungeonData.OnEquipLvup -= OnEquipLvup;
            }
        }

        public override void OnUIStart()
        {
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

            Data_Userinfo _Userinfo = userData.Data.Userinfo;
            this.txtUserName.text = _Userinfo.NickName;

            // 等级信息
            //this.txtUserName.text = userData.Data.UserName;
            this.txtLv.text = dungeonData.Data.HeroLv + "";
            this.sldLv.value = dungeonData.Data.HeroExp / 1.0f / dungeonData.Data.HeroExpMax;

            this._renderEquiped();
            this._renderBagList();

            this._renderAttrs();
        }

        /// <summary>
        /// 身上的装备
        /// </summary>
        private void _initEquipItems()
        {
            // 身上的装备位置
            for (int i = 0; i < 6; i++)
            {
                UIEquipItem item = Druid.Utils.UIUtils.FindCmp<UIEquipItem>(this.transform, "Content/Top/PlayerEquip/Viewport/Content/UIEquip" + i);

                item.SetEmpty();
                if (i == 0)
                {
                    this.listEquiped.Add(EquipPointEnum.weapon, item);
                }
                else if (i == 1)
                {
                    this.listEquiped.Add(EquipPointEnum.head, item);
                }
                else if (i == 2)
                {
                    this.listEquiped.Add(EquipPointEnum.chest, item);
                }
                else if (i == 3)
                {
                    this.listEquiped.Add(EquipPointEnum.shield, item);
                }
                else if (i == 4)
                {
                    this.listEquiped.Add(EquipPointEnum.cloak, item);
                }
                else if (i == 5)
                {
                    this.listEquiped.Add(EquipPointEnum.rune, item);
                }

            }
        }


        private void _renderEquiped()
        {
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            // 身上的装备
            Dictionary<EquipPointEnum, Data_Equip> EquipPoints = dungeonData.Data.EquipPoints;
            // 身上的
            foreach (var item in EquipPoints)
            {
                this.listEquiped[item.Key].SetEquip(item.Value);
            }
        }

        /// <summary>
        /// 渲染背包列表
        /// </summary>
        private void _renderBagList()
        {
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            // 所有装备
            List<Data_Equip> EquipList = dungeonData.Data.EquipList;

            // 背包只显示没装备的装备
            List<Data_Equip> showInBagList = new List<Data_Equip>();

            for (int i = 0; i < EquipList.Count; i++)
            {
                if (!EquipList[i].IsEquipOn())
                {
                    showInBagList.Add(EquipList[i]);
                }
            }

            int equipCounts = showInBagList.Count;
            // 背包数量，默认最少五行
            int bagItemCount = 0;

            if (equipCounts < 20)
            {
                bagItemCount = 20;
            }
            else
            {
                bagItemCount = (equipCounts / 5 + 1) * 5;
            }


            for (int i = 0; i < bagItemCount; i++)
            {
                UIEquipItem item;
                if (this.listBag.Count > i)
                {
                    item = this.listBag[i];
                }
                else
                {
                    Transform goods = Instantiate(this.pfbEquipItem, this.pfbEquipItem.parent);
                    goods.gameObject.SetActive(true);
                    item = goods.GetComponent<UIEquipItem>();
                    this.listBag.Add(item);
                }

                if (i < equipCounts)
                {
                    item.SetEquip(showInBagList[i]);
                }
                else
                {
                    item.SetEmpty();
                }
            }


        }

        /// <summary>
        /// 属性
        /// </summary>
        private void _renderAttrs()
        {
            Hero hero = PlayManager.I.mHero;

            this.txtHp.text = hero.OriAtt.MaxHP + "";
            this.txtAtk.text = hero.OriAtt.Attack + "";
        }



        /// <summary>
        /// 更换装备
        /// </summary>
        /// <param name="point"></param>
        /// <param name="id"></param>
        public void OnEquipChange(EquipPointEnum point, Data_Equip data)
        {
            this.listEquiped[point].SetEquip(data);

            this._renderBagList();
            this._renderAttrs();
        }

        public void OnEquipLvup(EquipPointEnum point, Data_Equip data)
        {
            this._renderAttrs();
        }

    }
}


