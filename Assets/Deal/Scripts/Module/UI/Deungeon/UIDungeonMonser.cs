using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Druid;
using TMPro;
using Deal.Data;

namespace Deal.UI
{
    public class UIDungeonMonser : UIBase
    {
        public TextMeshProUGUI txtLvId;


        public Transform pnlInfo;
        public TextMeshProUGUI txtName;
        public TextMeshProUGUI txtHp;
        public TextMeshProUGUI txtAtt;
        public Slider sliderHp;

        public override void OnUIAwake()
        {
            this.HideMonsterInfo();

            DungeonData dungeon = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);

            Data_DungeonStage dataStage = dungeon.Data.DataStage;
            double dungeonLv = dataStage.DungeonBuilding;
            DataDungeonLevel dataDungeon = dataStage.DataDungeonLevel;
            if (dataDungeon.isTmp == true)
            {
                this.txtLvId.text = "";
            }
            else
            {
                this.txtLvId.text = $"第{dataDungeon.lvId + 1}关";
            }
        }

        public void SetMonster(Enemy enemy)
        {
            this.pnlInfo.gameObject.SetActive(true);
            ExcelData.Monster monster = ConfigManger.I.GetMonsterCfg(enemy.monsterType);
            if (monster != null)
            {
                this.txtName.text = monster.chinese;
            }

            this.txtHp.text = $"{enemy.CurAtt.HP }/{enemy.CurAtt.MaxHP}";
            this.sliderHp.value = enemy.CurAtt.HP / 1.0f / enemy.CurAtt.MaxHP;
            this.txtAtt.text = $"攻击力：{enemy.CurAtt.Attack }   速度：{this.getSpeedName(enemy.CurAtt.AttackSpeed)}";

            enemy.OnRoleHpUpdate += this.UpdateMonsterHp;
        }

        public void HideMonsterInfo()
        {
            this.pnlInfo.gameObject.SetActive(false);
        }

        public void OnExitClick()
        {
            Hero hero = PlayManager.I.mHero;
            hero.Controller.SetStatePause();
            PlayManager.I.LoadGameScene();
        }


        private void UpdateMonsterHp(int hp, int maxHp)
        {
            this.txtHp.text = $"{hp} / {maxHp}";
            this.sliderHp.value = hp / 1.0f / maxHp;
        }

        public string getSpeedName(float speed)
        {
            if (speed <= 0.6f)
            {
                return "极快";
            }
            else if (speed <= 0.8f)
            {
                return "快速";
            }
            else if (speed <= 1.0f)
            {
                return "中速";
            }
            else
            {
                return "慢速";
            }
        }
    }
}

