using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using TMPro;

namespace Deal.UI
{
    public class UIPlayerLvUp : UIBase
    {
        public TextMeshProUGUI txtlv;

        public PlayerAttributeUpItem attributeUpItemHp;
        public PlayerAttributeUpItem attributeUpItemAtk;

        public CmpRewardItem pfbRewarditem;

        #region override
        public override void OnUIAwake()
        {
            Druid.Utils.UIUtils.AddBtnClick(this.transform, "", this.OnClose1Click);
        }


        public override void OnInit(UIParamStruct param)
        {
            List<Data_GameAsset> rewards = param.param as List<Data_GameAsset>;

            DungeonData dungeon = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            // 属性变化
            this._renderAttrChange(dungeon.Data.HeroLv - 1, dungeon.Data.HeroLv);

            //奖励展示
            for (int i = 0; i < rewards.Count; i++)
            {
                CmpRewardItem item = Instantiate(this.pfbRewarditem, this.pfbRewarditem.transform.parent);
                item.gameObject.SetActive(true);

                item.SetAsset(rewards[i].assetType, rewards[i].assetNum);
            }

            //等级
            this.txtlv.text = dungeon.Data.HeroLv + "";

            Hero hero = PlayManager.I.mHero;
            hero.Controller.SetStatePause();

        }

        #endregion override


        #region private

        /// <summary>
        /// 升级属性变化
        /// </summary>
        /// <param name="fromLv"></param>
        /// <param name="toLv"></param>
        private void _renderAttrChange(int fromLv, int toLv)
        {

            BattleRoleAtt formAtt = MathUtils.GetHeroAttByLv(fromLv);
            BattleRoleAtt toAtt = MathUtils.GetHeroAttByLv(toLv);

            attributeUpItemHp.SetData(EquipAttrEnum.hp, formAtt.HP, toAtt.HP);
            attributeUpItemAtk.SetData(EquipAttrEnum.attack, formAtt.Attack, toAtt.Attack);
            //if (formAtt.)
            //{
            //}
        }

        #endregion override

        #region click

        /// <summary>
        /// 点击继续
        /// </summary>
        public void OnClose1Click()
        {
            //Time.timeScale = 1;

            List<Data_GameAsset> rewards = this.Param.param as List<Data_GameAsset>;

            Hero hero = PlayManager.I.mHero;

            for (int i = 0; i < rewards.Count; i++)
            {
                Data_GameAsset _GameAsset = rewards[i];
                DealUtils.newDropItem(_GameAsset.assetType, _GameAsset.assetNum, hero.transform.position, false);
            }

            hero.Controller.SetStateIdle();

            this.CloseSelf();
        }

        #endregion click
    }

}
