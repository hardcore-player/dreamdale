using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Druid;
using DG.Tweening;
using Deal.UI;
using Deal.Dungeon;
using Deal.Data;
using System;

namespace Deal
{

    public delegate void DelegateHeroFightEnter(BattleRoleBase role);
    public delegate void DelegateHeroFightStart(BattleRoleBase role);
    public delegate void DelegateHeroFightEnd(BattleRoleBase role);
    public delegate void DelegateHeroFightExit(BattleRoleBase role);
    public delegate void DelegateHeroDie(BattleRoleBase role);
    public delegate void DelegateHeroOpenTreasure(Dungeon_Treasure treasure);


    /// <summary>
    /// 主角英雄
    /// </summary>
    public class Hero : BattleRole
    {
        private CharacterController _controller;
        private CharacterFarm _farmCharacter;
        private CharacterBattle _battleCharacter;
        private CharacterChatBubble _chatCharacter;
        private CharacterRider _riderCharacter;

        public CharacterController Controller { get => _controller; set => _controller = value; }
        public CharacterFarm FarmCharacter { get => _farmCharacter; set => _farmCharacter = value; }
        public CharacterBattle BattleCharacter { get => _battleCharacter; set => _battleCharacter = value; }
        public CharacterChatBubble ChatCharacter { get => _chatCharacter; set => _chatCharacter = value; }
        public CharacterRider RiderCharacter { get => _riderCharacter; set => _riderCharacter = value; }

        public DelegateHeroFightEnter OnHeroFightEnter;
        public DelegateHeroFightStart OnHeroFightStart;
        public DelegateHeroFightEnd OnHeroFightEnd;
        public DelegateHeroFightExit OnHeroFightExit;
        public DelegateHeroDie OnHeroFightDie;
        public DelegateHeroOpenTreasure OnHeroOpenTreasure;

        private bool _isRider = false;


        public override void OnAwake()
        {
            this.Controller = this.GetComponent<CharacterController>();
            this.FarmCharacter = this.GetComponentInChildren<CharacterFarm>();
            this.BattleCharacter = this.GetComponentInChildren<CharacterBattle>();
            this.ChatCharacter = this.GetComponentInChildren<CharacterChatBubble>();
            this.RiderCharacter = this.GetComponentInChildren<CharacterRider>();

            this.Controller.Hero = this;
            this.FarmCharacter.Hero = this;
            this.BattleCharacter.Hero = this;
            this.ChatCharacter.Hero = this;
            this.RiderCharacter.Hero = this;

            this.RestBattleBaseAtt();

            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            bool isRider = userData.Data.IsVip || userData.Data.IsInMapRider;
            this.SetRider(isRider);

            this.SetGoldAxe(userData.Data.IsGoldAxe);
            this.SetGoldPickaxe(userData.Data.IsGoldPickaxe);

        }

        public void SetRider(bool isRider)
        {
            this._isRider = isRider;
            this.Controller.speed = isRider ? 5 : 3;
            this.RiderCharacter.SetRider(isRider);
        }

        public void SetGoldAxe(bool isGold)
        {
            this.GetWeapon(WorkshopToolEnum.Axe).SetGold(isGold);
        }

        public void SetGoldPickaxe(bool isGold)
        {
            this.GetWeapon(WorkshopToolEnum.Pickaxe).SetGold(isGold);
        }

        public override void PlayWeapon(WorkshopToolEnum workshopTool)
        {
            if (this.FarmCharacter.HasSheep())
            {
                this.FarmCharacter.SheepHome();
            }

            if (this.FarmCharacter.HasChiken())
            {
                this.FarmCharacter.ChikensHome();
            }

            base.PlayWeapon(workshopTool);
            this.Controller.PlayAnimation(workshopTool);
        }

        /// <summary>
        /// 钓鱼
        /// </summary>
        public void PlayStartFishingRod()
        {
            this.Controller.SetStatePause();
            base.PlayWeapon(WorkshopToolEnum.FishingRod);

            WeaponFishingRod weaponFishing = this.GetWeapon(WorkshopToolEnum.FishingRod) as WeaponFishingRod;
            weaponFishing.playStartAttack();

            SoundManager.I.playEffect(AddressbalePathEnum.WAV_fish_start);
            //this.Controller.PlayAnimation(workshopTool);
        }

        public void OpenRoleBattle()
        {
            this.roleDisplay.gameObject.SetActive(true);
            this.weaponParent.gameObject.SetActive(true);
            this.headParent.gameObject.SetActive(true);
            this.BattleCharacter.enabled = true;

            this.FarmCharacter.enabled = false;
        }

