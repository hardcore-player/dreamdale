using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Deal.UI;
using Deal.Dungeon;
using Deal.Data;
using Druid.Utils;

namespace Deal
{
    /// <summary>
    /// 战斗场景管理器
    /// </summary>
    public class DungeonManager : Singleton<DungeonManager>
    {
        public SpriteRenderer spMask;

        public Dungeon_Level DungeonLevel;

        public bool IsDebug = false;

        private bool _isStart = false;

        public void StartLevel()
        {
            Hero hero = PlayManager.I.mHero;
            hero.transform.position = DungeonLevel.bornPoint.position;

            spMask.gameObject.SetActive(true);
            this._isStart = true;

            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            dungeonData.OnHeroLvChange += OnHeroLvChange;

            hero.OnHeroFightEnter += OnHeroFightEnter;
            hero.OnHeroFightStart += OnHeroFightStart;
            hero.OnHeroFightEnd += OnHeroFightEnd;
            hero.OnHeroFightExit += OnHeroFightExit;
            hero.OnHeroFightDie += OnHeroFightDie;
            hero.OnHeroOpenTreasure += OnHeroOpenTreasure;
        }


        private void OnDestroy()
        {
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            if (dungeonData != null)
            {
                dungeonData.OnHeroLvChange -= OnHeroLvChange;
            }

            Hero hero = PlayManager.I.mHero;
            if (hero != null)
            {
                hero.OnHeroFightEnter -= OnHeroFightEnter;
                hero.OnHeroFightStart -= OnHeroFightStart;
                hero.OnHeroFightEnd -= OnHeroFightEnd;
                hero.OnHeroFightExit -= OnHeroFightExit;
                hero.OnHeroFightDie -= OnHeroFightDie;
                hero.OnHeroOpenTreasure -= OnHeroOpenTreasure;
            }

        }


        /// <summary>
        /// 升级
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="exp"></param>
        public async void OnHeroLvChange(int lv, int exp, bool lvup)
        {
            if (lvup)
            {
                // 回满血
                Hero hero = PlayManager.I.mHero;

                int rHp = (int)(hero.CurAtt.MaxHP - hero.CurAtt.HP);
                hero.CurAtt.HP = hero.CurAtt.MaxHP;
                hero.UpdateHP();

                DealUtils.newRecoverHpNum(rHp, hero);

                // 奖励掉落
                UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);

                List<Data_GameAsset> rewards = DealUtils.GetHeroLvupReword();

                //for (int i = 0; i < rewards.Count; i++)
                //{
                //    userData.AddAssetForce(rewards[i].assetType, rewards[i].assetNum);
                //}

                await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIPlayerLvUp, UILayer.Dialog, new UIParamStruct(rewards));

                DataManager.I.Save(DataDefine.UserData);
                DataManager.I.Save(DataDefine.MapData);
                DataManager.I.Save(DataDefine.DungeonData);
            }
        }

        public void OnHeroFightEnter(BattleRoleBase enemy)
        {
            Enemy _enemy = enemy as Enemy;
            UIDungeonMonser uiMonster = UIManager.I.Get(AddressbalePathEnum.PREFAB_UIMonster) as UIDungeonMonser;
            uiMonster.SetMonster(_enemy);

        }

        public void OnHeroFightStart(BattleRoleBase enemy)
        {
            Enemy _enemy = enemy as Enemy;

            //UIDungeonMonser uiMonster = UIManager.I.Get(AddressbalePathEnum.PREFAB_UIMonster) as UIDungeonMonser;
            //uiMonster.SetMonster(enemy);

        }

        public void OnHeroFightEnd(BattleRoleBase enemy)
        {
            Enemy _enemy = enemy as Enemy;

        }

        public void OnHeroFightExit(BattleRoleBase enemy)
        {
            Enemy _enemy = enemy as Enemy;

            UIDungeonMonser uiMonster = UIManager.I.Get(AddressbalePathEnum.PREFAB_UIMonster) as UIDungeonMonser;
            uiMonster.HideMonsterInfo();

            if (enemy.IsDie())
            {

                DungeonData dungeon = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);

                Data_DungeonStage dataStage = dungeon.Data.DataStage;

                if (dataStage.DataDungeonLevel != null)
                {
                    dataStage.DataDungeonLevel.EnemeyDie(_enemy.MonsterId);

                    DataManager.I.Save(DataDefine.DungeonData);
                    DataManager.I.Save(DataDefine.MapData);

                }

                DealUtils.DungeonMonsterDrop(enemy.transform.position);

                TaskManager.I.OnTaskKill(1);
                ActivityUtils.DoDailyTask(DailyTaskTypeEnum.kill, 1);
            }

            Hero hero = PlayManager.I.mHero;
            hero.Controller.SetStateIdle();

        }

        public async void OnHeroFightDie(BattleRoleBase role)
        {
            // 复活提示
            UIReplay uIReplay = await UIManager.I.PushAsync(AddressbalePathEnum.PREFAB_UIReplay, UILayer.Dialog) as UIReplay;
            uIReplay.SetCallback(() =>
            {
                Hero hero = PlayManager.I.mHero;
                hero.CurAtt.HP = hero.CurAtt.MaxHP;
                hero.UpdateHP();

                DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
                dungeonData.Data.DataStage.ResurrectionTimes++;
                dungeonData.Save();
            }, () =>
            {
                // 死亡

                DungeonData dungeon = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);

                Data_DungeonStage dataStage = dungeon.Data.DataStage;
                DataDungeonLevel dungeonLevel = dataStage.DataDungeonLevel;

                if (dungeonLevel != null)
                {

                    dungeonLevel.enemies.Clear();
                    dungeonLevel.treasures.Clear();

                    dungeonLevel.InCd = true;
                    dungeonLevel.CommonRefreshNeed = 10 * 60;
                    dungeonLevel.CommonCDAt = TimeUtils.TimeNowMilliseconds();

                    DataManager.I.Save(DataDefine.DungeonData);
                    DataManager.I.Save(DataDefine.MapData);

                }


                PlayManager.I.LoadGameScene();
            });
        }

        public void OnHeroOpenTreasure(Dungeon_Treasure treasure)
        {
            DungeonData dungeon = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);

            Data_DungeonStage dataStage = dungeon.Data.DataStage;

            if (dataStage.DataDungeonLevel != null)
            {
                dataStage.DataDungeonLevel.TreasureOpend(treasure.TreasureId);
                DataManager.I.Save(DataDefine.DungeonData);
                DataManager.I.Save(DataDefine.MapData);
            }
        }



        private void Update()
        {
            if (this._isStart == false) return;
            Hero hero = PlayManager.I.mHero;
            this.spMask.transform.position = hero.transform.position;
        }
    }
}