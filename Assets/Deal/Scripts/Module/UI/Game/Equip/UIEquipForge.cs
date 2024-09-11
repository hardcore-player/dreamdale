using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using Deal.Data;
using TMPro;

namespace Deal.UI
{
    /// <summary>
    /// 装备强化界面
    /// </summary>
    public class UIEquipForge : UIBase
    {
        public UIEquipItem uiEquip;
        public TextMeshProUGUI txtEquipName;
        public TextMeshProUGUI txtEquipQuility; // 品质

        //属性
        public CmpEquipAttrItem pfbAttrItem;

        private Dictionary<EquipAttrEnum, CmpEquipAttrItem> _attrItems = new Dictionary<EquipAttrEnum, CmpEquipAttrItem>();

        public Button btnEquip;
        public Button btnLvup;
        public Button btnLvupMax;

        public TextMeshProUGUI txtBtnEquip;
        public TextMeshProUGUI txtBtnLvup;

        public CmpAssetPriceTotal price0;
        public CmpAssetPriceTotal price1;

        public Data_Equip _data;

        #region Override

        public override void OnUIAwake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/BtnClose", this.OnCloseClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Btns/BtnEquip", this.OnEquipClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Btns/BtnLvUp", this.OnLvupClick);
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "Content/Btns/BtnLvUpMax", this.OnRecoverClick);
        }

        public override void OnUIStart()
        {
        }

        private void OnDestroy()
        {
        }

        public override void OnInit(UIParamStruct param)
        {
            this._data = param.param as Data_Equip;

            this.SetData(this._data);
        }

        #endregion Override

        #region public

        #endregion public


        #region private

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_equip"></param>
        private void SetData(Data_Equip _equip)
        {
            this.uiEquip.SetEquip(_equip);

            ExcelData.Equip equipCfg = ConfigManger.I.GetEquipCfg(_equip.equipId);
            if (equipCfg == null) return;

            this.txtEquipName.text = equipCfg.name;
            this.txtEquipQuility.text = DealUtils.getQualityName(_equip.equipQuality);
            this.txtEquipQuility.color = DealUtils.getQualityColor(_equip.equipQuality);

            DungeonData dungeon = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            // 正在装备的
            Data_Equip onEquip = dungeon.GetEquip(_equip.point);

            if (onEquip != null && onEquip == _equip)
            {
                //正在装备
                this.txtBtnEquip.text = "卸下";
            }
            else
            {
                this.txtBtnEquip.text = "装备";
            }

            BattleRoleAtt cAtt = MathUtils.GetEquipAtt(_equip);
            BattleRoleAtt nAtt = MathUtils.GetEquipAtt(new Data_Equip(_equip.equipId, _equip.equipLv + 1, _equip.equipQuality));

            if (cAtt.HP > 0)
            {
                this._rendAttr(EquipAttrEnum.hp, cAtt.HP, nAtt.HP);
            }
            if (cAtt.Attack > 0)
            {
                this._rendAttr(EquipAttrEnum.attack, cAtt.Attack, nAtt.Attack);
            }
            if (cAtt.Crit > 0)
            {
                this._rendAttr(EquipAttrEnum.crit, cAtt.Crit, nAtt.Crit);
            }
            if (cAtt.DeCrit > 0)
            {
                this._rendAttr(EquipAttrEnum.decrit, cAtt.DeCrit, nAtt.DeCrit);
            }
            if (cAtt.Hit > 0)
            {
                this._rendAttr(EquipAttrEnum.hit, cAtt.Hit, nAtt.Hit);
            }
            if (cAtt.Dodge > 0)
            {
                this._rendAttr(EquipAttrEnum.dodge, cAtt.Dodge, nAtt.Dodge);
            }
            if (cAtt.HPReg > 0)
            {
                this._rendAttr(EquipAttrEnum.hreg, cAtt.HPReg, nAtt.HPReg);
            }

            ///价格
            AssetEnum price0 = DealUtils.toAssetEnum(equipCfg.rune);
            int priceRune = MathUtils.GetRunePrice(this._data.equipLv);
            this.price0.SetAsset(price0, priceRune);

            int priceGold = this._data.equipLv * 200;
            this.price1.SetAsset(AssetEnum.Gold, priceGold);
        }

        private void _rendAttr(EquipAttrEnum equipAttr, float c, float n)
        {
            if (!this._attrItems.ContainsKey(equipAttr))
            {
                CmpEquipAttrItem item = Instantiate(this.pfbAttrItem, this.pfbAttrItem.transform.parent);
                item.gameObject.SetActive(true);
                this._attrItems.Add(equipAttr, item);
            }
            this._attrItems[equipAttr].SetAttr(equipAttr, c, n);
        }

        #endregion private

        #region click

        public void OnEquipClick()
        {
            DungeonData dungeon = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            // 正在装备的
            Data_Equip onEquip = dungeon.GetEquip(this._data.point);

            if (onEquip != null && onEquip == this._data)
            {
                //正在装备
                DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
                dungeonData.Equip(this._data.point, null);

                this.CloseSelf();
            }
            else
            {
                DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
                dungeonData.Equip(this._data.point, this._data);

                this.CloseSelf();
            }
        }

        /// <summary>
        /// 升级公式如下:
        ///消耗符文=等级数
        ///消耗金币 = 等级数x200
        /// </summary>
        public void OnLvupClick()
        {
            ExcelData.Equip equipCfg = ConfigManger.I.GetEquipCfg(this._data.equipId);
            if (equipCfg == null) return;

            AssetEnum price0 = DealUtils.toAssetEnum(equipCfg.rune);

            int priceRune = MathUtils.GetRunePrice(this._data.equipLv);
            int priceGold = this._data.equipLv * 200;

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);

            if (userData.GetAssetNum(AssetEnum.Gold) >= priceGold && userData.GetAssetNum(price0) >= priceRune)
            {
                userData.CostAsset(AssetEnum.Gold, priceGold);
                userData.CostAsset(price0, priceRune);

                dungeonData.EquipLvup(this._data);
                this.SetData(this._data);

                DataManager.I.Save(DataDefine.UserData);
                //DataManager.I.Save(DataDefine.DungeonData);
            }
        }

        public void OnRecoverClick()
        {
            ExcelData.Equip equipCfg = ConfigManger.I.GetEquipCfg(this._data.equipId);
            if (equipCfg == null) return;

            if (this._data.equipLv <= 1) return;

            int totalNum1 = 0;
            int totalNum2 = 0;
            for (int i = 1; i < this._data.equipLv; i++)
            {
                int priceRune = MathUtils.GetRunePrice(i);
                int priceGold = i * 200;

                totalNum1 += priceRune;
                totalNum2 += priceGold;
            }

            AssetEnum price0 = DealUtils.toAssetEnum(equipCfg.rune);

            List<Data_GameAsset> rewards = new List<Data_GameAsset>();
            rewards.Add(new Data_GameAsset(price0, totalNum1));
            rewards.Add(new Data_GameAsset(AssetEnum.Gold, totalNum2));

            // 提示还原后的物品
            ShopUtils.showRewardAndToUser(rewards);

            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            dungeonData.EquipLvRecover(this._data);
            this.SetData(this._data);
        }

        #endregion click
    }
}