        public void CloseRoleBattle()
        {
            this.roleDisplay.gameObject.SetActive(false);
            this.weaponParent.gameObject.SetActive(false);
            this.headParent.gameObject.SetActive(false);
            this.BattleCharacter.enabled = false;

            this.FarmCharacter.enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetEnum"></param>
        /// <returns></returns>
        public override bool CanCollectAsset(AssetEnum assetEnum)
        {
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            return userData.CanCollectRes(assetEnum);
        }


        public void UpdateHP()
        {
            this.roleDisplay.UpdateHp((int)this.CurAtt.HP, (int)this.CurAtt.MaxHP);
        }

        /// <summary>
        /// 缺少工具的气泡
        /// </summary>
        public void ShowLackToolBubble(WorkshopToolEnum toolEnum)
        {
            this.ChatCharacter.ShowLackTool(toolEnum);
        }

        /// <summary>
        /// 隐藏工具的气泡
        /// </summary>
        public void HideToolBubble(WorkshopToolEnum toolEnum)
        {
            this.ChatCharacter.HideLackTool(toolEnum);
        }


        public override void OnDie()
        {
            base.OnDie();
            this.OnHeroFightDie(this);
        }

        public override void PlayBorn()
        {
            this.birth.SetActive(true);
            Animator animator = this.birth.GetComponent<Animator>();
            animator.speed = 1;
            animator.Play($"ani_birth0", 0, 0);
        }

        /// <summary>
        /// 战斗开始
        /// </summary>
        /// <param name="role"></param>
        public override void OnFightEnter(BattleRoleBase role)
        {
            base.OnFightEnter(role);

            if (this.OnHeroFightEnter != null)
            {
                this.OnHeroFightEnter(role);
            }

        }

        public override void OnFightStart(BattleRoleBase role)
        {
            base.OnFightStart(role);

            if (this.OnHeroFightStart != null)
            {
                this.OnHeroFightStart(role);
            }
        }

        /// <summary>
        /// 战斗结束
        /// </summary>
        /// <param name="role"></param>
        public override void OnFightEnd(BattleRoleBase role)
        {
            base.OnFightEnd(role);

            if (this.OnHeroFightEnd != null)
            {
                this.OnHeroFightEnd(role);
            }
        }

        public override void OnFightExit(BattleRoleBase role)
        {
            base.OnFightExit(role);

            if (this.OnHeroFightExit != null)
            {
                this.OnHeroFightExit(role);
            }

        }

        /// <summary>
        /// 打开地牢宝藏
        /// </summary>
        /// <param name="treasure"></param>
        public void OpenTreasure(Dungeon_Treasure treasure)
        {
            this.OnHeroOpenTreasure(treasure);
        }

        /// <summary>
        /// 重制战斗属性
        /// </summary>
        public void RestBattleBaseAtt()
        {
            // 角色基础属性
            UserData userData = DataManager.I.Get<UserData>(DataDefine.UserData);
            DungeonData dungeonData = DataManager.I.Get<DungeonData>(DataDefine.DungeonData);
            this.OriAtt = MathUtils.GetHeroAttByLv(dungeonData.Data.HeroLv);

            // 装备
            //List<Data_Equip> equipList = dungeonData.Data.EquipList;
            Dictionary<EquipPointEnum, Data_Equip> equipList = dungeonData.Data.EquipPoints;
            foreach (var item in equipList)
            {
                BattleRoleAtt roleAtt = MathUtils.GetEquipAtt(item.Value);
                this.OriAtt += roleAtt;
            }

            foreach (StatueEnum item in Enum.GetValues(typeof(StatueEnum)))
            {
                if (item != StatueEnum.None)
                {
                    int statueLv = userData.GetStatueLv(item);
                    // 等级
                    int lv = statueLv / 100;
                    int expLv = statueLv % 100;


                    int hp = MathUtils.GetStatueHp(lv, expLv);
                    int atk = MathUtils.GetStatueAtk(lv, expLv);

                    this.OriAtt.MaxHP += hp;
                    this.OriAtt.Attack += atk;
                }
            }

            // 攻击
            float atkBuff = MathUtils.GetStatueBuff(StatueEnum.Colossus);
            this.OriAtt.Attack = this.OriAtt.Attack * (1 + atkBuff);

            // 血量
            float hpBuff = MathUtils.GetStatueBuff(StatueEnum.GoldApple);
            this.OriAtt.MaxHP = this.OriAtt.MaxHP * (1 + hpBuff);
            this.OriAtt.HP = this.OriAtt.MaxHP;

            // 闪避
            float dodgeBuff = MathUtils.GetStatueBuff(StatueEnum.MusicalKey);
            this.OriAtt.Dodge = this.OriAtt.Dodge * (1 + dodgeBuff);

            // 攻击回血
            float regBuff = MathUtils.GetStatueBuff(StatueEnum.Excalibur);
            this.OriAtt.HPReg = this.OriAtt.HPReg * (1 + regBuff);

            // 暴击率
            float critBuff = MathUtils.GetStatueBuff(StatueEnum.Dinosaur);
            this.OriAtt.Crit = this.OriAtt.Crit * (1 + critBuff);

            // 抗暴击率
            float decritBuff = MathUtils.GetStatueBuff(StatueEnum.Heart);
            this.OriAtt.DeCrit = this.OriAtt.DeCrit * (1 + decritBuff);

            // 命中
            float hitBuff = MathUtils.GetStatueBuff(StatueEnum.Scarecrow);
            this.OriAtt.Hit = this.OriAtt.Hit * (1 + hitBuff);

            this.OriAtt.Attack = (int)this.OriAtt.Attack;
            this.OriAtt.MaxHP = (int)this.OriAtt.MaxHP;
            this.OriAtt.HP = (int)this.OriAtt.HP;

        }
    }
}

